//����ģ��汾V3
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSViewAdminService.Core;
using CSViewAdminService.Filters;
using CSViewAdminService.Utility;
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
using CSViewAdminService.Utility.LoggerHelper;
using Microsoft.Extensions.FileProviders;

namespace CSViewAdminService
{
    /// <summary>
    /// ������
    /// </summary>
    public class Startup
    {

        private IServiceCollection services;
        private IDisposable callbackRegistration;
        /// <summary>
        /// ���캯��
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
        /// �����´�appsettings����
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeCallBack(object obj)
        {
            callbackRegistration?.Dispose();
            GetJsonConfig();
            //����ע��callback���´�appsettings.josn���º���Զ�����
            callbackRegistration = Configuration.GetReloadToken().RegisterChangeCallback(ChangeCallBack, obj);
        }

        /// <summary>
        /// ��ȡ�����ļ�
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
            //����Ĭ��ModelState��Ϊ
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //ȫ������Json���л�����
            services.AddControllers(ops =>
            {
                ops.Filters.Add(new WebApiExceptionFilterAttribute());
                ops.Filters.Add(new WebApiTrackerAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson(options =>
             {
                 //����ʱ���ʽ
                 options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                 options.SerializerSettings.ContractResolver = new CustomContractResolver();
             });
            ////System.Text.Json������
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new LongToStringConverter());
            //    options.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter());
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //});
            services.AddSwaggerGen(options =>
                {
                    //options.SwaggerDoc("v1", new OpenApiInfo { Title = "CSViewAdminService API", Version = "v1" });
                    options.SwaggerDoc("UserManage", new OpenApiInfo { Title = "����ģ��API", Version = "UserManage" });
                    options.DescribeAllEnumsAsStrings();
                    var basePath = AppContext.BaseDirectory;


                    //�Զ�����XML
                    IFileProvider fileProvider = new PhysicalFileProvider(basePath);
                    var contents = fileProvider.GetDirectoryContents("");
                    foreach (var content in contents)
                    {
                        if (content.Name.EndsWith(".xml"))
                        {
                            options.IncludeXmlComments(Path.Combine(basePath, content.Name));
                        }
                    }

                    //options.IncludeXmlComments(Path.Combine(basePath, "CSViewAdminService.xml"));
                    //options.IncludeXmlComments(Path.Combine(basePath, "CSViewAdminService.Core.xml"));
                    //options.IncludeXmlComments(Path.Combine(basePath, "CSViewAdminService.Basic.Abstractions.xml"));
                    //options.IncludeXmlComments(Path.Combine(basePath, "CSViewAdminService.AppLayer.xml"));
                });
            services.AddHealthChecks();
            //ע�����
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
            ////���ÿ��Է��ʵķ�wwwroot�ļ���
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //       Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles")),
            //    RequestPath = "/UploadFiles" //��д��һ������·����
            //});
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                // options.SwaggerEndpoint($"/swagger/v1/swagger.json", "CSViewAdminService API V1");
                options.SwaggerEndpoint("/swagger/UserManage/swagger.json", "����ģ��");

            });
            app.UseHealthChecks("/health");
        }
    }
}
