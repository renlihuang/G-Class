using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DCS.TaskManage.FrameWork
{
    class TaskEvenThread: TaskThread
    {
        /// <summary>
        /// 消息循环
        /// </summary>
        TaskEvenLoop _taskEvenLoop;
        /// <summary>
        /// 手动重置事件
        /// </summary>
        ManualResetEvent _manulResetEvent;

        /// <summary>
        /// 是否循环
        /// </summary>
        bool _isLoop;

        public IEventLoop EvenLoop
        {
            get { return _taskEvenLoop; }
        }


        public TaskEvenThread()
        {
            _manulResetEvent = new ManualResetEvent(false);
            //
            _taskEvenLoop = new TaskEvenLoop();
            //关联回调
            _taskEvenLoop._prePostEventCallBack = PrePostEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            if (!_isLoop)
            {
                _isLoop = true;
            }
            return base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            if (_isLoop)
            {
                _isLoop = false;
                //使线程退出
                _manulResetEvent.Set();
            }
 

            base.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void RunTask()
        {
            if (_isLoop)
            {
                //等待信号状态
                _manulResetEvent.WaitOne();
                //处理任务
                _taskEvenLoop.RunTaskEvenLoop();
                //重置信号状态
                _manulResetEvent.Reset();
            }
        }

        /// <summary>
        /// 投递任务之前的回调
        /// </summary>
        /// <param name="operation"></param>
        private void PrePostEvent(TaskEvenLoop.TaskEventOperation operation)
        {
            //有事件投递唤醒线程
            _manulResetEvent.Set();
        }
    }
}
