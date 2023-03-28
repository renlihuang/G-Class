using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using DCS.CORE.Interface;

namespace DCS.TaskManage.FrameWork
{
    


    class TaskEvenLoop : IEventLoop
    {
        /// <summary>
        /// 任务队列元素
        /// </summary>
        public class TaskEventOperation
        {
            /// <summary>
            /// 有参调用
            /// </summary>
            Action _actionNoParam { get; set; }

            /// <summary>
            /// 无参调用
            /// </summary>
            Action<object> _actionParam { get; set; }

            /// <summary>
            /// 参数传参
            /// </summary>
            object _param;

            /// <summary>
            /// 构造无参的对象
            /// </summary>
            /// <param name="action"></param>
            public TaskEventOperation(Action action)
            {
                _actionNoParam = action;
                _actionParam = null;
                _param = null;
            }

            /// <summary>
            /// 构造有参的对象
            /// </summary>
            /// <param name="action"></param>
            /// <param name="param"></param>
            public TaskEventOperation(Action<object> action,object param)
            {
                _actionParam = action;
                _param = param;
                _actionNoParam = null;
            }

            /// <summary>
            /// 调用方法
            /// </summary>
            public void Invoke()
            {
                //防止业务代码中的异常导致整个进程翻车
                if (_actionNoParam != null)
                {
                    _actionNoParam();
                }
                else if (_actionParam != null)
                {
                    _actionParam(_param);
                }
            }
        }


        /// <summary>
        /// 执行超时时间
        /// </summary>
        public readonly int _timerout = 10000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        public delegate void PrePostEvent(TaskEventOperation operation);

        /// <summary>
        /// 事件在未处理前的回调
        /// </summary>
        public  PrePostEvent _prePostEventCallBack;

        /// <summary>
        /// 任务队列
        /// </summary>
        ConcurrentQueue<TaskEventOperation> _taskQueue;

        /// <summary>
        /// LOG操作
        /// </summary>
        ILogOperator _logOperator;


        public TaskEvenLoop()
        {
            _prePostEventCallBack = null;
            //创建任务队列
            _taskQueue = new ConcurrentQueue<TaskEventOperation>();
        }


        /// <summary>
        /// 设置LOG操作
        /// </summary>
        /// <param name="logOperator"></param>
        public void SetLogOperator(ILogOperator logOperator)
        {
            _logOperator = logOperator;
        }

        /// <summary>
        /// 添加任务到队列
        /// </summary>
        /// <param name="action"></param>
        public void PostTaskEvent(Action action)
        {
            TaskEventOperation eventOperation = new TaskEventOperation(action);
            //添加到队列
            _taskQueue.Enqueue(eventOperation);

            //发送通知
            if (_prePostEventCallBack != null)
            {
                _prePostEventCallBack(eventOperation);
            }
        }

        /// <summary>
        /// 添加任务到队列
        /// </summary>
        /// <param name="action"></param>
        public void PostTaskEvent(Action<object> action,object param)
        {
            TaskEventOperation eventOperation = new TaskEventOperation(action, param);
            //添加到队列
            _taskQueue.Enqueue(eventOperation);
            //发送通知
            if (_prePostEventCallBack != null)
            {
                _prePostEventCallBack(eventOperation);
            }
        }

        /// <summary>
        /// 执行队列队列里的委托
        /// </summary>
        public void RunTaskEvenLoop()
        {
            TaskEventOperation eventOperation = null;

            int nowTickCount = Environment.TickCount;
            //队列不为空循环执行委托
            while (!_taskQueue.IsEmpty)
            {
                //如果执行超过指定时间主动退出
                if (Environment.TickCount - nowTickCount >= _timerout)
                {
                    break;
                }
                //取出队列元素
                if (_taskQueue.TryDequeue(out eventOperation))
                {
                    //调用委托
                    try
                    {
                        eventOperation.Invoke();
                    }
                    catch (Exception ex)
                    {
                        if (_logOperator != null)
                        {
                            //记录异常信息
                            _logOperator.AddLog(LogType.userLog, "任务队列", "任务队列调用异常", string.Format("调用任务异常,原因为:{0} 调用堆栈信息:",ex.Message));
                            //记录调用堆栈信息
                            _logOperator.AddLog(LogType.userLog, "任务队列", "任务队列调用异常", ex.StackTrace);
                        }
                    }
           
                }
              
            }
        }
    }
}
