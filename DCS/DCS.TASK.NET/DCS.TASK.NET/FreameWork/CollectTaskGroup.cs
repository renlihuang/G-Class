using DCS.CORE.Interface;
using DCS.OpcClient;
using DCS.TASK.NET.Common;
using DCS.TASK.NET.Config;
using DCS.TASK.NET.ViewModel.Base;
using DCS.TaskManage.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.TaskManage.FrameWork
{
    /// <summary>
    /// 任务组状态
    /// </summary>
    public class TaskGroupStatus
    {
        //改变的任务组
        public CollectTaskGroup collectTask { set; get; }
        //当前状态
        public bool nowStatus { set; get; }
    }

    /// <summary>
    /// 任务组，可以是采集任务和定时任务组
    /// </summary>
    ///
    public class CollectTaskGroup : TaskTimer
    {
        /// <summary>
        /// 任务组信息
        /// </summary>
        TaskGroupInfo _taskGroupInfo;

        /// <summary>
        /// 
        /// </summary>
        TaskEvenLoop _taskEvenLoop;

        /// <summary>
        /// 操作PLC的类
        /// </summary>
        IOpcOperator _opcUaOperator;

        /// <summary>
        /// log操作
        /// </summary>
        ILogOperator _logOperator;

        /// <summary>
        /// 连接状态
        /// </summary>
        bool  _connectStatus;


        /// <summary>
        /// 设置任务目录名
        /// </summary>
        string _taskDicrectoryName;
        public string TaskDicrectoryName
        {
            set
            {
                _taskDicrectoryName = value;
                //设置目录
                _taskEvenLoop.PostTaskEvent(() =>
                {
                    //以任务目录
                    _logOperator.SetTaskDicrectoryName(_taskDicrectoryName);
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskGroupInfo GetTaskGroupInfo()
        {
            return _taskGroupInfo;
        }

        /// <summary>
        /// 获取异步执行接口
        /// </summary>
        /// <returns></returns>
        public IEventLoop GetEvenLoop()
        { 
            return _taskEvenLoop;
        }

        /// <summary>
        /// 获取LOG操作接口
        /// </summary>
        /// <returns></returns>
        public ILogOperator GetLogOperator()
        {
            return _logOperator;
        }

        /// <summary>
        /// 获取OPC操作接口
        /// </summary>
        /// <returns></returns>
        public IOpcOperator GetOpcOperator()
        {
            return _opcUaOperator;
        }

        /// <summary>
        /// 创建一个任务组
        /// </summary>
        /// <param name="taskGroupInfo"></param>
        public CollectTaskGroup(TaskGroupInfo taskGroupInfo)
        {
            //LOG操作
            _logOperator = new LogHelp();
            //创建eventLoop
            _taskEvenLoop = new TaskEvenLoop();
            //设置LOG操作接口
            _taskEvenLoop.SetLogOperator(_logOperator);
            //5S检测一次PLC连接
            this.SetInterval(5000);
            //任务组信息
            _taskGroupInfo = taskGroupInfo;
            //设置LOG名称
            SetLogDirectoryName();

            ///是采集任务组就需要创建PLC连接
            if (taskGroupInfo.NodeType == TaskTreeNodeType.TaskCollectGroup)
            {
                //创建opc ua连接客户端
                _opcUaOperator = OpcUaHelp.CreateInstance();
                //设置LOG操作
                _opcUaOperator.SetLogOperator(_logOperator);
                //更新NodeID命名空间
                UpdateNameSpaceIndex();
            }
        }




        /// <summary>
        /// 更新OPC命名空间索引
        /// </summary>
        public void UpdateNameSpaceIndex()
        {
            _taskEvenLoop.PostTaskEvent(() =>
            {
                if (_opcUaOperator != null)
                {
                    //设置命名空间索引
                    _opcUaOperator.SetNodeNS(_taskGroupInfo.NodeNS);
                }
            });
        }

        /// <summary>
        /// 任务组名称改变
        /// </summary>
        public void UpdateGroupName()
        {
             //更新LOG名称
            _taskEvenLoop.PostTaskEvent(() =>
            {
                SetLogDirectoryName();
            });
        }


        public void SetLogDirectoryName()
        {
            _logOperator.SetTaskGroupName(_taskGroupInfo.GroupName);
        }

        /// <summary>
        /// 发送OPC的连接状态
        /// </summary>
        /// <param name="connectStatus"></param>
        void UpdateConnectStatus(bool connectStatus)
        {
            //如果状态改变就发送通知
            if (_connectStatus != connectStatus)
            {
                TaskGroupStatus taskGroupStatus = new TaskGroupStatus() { collectTask = this, nowStatus = connectStatus};
                //投递消息
                Global.GlobalMessageQueue.PostMessage(new OpcStatusItem() { taskGroupStatus = taskGroupStatus });
                //保存上一次的状态
                _connectStatus = connectStatus;
            }
        }

        /// <summary>
        /// 系统初始化
        /// </summary>
        public override void OnStartUp()
        {
            //以任务组名称设置LOG的的根目录
            UpdateGroupName();
            //执行连接OPC操作
            if (_opcUaOperator != null)
            {
                //连接OPC
                ConnectOpcUaServer();
            }
        }


        /// <summary>
        /// 停止时断开连接
        /// </summary>
        public override void Stop()
        {
            _taskEvenLoop.PostTaskEvent(()=>
            {
                _opcUaOperator.Close();
            });
            //停止现场
            base.Stop();
        }


        /// <summary>
        /// 连接PLC
        /// </summary>
        private void ConnectOpcUaServer()
        {
            bool isConected = _opcUaOperator.Open(_taskGroupInfo.OpcUaUrl);
            //设置状态改变
            UpdateConnectStatus(isConected);
        }


        /// <summary>
        /// 投递一个重联请求
        /// </summary>
        public void PostReconnect()
        {
            //执行连接OPC操作
            if (_opcUaOperator != null)
            {
                _taskEvenLoop.PostTaskEvent((Action)ReconnectOpcUaServer);
            }
        }


        /// <summary>
        /// 重新连接
        /// </summary>
        private void ReconnectOpcUaServer()
        {
            //先断开旧连接
            _opcUaOperator.Close();
            //连接PLC
            bool isConnect = _opcUaOperator.Open(_taskGroupInfo.OpcUaUrl);
            //更新连接状态
            UpdateConnectStatus(isConnect);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void RunTask()
        {
            //执行事件循环
            _taskEvenLoop.RunTaskEvenLoop();
            //继续传递
            base.RunTask();
        }

        /// <summary>
        /// 执行定时任务
        /// </summary>
        public override void RunTimeTask()
        {
            //保存LOG
            _logOperator.SaveLog();
            //检测PLC的连接状态
            if (_opcUaOperator != null)
            {
                //如果连接断开就重新连接
                if (!_opcUaOperator.GetConnectStatus())
                {
                    //设置状态改变
                    UpdateConnectStatus(false);
                    //重新连接PLC
                    ReconnectOpcUaServer();
                }
            }
        }   
    }
}
