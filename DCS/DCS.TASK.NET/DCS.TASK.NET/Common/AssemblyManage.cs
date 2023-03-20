
using DCS.CORE.Interface;
using DCS.TASK.NET.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace DCS.TaskManage.FrameWork
{
    /// <summary>
    /// 程序集管理
    /// </summary>
    class AssemblyManage
    {

        /// <summary>
        /// 类名对应的Type
        /// </summary>
        Dictionary<string, Type> _dicTypes = new Dictionary<string, Type>();

        /// <summary>
        /// 过滤接口名称
        /// </summary>
        string _interfaceName = "IPeriodicTask";

        /// <summary>
        /// 类名列表
        /// </summary>
        List<string> _classNames;

        /// <summary>
        /// log操作
        /// </summary>
        ILogOperator _logOperator;

        /// <summary>
        /// ServiceProvider
        /// </summary>
        ServiceProvider _serviceProvider;

        /// <summary>
        /// 获取服务引用
        /// </summary>
        /// <returns></returns>
        public ServiceProvider GetServiceProvider()
        { 
            return _serviceProvider;
        }

        /// <summary>
        /// 设置LOG操作接口
        /// </summary>
        /// <param name="logOperator"></param>
        public void SetLogOperator(ILogOperator logOperator)
        {
            _logOperator = logOperator;
        }

        /// <summary>
        /// 获取实现类列表
        /// </summary>
        public List<string> ClassNames
        {
            get { return _classNames; }
        }


        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <returns></returns>
        public bool LoadAssembly(IServiceCollection services, Assembly assemblyFile )
        {
            bool blResult = false;
            //是否加载成功
            if (assemblyFile != null)
            {
                try
                {
                    //加载所有的类
                    Type[] types = assemblyFile.GetTypes();
                    //循环遍历程序集下的类
                    foreach (Type type in types)
                    {
                        //必须是实现类
                        if (type.IsClass)
                        {
                            //判断是否实现了对应接口
                            Type itype = type.GetInterface(_interfaceName);
                            //判断是否实现了指定接口
                            if (itype != null)
                            {
                                //加入字典
                                _dicTypes[type.Name] = type;
                            }
                        }
                    }

                    //保存类名
                    _classNames = _dicTypes.Keys.ToList();

                    //判断是否加载成功
                    if (_classNames.Count > 0)
                    {
                        blResult = true;
                        //注册程序集
                        RegisterService(services);
                    }
                }
                catch (Exception ex)
                {
                    if (_logOperator != null)
                    {
                        _logOperator.AddLog(LogType.userLog, "程序集异常", "程序集加载异常", string.Format("程序集加载异常,原因是:{0}调用堆栈信息:", ex.Message));
                        _logOperator.AddLog(LogType.userLog, "程序集异常", "程序集加载异常", ex.StackTrace);
                    }
                }

            }

            return blResult;
        }

        /// <summary>
        /// 把业务类添加到依赖注入容器中
        /// </summary>
        public void RegisterService(IServiceCollection services)
        {
            //获取依赖注入容器
            var serviceCollection = services;
            //循环遍历程序集
            foreach (var item in _dicTypes)
            {
                //每次都创建新实例
                serviceCollection.AddTransient(item.Value);
            }
            //构建依赖关系
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }



        /// <summary>
        /// 根据类名创建对象
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public IPeriodicTask CreateInstanceByClassName(string className)
        {
            IPeriodicTask periodicTask = null;
            //如果类名在字典中就创建对象
            if (_dicTypes.ContainsKey(className))
            {
                try
                {
                    //通过依赖注入容器创建对象
                    periodicTask = _serviceProvider.GetService(_dicTypes[className]) as IPeriodicTask;
                }
                catch(Exception ex) 
                {
                    if (_logOperator != null)
                    {
                        _logOperator.AddLog(LogType.userLog, "程序集异常", "反射创建对象异常", string.Format("反射创建对象异常,原因是:{0}调用堆栈信息:", ex.Message));
                        _logOperator.AddLog(LogType.userLog, "程序集异常", "反射创建对象异常", ex.StackTrace);
                    }
                }
            }
            //返回创建接口
            return periodicTask;
        }

    }
}
