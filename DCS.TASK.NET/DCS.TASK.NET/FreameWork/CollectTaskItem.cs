using DCS.CORE;
using DCS.CORE.Interface;
using DCS.TASK.NET.Common;
using DCS.TASK.NET.Config;
using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DCS.TaskManage.FrameWork
{
  
    /// <summary>
    /// 单个采集任务
    /// </summary>
    public class CollectTaskItem : TaskTimer
    {
        /// <summary>
        //关联具体业务实现类接口
        /// </summary>
        IPeriodicTask _periodicTask;

        /// <summary>
        /// 保存保存参数对应的字典
        /// </summary>
        DataMap _dataMap;

        /// <summary>
        /// 定时任务对应上下文
        /// </summary>
        TimedTaskContext _taskConext;

        /// <summary>
        /// 任务信息
        /// </summary>
        CollectTaskInfo _collectTaskInfo;

        /// <summary>
        /// 计时器用于计算任务周期
        /// </summary>
        Stopwatch _taskStopwatch = new Stopwatch();

        /// <summary>
        /// log操作
        /// </summary>
        ILogOperator _logOperator;

        /// <summary>
        /// 任务运行时间
        /// </summary>
        long _taskRuntime;

        /// <summary>
        /// 最长运行时间
        /// </summary>
        long _maxRuntime;

        /// <summary>
        /// 任务类名
        /// </summary>
        string _taskClassName;

        /// <summary>
        /// 
        /// </summary>
        IResetTaskMsg resetTask;

        /// <summary>
        /// 当前任务的运行时间
        /// </summary>
        public long TaskRuntime
        {
            get { return _taskRuntime; }
        }

        /// <summary>
        /// 操作文本
        /// </summary>
        public IGetTaskMsg TaskMsg { private set; get; }

        /// <summary>
        /// 最长运行时间
        /// </summary>
        public long MaxRuntime
        {
            set { _maxRuntime = value; }
            get { return _maxRuntime;}
        }
        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <returns></returns>
        public CollectTaskInfo GetCollectTaskInfo()
        {
            //获取运行状态
            _collectTaskInfo.IsRunming = IsRunming;
            return _collectTaskInfo;
        }

        /// <summary>
        /// 创建task实现类
        /// </summary>
        /// <returns></returns>
        public bool CanCreateTaskItem()
        {   
            //实现类有改变，就需要重新创建
            if (_taskClassName != _collectTaskInfo.ClassName)
            {
                var assemblyManage =  Global.GlobalAssemblyManage;
                //创建对象
                _periodicTask = assemblyManage.CreateInstanceByClassName(_collectTaskInfo.ClassName);
                //创建成功
                if (_periodicTask != null)
                {
                    _taskClassName = _collectTaskInfo.ClassName;
                }
            }
            
            //对象不为空说明创建成功
            return _periodicTask != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectTaskInfo"></param>
        public CollectTaskItem(CollectTaskInfo collectTaskInfo)
        {
            //设置数据字典
            _dataMap = new DataMap(collectTaskInfo.DicDataMap);
            //保存任务信息
            _collectTaskInfo = collectTaskInfo;
            //设置对应的实现类
            _periodicTask = null;

            //根据不同的类型创建上下文环境
            switch (collectTaskInfo.NodeType)
            {
                case TaskTreeNodeType.TaskTimerItem:
                    _taskConext = new CollectTaskContext();
                    break;
                case TaskTreeNodeType.TaskCollectItem:
                    _taskConext = new TimedTaskContext();
                    break;
            }
            //创建消息对象
            var taskMsg = new TaskMsg();
            //获取消息
            TaskMsg = taskMsg;
            //重置接口
            resetTask = taskMsg;
            //消息操作
            _taskConext.TaskMsgOperator = taskMsg;
            //关联DataMap
            _taskConext.DataMap = _dataMap;
        }

   

        /// <summary>
        /// 设置任务循环实列
        /// </summary>
        /// <param name="taskEvenLoop"></param>
        public void SetTaskEvenLoop(IEventLoop evenLoop)
        {
            _taskConext.EventLoop = evenLoop;
        }


        /// <summary>
        /// 设置LOG操作类
        /// </summary>
        /// <param name="logOperator"></param>
        public void SetLogOperator(ILogOperator logOperator)
        {
            _logOperator = logOperator;
            _taskConext.LogOperator = logOperator;
        }

        /// <summary>
        /// 设置OPC操作接口
        /// </summary>
        /// <param name="opcUaOperator"></param>
        public void SetOpcUaOperator(IOpcOperator opcUaOperator)
        {
            CollectTaskContext collectTaskContex = _taskConext as CollectTaskContext;

            if (collectTaskContex != null)
            {
                collectTaskContex.OpcOperator = opcUaOperator;
            }
        }


        /// <summary>
        /// 获取获取运行状态
        /// </summary>
        /// <returns></returns>
        public bool GetRunmingStatus()
        {
            return IsRunming;
        }
        /// <summary>
        /// 启动定时任务
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            bool blResult = false;
            //创建对象
            if (CanCreateTaskItem())
            {
                //设置任务运行周期
                SetInterval(_collectTaskInfo.Interval);
                //设置任务名称
                _taskConext.TaskName = _collectTaskInfo.TaskName;
                //启动工作线程
                blResult = base.Start();
            }
          
            return blResult;
        }

        /// <summary>
        /// 执行初始化
        /// </summary>

        public override void OnStartUp()
        {
            resetTask.Reset();

            try
            {
                //1执行任务开始前的初始化操作
                //2传递采集数据所需要的上下文环境到实现类
                _periodicTask.DoInit(_taskConext);
            }
            catch (Exception ex)
            {
                _logOperator.AddLog(LogType.userLog, _collectTaskInfo.TaskName, "启动任务异常", string.Format("启动任务异常异常,原因是{0} 调用堆栈信息:", ex.Message));
                //调用栈信息
                _logOperator.AddLog(LogType.userLog, _collectTaskInfo.TaskName, "启动任务异常", ex.StackTrace);
            }
            //调用父类
            base.OnStartUp();
        }


        /// <summary>
        /// 定时执行采集任务
        /// </summary>
        public override void RunTimeTask()
        {
            //开始计时
            _taskStopwatch.Start();
    
            try
            {
                //执行工作任务
                _periodicTask.DoTask();
            }
            catch (Exception ex)
            {
                _logOperator.AddLog(LogType.userLog, _collectTaskInfo.TaskName, "运行任务异常", string.Format("运行任务异常异常,原因是{0} 调用栈信息:", ex.Message));
                _logOperator.AddLog(LogType.userLog, _collectTaskInfo.TaskName, "运行任务异常", ex.StackTrace);
            }
            //停止计时
            _taskStopwatch.Stop();
            //读取任务运行时间
            _taskRuntime = _taskStopwatch.ElapsedMilliseconds;
            //获取最大运行时间
            if (_taskRuntime > _maxRuntime)
            {
                _maxRuntime = _taskRuntime;
            }
            //重置计时器
            _taskStopwatch.Reset();
        }


        /// <summary>
        /// 执行清理操作
        /// </summary>
        public override void OnCleanup()
        {
            try
            {
                //执行任务停止后的清理
                _periodicTask.DoUnInit();
            }
            catch (Exception ex)
            {
                _logOperator.AddLog(LogType.userLog, _collectTaskInfo.TaskName, "停止任务异常", string.Format("停止任务异常异常,原因是{0} 调用堆栈信息", ex.Message));
                //记录调用栈信息
                _logOperator.AddLog(LogType.userLog, _collectTaskInfo.TaskName, "停止任务异常", ex.StackTrace);
            }
        }

    }
}
