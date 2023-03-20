using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.CORE.Interface
{

    /// <summary>
    /// log类型
    /// </summary>
    public enum LogType
    { 
        //系统日志
        systemLog,
        //用户日志
        userLog
    }

    /// <summary>
    /// 基本的LOG操作
    /// </summary>
    public interface ILogOperator
    {
        /// <summary>
        /// 添加LOG
        /// </summary>
        /// <param name="LogDirectotyName"></param>
        /// <param name="LogFileName"></param>
        /// <param name="Text"></param>
        void AddLog(LogType logType, string logDirectotyName, string logFileName, string content);

        /// <summary>
        /// 保存LOG
        /// </summary>
        void SaveLog();

        /// <summary>
        /// 保存csv
        /// </summary>
        /// <param name="dicLst"></param>
        /// <param name="fullPath"></param>
        void SaveCSV(List<Dictionary<string, object>> dicLst, string fullPath);

        void ToCSVData(List<Dictionary<string, object>> dicLst, string sn, string statname);

        void ToCSVLOG(List<Dictionary<string, object>> dicLst, string statname, string sn, string name);

        /// <summary>
        /// 任务组名
        /// </summary>
        void SetTaskGroupName(string name);

        /// <summary>
        /// 任务目录名
        /// </summary>
        void SetTaskDicrectoryName(string name);
    }
}
