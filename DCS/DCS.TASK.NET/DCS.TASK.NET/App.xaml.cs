using CS.Base.AppSet;
using CS.View.Common;
using DCS.CORE.Interface;
using DCS.TASK.NET.Common;
using DCS.TaskManage.FrameWork;
using DCS.TaskManage.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DCS.TASK.NET
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// 记录LOG
        /// </summary>
        LogHelp logHelp = new LogHelp();

        /// <summary>
        ///单例处理
        /// </summary>
        SingleInstance _singleInstance = new SingleInstance() { ApplicationName = "DCS.TASK", WindowName = "DCS.TASK" };

        public App()
        {
            //注册应用程序异常回调
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            //处理其他异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// 处理线程其他异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            logHelp.AddLog(LogType.systemLog, "系统出错", "应用程序线程异常", string.Format("应用程序异常，原因是{0} 调用堆栈信息", ex.Message));
            logHelp.AddLog(LogType.systemLog, "系统出错", "应用程序线程异常", ex.StackTrace);
            //保存LOG
            logHelp.SaveLog();
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logHelp.AddLog(LogType.systemLog, "系统出错", "应用程序UI线程异常", string.Format("应用程序异常，原因是{0} 调用堆栈信息", e.Exception.Message));
            logHelp.AddLog(LogType.systemLog, "系统出错", "应用程序UI线程异常", e.Exception.StackTrace);
            //保存LOG
            logHelp.SaveLog();
        }



        /// <summary>
        /// 启动类
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //检查当前实例
            _singleInstance.CheckApplicationMutex();
            //启动类
            StartUp.OnStartup();

            AppConfig.SetAppSettings(ConfigurationManager.AppSettings);
        }
    }
}
