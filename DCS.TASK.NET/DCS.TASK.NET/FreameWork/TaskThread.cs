using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DCS.TaskManage.FrameWork
{
    public class TaskThread
    {
        /// <summary>
        /// 当前处理线程
        /// </summary>
        Thread _curThread;

        /// <summary>
        /// 是否运行
        /// </summary>
        bool _isRunming;

        /// <summary>
        /// 默认线程休眠100毫秒
        /// </summary>
        protected readonly int _sleepTime = 100;

        /// <summary>
        /// 获取当前运行状态
        /// </summary>
        public bool IsRunming
        {
            get { return _isRunming; }
        }

        /// <summary>
        /// 获取当前线程
        /// </summary>
        public Thread CurThread
        {
            get { return _curThread; }
        }


        /// <summary>
        /// 开启线程
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            //如果是正在运行就不启动线程
            if (!_isRunming)
            {
                //设置正在运行
                _isRunming = true;

                _curThread = new Thread(ThreadFunc);
                //设置为后台线程
                _curThread.IsBackground = true;
                //启动线程
                _curThread.Start();
            }

            return _isRunming;
        }


        /// <summary>
        /// 停止任务
        /// </summary>
        public virtual void Stop()
        {
            if (_isRunming)
            {
                _isRunming = false;
                //等待线程结束
                if (!_curThread.Join(_sleepTime))
                {
                    //强制停止线程
                   // _curThread.Abort();
                }

                //执行清理操作
                OnCleanup();
            }
        }

        /// <summary>
        /// 线程回调
        /// </summary>
        public void ThreadFunc()
        {
            //执行初始化
            OnStartUp();
            //循环执行
            while (_isRunming)
            {
                RunTask();
                Thread.Sleep(_sleepTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RunTask()
        {
            //不覆写直接抛出异常
            throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void OnStartUp()
        { 
        }

        /// <summary>
        /// 执行清理操作
        /// </summary>
        public virtual void OnCleanup()
        {  
        }

    }
}
