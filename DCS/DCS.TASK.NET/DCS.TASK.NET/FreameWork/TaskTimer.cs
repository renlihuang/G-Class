using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.TaskManage.FrameWork
{
    /// <summary>
    /// 线程定时器
    /// </summary>
    public class TaskTimer: TaskThread
    {
        /// <summary>
        /// 设置计数
        /// </summary>
        int _setInterval;

        /// <summary>
        /// 上一次的时间戳
        /// </summary>
        int _oldTickCount;

        /// <summary>
        /// 最大运行周期不超过10S
        /// </summary>
        readonly int _maxInterval = 10000;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            //启动时获取当前时间戳
            _oldTickCount = Environment.TickCount;
            //启动线程
            return base.Start();
        }

        /// <summary>
        /// 设置运行间隔
        /// </summary>
        /// <param name="Interval"></param>
        public void SetInterval(int Interval)
        {
            _setInterval = Interval;

            if (Interval < _sleepTime)
            {
                _setInterval = _sleepTime;
            }

            if (Interval > _maxInterval)
            {
                _setInterval = _maxInterval;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStartUp()
        {
            //执行定时任务
            RunTimeTask();
        }

        /// <summary>
        /// 定时任务回调
        /// </summary>
        public virtual void RunTimeTask()
        {
            //不覆写直接抛出异常
            throw new NotImplementedException();
        }

        /// <summary>
        /// 按指定时间执行
        /// </summary>
        public override void RunTask()
        {
           int nowTick = Environment.TickCount;
            ///事件到执行定时任务
            if ((nowTick - _oldTickCount) >= _setInterval)
            {
                //执行定时任务
                RunTimeTask();
                //更新时间
                _oldTickCount = nowTick;
            }
        }
    }
}
