﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CS.View.Common
{
    class IniFile
    {
        private string _path; //INI档案名 
        public string IniPath { get { return _path; } set { _path = value; } }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STRINGBUFFER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szText;
        }

        //读写INI文件的API函数 
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string section, string key, string def, out STRINGBUFFER retVal, int size, string filePath);

        //类的构造函数，传递INI档案名 
        public IniFile(string iniFileName)
        {
            string iniFilePath = AppDomain.CurrentDomain.BaseDirectory + "Config\\";

            if (!Directory.Exists(iniFilePath))
            {
                //创建目录
                Directory.CreateDirectory(iniFilePath);
            }

            _path = iniFilePath + iniFileName;
            if (!File.Exists(_path))
            {
                CreateIniFile();
            }
        }

        //写INI文件 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this._path);
        }

        //读取INI文件指定 
        public string IniReadValue(string Section, string Key)
        {
            int i;
            STRINGBUFFER RetVal;
            i = GetPrivateProfileString(Section, Key, null, out RetVal, 255, this._path);
            string temp = RetVal.szText;
            return temp.Trim();
        }

        /// <summary>
        /// 创建INI文件
        /// </summary>
        public void CreateIniFile()
        {
            StreamWriter w = File.CreateText(_path);
            w.Write("");
            w.Flush();
            w.Close();
        }
    }
}
