using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Config
{
    public class TaskRootConfig: TaskBaseModel
    {
        /// <summary>
        /// 全局字典
        /// </summary>
        public ConcurrentDictionary<string, string> GlobalDataMap { private set; get;} = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 任务树
        /// </summary>
        public List<TaskDicrectory> TaskDicrectories { private set; get; } = new List<TaskDicrectory>();
    }
}
