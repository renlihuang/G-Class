
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
        /// Mes配置文档地址
        /// </summary>

        static string _mesFilePath;

        public static string MesFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_mesFilePath))
                {
                    _mesFilePath = CurrentAppSettings["MesfilePath"];
                }

                return _mesFilePath;
            }
        }

        /// <summary>
        /// webserviceini文件路径
        /// </summary>
        static string _webserviceiniPath;

        public static string WebserviceiniPath
        {
            get
            {
                if (string.IsNullOrEmpty(_webserviceiniPath))
                {
                    _webserviceiniPath = CurrentAppSettings["WebserviceiniPath"];
                }
                return _webserviceiniPath;
            }
        }

        /// <summary>
        /// mesdata地址
        /// </summary>
        static string _mesdataPath;

        public static string MesdataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_mesdataPath))
                {
                    _mesdataPath = CurrentAppSettings["MesdataPath"];
                }

                return _mesdataPath;
            }
        }

        /// <summary>
        /// meslog地址
        /// </summary>
         static string _meslogPath;

        public static string MeslogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_meslogPath))
                {
                    _mesdataPath = CurrentAppSettings["MeslogPath"];
                }

                return _meslogPath;
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
