using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.TaskStatus
{
    /// <summary>
    /// 操作数据接口
    /// </summary>
    internal interface IUpdateStatus
    {
        /// <summary>
        /// 重新加载数据
        /// </summary>
        void ReloadData();

        /// <summary>
        /// 更新数据
        /// </summary>
        void UpdateData();
    }
}
