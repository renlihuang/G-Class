using DCS.BASE.Config;
using DCS.TaskManage.FrameWork;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{
    /// <summary>
    /// 全局公共类
    /// </summary>
    class Global
    {
        /// <summary>
        /// 全局字典
        /// </summary>
        static ConcurrentDictionary<string, string> _globalDataMap;
        public static  ConcurrentDictionary<string, string> GlobalDataMap 
        { 
            set { GlobalConfig.SetDataMap(value); _globalDataMap = value; } 
            get { return _globalDataMap; } 
        }
        /// <summary>
        /// 全局程序集管理器
        /// </summary>
        public static AssemblyManage GlobalAssemblyManage { private set; get; } = new AssemblyManage();
        /// <summary>
        /// 全局依赖注入容器
        /// </summary>
        public static  ServiceCollection GlobalServiceCollection { private set; get; } = new ServiceCollection();
        /// <summary>
        ///全局消息队列
        /// </summary>
        public static MessageQueue  GlobalMessageQueue { private set; get; } = new MessageQueue();

    }
}
