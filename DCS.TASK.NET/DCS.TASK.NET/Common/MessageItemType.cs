using DCS.TASK.NET.ViewModel.Base;
using DCS.TaskManage.FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{
    public enum MessageType
    {
        /// <summary>
        /// OPC UA连接状态
        /// </summary>
        OpcUaStatus,

        /// <summary>
        /// 添加LOG
        /// </summary>
        AddLog,
    }

    /// <summary>
    /// 基本消息
    /// </summary>
    public interface MessageItemBase
    {
        //消息类别属性
        MessageType MessageType { get; }
    }

    /// <summary>
    /// LOG消息
    /// </summary>
    class logMessageItem: MessageItemBase
    {
        /// <summary>
        /// LOG数据
        /// </summary>
        public LogItemViewModel logItem { set; get; }

        /// <summary>
        /// 实现属性
        /// </summary>
        public MessageType MessageType { get; } = MessageType.AddLog;
    }

    /// <summary>
    /// OPC连接状态改变
    /// </summary>
    class OpcStatusItem : MessageItemBase
    {
        /// <summary>
        /// 任务组状态
        /// </summary>
        public TaskGroupStatus  taskGroupStatus { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public MessageType MessageType { get; } = MessageType.OpcUaStatus;
    }
}



