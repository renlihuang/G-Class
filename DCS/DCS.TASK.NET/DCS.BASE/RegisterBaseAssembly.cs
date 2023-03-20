using DCS.CORE.Interface;
using DCS.OpcClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.BASE
{
    public  class RegisterBaseAssembly
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void RegisterService(IServiceCollection serviceCollection)
        {
            //添加注入HTTP服务
            serviceCollection.AddHttpClient();
            //注入HTTP请求类
            serviceCollection.AddScoped<RequestToHttpHelper>();
        }
    }
}
