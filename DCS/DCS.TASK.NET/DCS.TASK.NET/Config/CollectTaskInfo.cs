using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Config
{
    /// <summary>
    /// 任务信息
    /// </summary>
    public class CollectTaskInfo: TaskBaseModel
    {
        /// <summary>
        ///任务名称
        /// </summary>
        public string TaskName { set; get; }

        /// <summary>
        ///关联的程序集类名
        /// </summary>
        public string ClassName { set; get; }

        /// <summary>
        /// 定时间隔
        /// </summary>
        public int Interval { set; get; }


        /// <summary>
        ///是否运行
        /// </summary>
        public bool IsRunming { set; get; }

        /// <summary>
        /// 保存参数对应的字典
        /// </summary>
        public Dictionary<string, string> DicDataMap = new Dictionary<string, string>();
    }
}
