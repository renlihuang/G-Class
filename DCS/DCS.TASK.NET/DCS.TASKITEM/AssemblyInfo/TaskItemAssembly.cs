using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASKITEM
{
    /// <summary>
    /// 这个不要删
    /// </summary>
    public  class TaskItemAssembly
    {
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetAssembly()
        { 
            return Assembly.GetExecutingAssembly();
        }
    }
}
