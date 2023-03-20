using DCS.BASE.IniFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{
    /// <summary>
    /// 一些配置文件的操作
    /// </summary>
    internal class IniFileConfig
    {
        /// <summary>
        /// 当前实列
        /// </summary>
        public readonly static IniFileConfig Current = new IniFileConfig();

        /// <summary>
        /// 当前配置文件名
        /// </summary>
        string _iniFileName = System.Environment.CurrentDirectory + "\\config.ini";

        private IniFileConfig()
        {
            CanCreateFile();
        }

        /// <summary>
        /// 获取的值转换为BOOL
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetBoolValue(string section, string key)
        {
            bool result = false;

            string value = GetStringValue(section, key);
            //转换为BOOL
            if (!string.IsNullOrEmpty(value))
            {
                bool.TryParse(value, out result);
            }

            return result;
        }


        /// <summary>
        /// 获取的值转换为BOOL
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetIntValue(string section, string key)
        {
            int result = -1;

            string value = GetStringValue(section, key);
            //转换为BOOL
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out result);
            }

            return result;
        }


        /// <summary>
        /// 获取的值转换为BOOL
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetStringValue(string section, string key)
        {
            string result = string.Empty;

            if (File.Exists(_iniFileName))
            {
                result = IniFileAPI.INIGetStringValue(_iniFileName, section, key, "");
            }

            return result;
        }

        /// <summary>
        /// 创建配置文件
        /// </summary>
        private void CanCreateFile()
        {
            if (!File.Exists(_iniFileName))
            {
                File.Create(_iniFileName);
                //写入一个默认值
            }
            //写入一个默认值
            if (string.IsNullOrEmpty(GetStringValue("UserConfig", "IsManage")))
            {
                IniFileAPI.INIWriteValue(_iniFileName, "UserConfig", "IsManage", "0");
            }
        }

    }
}
