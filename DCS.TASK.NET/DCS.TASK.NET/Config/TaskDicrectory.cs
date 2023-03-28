using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Config
{
    /// <summary>
    /// 任务目录
    /// </summary>
    public class TaskDicrectory: TaskBaseModel
    {
        /// <summary>
        /// 目录名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 任务树
        /// </summary>
        public List<TaskGroupInfo> TaskGroups { private set; get; } = new List<TaskGroupInfo>();
    }
}
