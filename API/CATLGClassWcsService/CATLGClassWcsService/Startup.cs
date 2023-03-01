//代码模板版本V3
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CATLGClassWcsService.Core;
using CATLGClassWcsService.Filters;
using CATLGClassWcsService.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using MediatR;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json;
using CATLGClassWcsService.Utility.LoggerHelper;
using Microsoft.Extensions.FileProviders;

namespace CATLGClassWcsService
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {

        private IServiceCollection services;
        private IDisposable callbackRegistration;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .Build();
            callbackRegistration = Configuration.GetReloadToken().RegisterChangeCallback(ChangeCallBack, Configuration);

        }



        /// <summary>
        /// 更新下次appsettings重载
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeCallBack(object obj)
        {
            callbackRegistration?.Dispose();
            GetJsonConfig();
            //重新注册callback，下次appsettings.josn更新后会自动调用
            callbackRegistration = Configuration.GetReloadToken().RegisterChangeCallback(ChangeCallBack, obj);
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        private void GetJsonConfig()
        {

            AppsettingsConfig config = new AppsettingsConfig();
            Configuration.Bind("WebConfig", config);


            //var serviceApiHostConfig = Configuration.GetSection("ServiceApiHosts")?.Get<List<ServiceApiHostModel>>();
            
            //var serviceApiHostConfig = new List<ServiceApiHostModel>();
            //Configuration.Bind("ServiceApiHosts", serviceApiHostConfig);
            
            //AppsettingsConfig.ServiceApiHosts = new List<ServiceApiHostModel>();
            //AppsettingsConfig.ServiceApiHosts = serviceApiHostConfig;
        }

        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            this.services = services;
            
            GetJsonConfig();
            services.AddMediatR(Assembly.GetEntryAssembly());
            //禁用默认ModelState行为
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //全局配置Json序列化处理
            services.AddControllers(ops =>
            {
                ops.Filters.Add(new WebApiExceptionFilterAttribute());
                ops.Filters.Add(new WebApiTrackerAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson(options =>
             {
                 //设置时间格式
                 options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                 options.SerializerSettings.ContractResolver = new CustomContractResolver();
             });
            ////System.Text.Json配置项
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new LongToStringConverter());
            //    options.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter());
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //});
            services.AddSwaggerGen(options =>
                {
                    //options.SwaggerDoc("v1", new OpenApiInfo { Title = "CATLGClassWcsService API", Version = "v1" });
                    options.SwaggerDoc("Basic", new OpenApiInfo { Title = "基础模块API", Version = "Basic" });
                    options.DescribeAllEnumsAsStrings();
                    var basePath = AppContext.BaseDirectory;


                    //自动加载XML
                    IFileProvider fileProvider = new PhysicalFileProvider(basePath);
                    var contents = fileProvider.GetDirectoryContents("");
                    foreach (var content in contents)
                    {
                        if (content.Name.EndsWith(".xml"))
                        {
                            options.IncludeXmlComments(Path.Combine(basePath, content.Name));
                        }
                    }

                    //options.IncludeXmlComments(Path.Combine(basePath, "CATLGClassWcsService.xml"));
                    //options.IncludeXmlComments(Path.Combine(basePath, "CATLGClassWcsService.Core.xml"));
                    //options.IncludeXmlComments(Path.Combine(basePath, "CATLGClassWcsService.Basic.Abstractions.xml"));
                    //options.IncludeXmlComments(Path.Combine(basePath, "CATLGClassWcsService.AppLayer.xml"));
                });
            services.AddHealthChecks();
            //注册服务
            RegisterService.RegisterComponents(services);
            MyLogger.AddMyLogger();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CommonServiceProvider.ServiceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            app.UseStaticHttpContext();
            app.UseStaticFiles();
            ////设置可以访问的非wwwroot文件夹
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //       Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles")),
            //    RequestPath = "/UploadFiles" //重写了一个虚拟路径。
            //});
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                // options.SwaggerEndpoint($"/swagger/v1/swagger.json", "CATLGClassWcsService API V1");
                options.SwaggerEndpoint("/swagger/Basic/swagger.json", "基础模块");

            });
            app.UseHealthChecks("/health");
        }
    }
}
