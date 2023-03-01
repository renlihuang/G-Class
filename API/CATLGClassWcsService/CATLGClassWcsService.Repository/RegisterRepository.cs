using CATLGClassWcsService.Core;
using Microsoft.Extensions.DependencyInjection;
using CATLGClassWcsService.DAL;

namespace CATLGClassWcsService.Repository
{
    /// <summary>
    /// DAL注入服务
    /// </summary>
    public static class RegisterRepository
    {
        /// <summary>
        /// 注入接口
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped(typeof(IDefaultUnitOfWorkV2<,,>), typeof(DefaultUnitOfWorkV2Impl<,,>));
            services.AddScoped(typeof(IDefaultUnitOfWork<,,>), typeof(DefaultUnitOfWorkImpl<,,>));

        }
    }
}
