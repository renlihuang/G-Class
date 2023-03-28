using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.BASE.Config
{
    //全局设置
    public class GlobalConfig
    {
        private static IDictionary<string, string> _globalDataMap;

        public static void SetDataMap(IDictionary<string, string> globalDataMap)
        {
            if (_globalDataMap == null)
            {
                _globalDataMap = globalDataMap;
            }
        }

        /// <summary>
        /// 根据key获取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringByKey(string key)
        {
            string value = string.Empty;

            if (_globalDataMap != null)
            {
                if (_globalDataMap.ContainsKey(key))
                {
                    value = _globalDataMap[key];
                }
            }    

            return value;
        }
    }
}
