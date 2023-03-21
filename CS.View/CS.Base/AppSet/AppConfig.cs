
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Base.AppSet
{
    public class AppConfig
    {
        /// <summary>
        /// 权限管理API地址
        /// </summary>

        static string _webApiHost;

        public static string WebApiHost
        {
            get
            {
                if (string.IsNullOrEmpty(_webApiHost))
                {
                    _webApiHost = CurrentAppSettings["WebApiHost"];
                }

                return _webApiHost;
            }
        }

        /// <summary>
        /// 业务API地址
        /// </summary>
        static string _businessApiHost;

        public static string BusinessApiHost
        {
            get
            {
                if (string.IsNullOrEmpty(_businessApiHost))
                {
                    _businessApiHost = CurrentAppSettings["BusinessApiHost"];
                }

                return _businessApiHost;
            }
        }


        /// <summary>
        /// 设置appsettings
        /// </summary>
        public static void SetAppSettings(NameValueCollection appSettings)
        {
            CurrentAppSettings = appSettings;
        }

        /// <summary>
        /// 当前APPsettings
        /// </summary>
        public static NameValueCollection CurrentAppSettings { get; private set; }

    }
}
