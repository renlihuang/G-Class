using DCS.BASE;
using DCS.BLL.AssemblyInfo;
using DCS.CORE.Interface;
using DCS.OpcClient;
using DCS.TASKITEM;
using DCS.TaskManage.FrameWork;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{
    static class StartUp
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        public static void OnStartup()
        {
            var service = new ServiceCollection();
            //注册
            OnRegister(service);
            //创建自动注入
            var autoInjectService = new AutoInject(service);
            //注册程序集
            RegisterBaseAssembly.RegisterService(service);
            //加载BLL程序集
            var assembly = BLLAssemblyInfo.GetAssembly();
            //注册程序集
            autoInjectService.RegisterAssembly(assembly);
            //注册程序集
            assembly = TaskItemAssembly.GetAssembly();
            //注册程序集
            Global.GlobalAssemblyManage.LoadAssembly(service, assembly);
            //加载皮肤
            ServiceFiguration.LoadSkin();
        }

        /// <summary>
        /// 注册程序集
        /// </summary>
        /// <param name="services"></param>
        private static void OnRegister(IServiceCollection services)
        {
            //注册OPC服务
            services.AddTransient<IOpcOperator, OpcUaHelp>();
        }
    }
}
