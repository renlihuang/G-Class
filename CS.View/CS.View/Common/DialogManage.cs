using CS.View.ViewModel.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CS.View.Common
{
    /// <summary>
    /// 容器的值
    /// </summary>
    internal class ServiceProviderInstacnce
    {
        public static ServiceProvider CurrentServiceProvider { get; private set; }

        /// <summary>
        /// 初始化容器值
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ServiceProviderInstacnce(ServiceProvider serviceProvider)
        {
            CurrentServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return CurrentServiceProvider.GetService<T>();
        }
    }

    internal class DialogManage
    {
        /// <summary>
        /// 注入容器接口
        /// </summary>
        private static ServiceCollection _serviceCollection;

        /// <summary>
        /// 程序集管理接口
        /// </summary>
        private ServiceProvider _serviceProvider = null;

        /// <summary>
        /// 设置注入容器
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        public static void SetServiceCollection(ServiceCollection serviceDescriptors)
        {
            _serviceCollection = serviceDescriptors;
            //设置容器
            new ServiceProviderInstacnce(_serviceCollection.BuildServiceProvider());
        }

        private List<string> _classNames;

        //过滤的接口名称
        private string _filterInterfaceName = "IBaseModel";

        /// <summary>
        /// 窗口对应的TYPE
        /// </summary>
        private Dictionary<string, Type> _dicTypes = new Dictionary<string, Type>();

        /// <summary>
        /// 创建窗口
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public IBaseModel CreateDialog(string className)
        {
            IBaseModel BaseModel = null;

            if (_dicTypes.ContainsKey(className))
            {
                Type type = _dicTypes[className];

                //通过依赖注入容器创建
                BaseModel = _serviceProvider.GetService(type) as IBaseModel;
            }

            return BaseModel;
        }

        /// <summary>
        /// 加载窗口
        /// </summary>
        /// <returns></returns>
        public async Task LoadViews()
        {
            ///加载程序集
            await Task.Run(() =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                ///获取程序集
                Type[] types = assembly.GetTypes();
                //
                foreach (Type type in types)
                {
                    if (type.IsClass)
                    {
                        Type iType = type.GetInterface(_filterInterfaceName);
                        //获取
                        if (iType != null)
                        {
                            _dicTypes[type.Name] = type;
                        }
                    }
                }

                _classNames = _dicTypes.Keys.ToList();
                //注册程序集
                RegisterDialog();
            });
        }

        /// <summary>
        /// 注册对话框
        /// </summary>
        public void RegisterDialog()
        {
            if (_dicTypes.Count > 0)
            {
                foreach (var item in _dicTypes)
                {
                    _serviceCollection.AddTransient(item.Value);
                }

                if (_serviceProvider == null)
                {
                    _serviceProvider = _serviceCollection.BuildServiceProvider();
                }
            }
        }
    }
}