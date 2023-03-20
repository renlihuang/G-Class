using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.CORE
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class LogExtendFunction
    {
        /// <summary>
        /// 添加用户LOG
        /// </summary>
        /// <param name="logOperator"></param>
        /// <param name="logDirectotyName"></param>
        /// <param name="logFileName"></param>
        /// <param name="content"></param>
        public static void AddUserLog(this ILogOperator logOperator, string logDirectotyName, string logFileName, string content)
        {
            logOperator.AddLog(LogType.userLog, logDirectotyName, logFileName, content);
        }

        /// <summary>
        /// 添加系统LOG
        /// </summary>
        /// <param name="logOperator"></param>
        /// <param name="logDirectotyName"></param>
        /// <param name="logFileName"></param>
        /// <param name="content"></param>
        public static void AddSystemLog(this ILogOperator logOperator, string logDirectotyName, string logFileName, string content)
        {
            logOperator.AddLog(LogType.systemLog, logDirectotyName, logFileName, content);
        }
        /// <summary>
        /// 异步保存LOG
        /// </summary>
        /// <param name="logOperator"></param>
        /// <returns></returns>
        public static async void SaveLogAsync(this ILogOperator logOperator)
        {
            await Task.Run(() =>
            {
                logOperator.SaveLog();
            });
        }
    }
}
