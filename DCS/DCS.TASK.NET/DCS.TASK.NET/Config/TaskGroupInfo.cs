using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Config
{
    /// <summary>
    /// 任务组信息
    /// </summary>
    public class TaskGroupInfo: TaskBaseModel
    {
        /// <summary>
        /// PLC名称
        /// </summary>
        public string GroupName { set; get; }

        /// <summary>
        /// OPC连接地址
        /// </summary>
        public string OpcUaUrl { set; get; }

        /// <summary>
        /// OPC命名空间索引信息
        /// </summary>
        public short NodeNS { get; set; }

        /// <summary>
        /// 对应的任务
        /// </summary>
        public List<CollectTaskInfo> CollectTasks { private set; get; } = new List<CollectTaskInfo>();
    }
}
