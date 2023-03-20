using DCS.CORE.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TaskManage.Log
{

    /// <summary>
    /// LOG信息
    /// </summary>
    internal class LogInfo
    {
        //log类型
        public LogType LogType { set; get; }
        //Log目录名
        public string LogDirectotyName { set; get; }

        //Log文件目录
        public string LogFileName { set; get; }

        //Log正文 
        public string Text { set; get; }

        //创建时间
        public string CreateTime { set; get; }
    }

    /// <summary>
    /// LOG操作
    /// </summary>
    class LogHelp : ILogOperator
    {
        /// <summary>
        /// 任务组名
        /// </summary>
        string _taskGroupName;

        /// <summary>
        /// 任务目录名
        /// </summary>
        string _taskDicrectoryName;

        /// <summary>
        /// 添加LOG前的回调
        /// </summary>
        /// <param name="logInfo"></param>
        public delegate void PreAddLogCallBack(LogInfo logInfo);

        /// <summary>
        /// 系统LOG回调
        /// </summary>
        PreAddLogCallBack _systemLogCallBack;

        /// <summary>
        /// 用户LOG回调
        /// </summary>
        PreAddLogCallBack _userLogCallBack;



        /// <summary>
        /// LOG队列
        /// </summary>
        ConcurrentQueue<LogInfo> _logQueue = new  ConcurrentQueue<LogInfo>();


        /// <summary>
        /// 设置系统LOG回调
        /// </summary>
        public PreAddLogCallBack SystemLogCallBack
        {
            set { _systemLogCallBack = value; }
        }

        /// <summary>
        /// 设置用户LOG回调
        /// </summary>
        public PreAddLogCallBack UserLogCallBack
        {
            set { _userLogCallBack = value; } 
        }



        /// <summary>
        /// 设置任务目录名称
        /// </summary>
        /// <param name="name"></param>
        void ILogOperator.SetTaskGroupName(string name)
        {
            _taskGroupName = name;
        }

        /// <summary>
        /// 设置任务目录名称
        /// </summary>
        /// <param name="name"></param>
        void ILogOperator.SetTaskDicrectoryName(string name)
        {
            _taskDicrectoryName = name;
        }

        /// <summary>
        /// 添加LOG
        /// </summary>
        /// <param name="LogDirectotyName"></param>
        /// <param name="LogFileName"></param>
        /// <param name="Text"></param>
        public void AddLog(LogType logType, string logDirectotyName, string logFileName, string content)
        {
            LogInfo logInfo = new LogInfo()
            {
                LogType = logType,
                LogDirectotyName = logDirectotyName,
                LogFileName = logFileName,
                Text = content,
                CreateTime = DateTime.Now.ToString()
            };

            //添加到队列
            _logQueue.Enqueue(logInfo);
      
        }

        /// <summary>
        /// 保存LOG
        /// </summary>
        public void SaveLog()
        {
            LogInfo logInfo;
            //队列不为空就写入LOG
            while (!_logQueue.IsEmpty)
            {
                if (_logQueue.TryDequeue(out logInfo))
                {
                    ProcessCallback(logInfo);
                    //写入文本文件
                    SaveTxt(logInfo);
                }
            }
        }

        /// <summary>
        /// 处理LOG回调
        /// </summary>
        /// <param name="logInfo"></param>
        private void ProcessCallback(LogInfo logInfo)
        {
            if (logInfo.LogType == LogType.systemLog)
            {
                if (_systemLogCallBack != null)
                {
                    _systemLogCallBack(logInfo);
                }
            }

            if (logInfo.LogType == LogType.userLog)
            {
                if (_userLogCallBack != null)
                {
                    _userLogCallBack(logInfo);
                }
            }
        }

        public void ToCSVData(List<Dictionary<string, object>> dicLst, string sn, string statname)
        {
            try
            {
                string sfc = sn;
                string fullpath = $@"D:\CSVDATA\{statname}\{DateTime.Now.ToString("yyyyMMdd")}\{statname}_{DateTime.Now.ToString("yyyyMMdd")}.csv";
                SaveCSV(dicLst, fullpath);
            }
            catch (Exception ex)
            {
                AddLog(LogType.userLog, statname, statname, string.Format("创建CSV出错,错误信息") + ex.Message);
            }
        }

        public void ToCSVLOG(List<Dictionary<string, object>> dicLst, string infacename, string sn, string name)
        {
            try
            {
                string sfc = sn;
                string fullpath = $@"D:\CSVLOG\{infacename}\{DateTime.Now.ToString("yyyyMMdd")}\{infacename}_{DateTime.Now.ToString("yyyyMMdd")}.csv";
                SaveCSV(dicLst, fullpath);
            }
            catch (Exception ex)
            {
                AddLog(LogType.userLog, infacename, infacename, string.Format("创建CSV出错,错误信息") + ex.Message);
            }
        }

        public void SaveCSV(List<Dictionary<string, object>> dicLst, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            // FileStream fs = new FileStream(fullPath, System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite); 旧
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            if (dicLst != null && dicLst.Count > 0)
            {
                var dicFirst = dicLst.First();

                //写出列名称
                foreach (var item in dicFirst.Keys)
                {
                    data += item + "," + dicFirst[item] + "\n";
                }
                // sw.BaseStream.Seek(0, SeekOrigin.End); 旧
                sw.WriteLine(data.TrimEnd(','));

                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 保存Txt文件
        /// </summary>
        private  void SaveTxt(LogInfo LogItem)
        {
            string filePath = string.Format(@"{0}\\", LogItem.LogType);
            //创建根目录
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            //任务目录
            if (_taskDicrectoryName != string.Empty)
            {
                filePath += string.Format("{0}\\", _taskDicrectoryName);
                //创建当前目录
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
            }

            //任务目录
            if (_taskGroupName != string.Empty)
            {
                filePath += string.Format("{0}\\", _taskGroupName);
                //创建当前目录
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
            }


            //判断当天目录
            filePath += string.Format(@"{0}\\", DateTime.Now.ToString("yyyy-MM-dd"));

            //创建当前目录
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            //判断当天目录
            filePath += string.Format(@"{0}\\", LogItem.LogDirectotyName);

            //创建当前目录
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath += string.Format("{0}.txt", LogItem.LogFileName);

            bool blIsCreate = false;

            //判断文件是否已经创建
            if (!File.Exists(filePath))
            {
                //File.CreateText(FilePath);
                blIsCreate = true;
            }

            try
            {
                //打开文件流
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    //添加时间
                    string WriteTxt = string.Format("[{0}]:{1}", LogItem.CreateTime, LogItem.Text);

                    if (blIsCreate)
                    {
                        sw.WriteLine(WriteTxt);
                    }
                    else
                    {
                        //在结尾追加
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        //Sw.WriteLine(Environment.NewLine);
                        sw.WriteLine(WriteTxt);
                    }
                    //写入文件
                    sw.Flush();
                    //关闭文件流
                    sw.Close();
                }
            }
            catch { }
        }

      

    }


}
