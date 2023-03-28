using DCS.CORE;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TaskManage.FrameWork
{
    /// <summary>
    /// 自动注入程序集对象
    /// </summary>
    class AutoInject
    {
        readonly Dictionary<string, Type> _dicInterfaceTypes = new Dictionary<string, Type>();

        /// <summary>
        /// 保存注入容器接口
        /// </summary>
        readonly IServiceCollection _serviceCollection;

        public AutoInject(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            //初始化注册类型
            InitRegisterType();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitRegisterType()
        {
            //添加注入类型
            _dicInterfaceTypes.Add("AutoInjectScoped", typeof(IAutoInjectScoped));
            _dicInterfaceTypes.Add("AutoInjectSingleton", typeof(IAutoInjectSingleton));
            _dicInterfaceTypes.Add("AutoInjectTransient", typeof(IAutoInjectTransient));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerType"></param>
        /// <param name="interfaceType"></param>
        /// <param name="classType"></param>
        private void AddRegister(string registerType, Type interfaceType, Type classType)
        {
            switch (registerType)
            {
                case "AutoInjectScoped":
                    if (interfaceType != null)
                    {
                        _serviceCollection.AddScoped(interfaceType, classType);
                    }
                    else
                    {
                        _serviceCollection.AddScoped(classType);
                    }
                    break;
                case "AutoInjectSingleton":
                    if (interfaceType != null)
                    {
                        _serviceCollection.AddSingleton(interfaceType, classType);
                    }
                    else
                    {
                        _serviceCollection.AddSingleton(classType);
                    }
                    break;
                case "AutoInjectTransient":
                    if (interfaceType != null)
                    {
                        _serviceCollection.AddTransient(interfaceType, classType);
                    }
                    else
                    {
                        _serviceCollection.AddTransient(classType);
                    }
                    break;
            }
        }

        /// <summary>
        /// 注册程序集
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="executingAssembly"></param>
        public void RegisterAssembly(Assembly executingAssembly)
        {
            if (executingAssembly != null)
            {
                //获取所有的类
                Type[] types = executingAssembly.GetTypes();
                //循环
                foreach (Type type in types)
                {
                    if (type.IsClass)
                    {
                        //获取类
                        var interfaceTypes = type.GetInterfaces().ToList();
                        //循环所有的类
                        foreach (var InterfaceType in _dicInterfaceTypes)
                        {
                            var injectList = interfaceTypes.Where(x => x == InterfaceType.Value);
                            //判断是否需要注册
                            if (injectList.Count() > 0)
                            {
                                //获取实现的接口
                                var implementTypes = interfaceTypes.Where(x => x != InterfaceType.Value);
                                //有实现其他接口就注册接口和对应的实现类
                                if (implementTypes.Count() > 0)
                                {
                                    #region 注册接口对应实现类
                                    foreach (var implementType in implementTypes)
                                    {
                                        AddRegister(InterfaceType.Key, implementType, type);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //注册类
                                     AddRegister(InterfaceType.Key, null, type);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
