using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.Base
{
    internal class LogItemViewModel
    {
        /// <summary>
        /// LOG名称
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// LOG名称
        /// </summary>
        public string LogText { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string LogTime { get; set; }
    }
}
