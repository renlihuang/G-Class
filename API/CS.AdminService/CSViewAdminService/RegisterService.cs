using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;
using System.Collections.Generic;
using CSViewAdminService.Core;
using System.Linq;
using CSViewAdminService.AppLayer;
using CSViewAdminService.Utility;
using CSViewAdminService.MediatR;
using Microsoft.AspNetCore.Http;
using CSViewAdminService.Repository;
using CSViewAdminService.UserManage;

namespace CSViewAdminService
{
    public static class RegisterService
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            //生成代码开始位置勿删
            services.AddHttpClient();
            services.AddScoped<RequestToHttpHelper>();
            services.AddSingleton<TPLLogger>();
           // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();//单例
            RegisterApp.RegisterComponents(services);
            RegisterRepository.RegisterComponents(services);
            RegisterMediatR.RegisterComponents(services);
            RegisterUserManage.RegisterComponents(services);

            //自动注入IAutoInject
            services.Scan(x =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                x.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IAutoInject)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                        //接口注册Scoped
                    .AddClasses(classes => classes.AssignableTo(typeof(IScopedAutoInject)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    //接口注册Singleton
                    .AddClasses(classes => classes.AssignableTo(typeof(ISingletonAutoInject)))
                          .AsImplementedInterfaces()
                          .WithSingletonLifetime()
                   //接口注册Transient
                    .AddClasses(classes => classes.AssignableTo(typeof(ITransientAutoInject)))
                          .AsImplementedInterfaces()
                          .WithTransientLifetime()
                    //具体类注册Scoped
                    .AddClasses(classes => classes.AssignableTo(typeof(ISelfScopedAutoInject)))
                          .AsSelf()
                          .WithScopedLifetime()
                    //具体类注册Singleton
                    .AddClasses(classes => classes.AssignableTo(typeof(ISelfSingletonAutoInject)))
                          .AsSelf()
                          .WithSingletonLifetime()
                    //具体类注册Transient
                    .AddClasses(classes => classes.AssignableTo(typeof(ISelfTransientAutoInject)))
                          .AsSelf()
                          .WithTransientLifetime();
            });
        }
    }
}