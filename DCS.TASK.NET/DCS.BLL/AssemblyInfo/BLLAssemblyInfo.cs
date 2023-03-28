using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DCS.BLL.AssemblyInfo
{
    public class BLLAssemblyInfo
    {
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <returns></returns>
        public static  Assembly GetAssembly()
        { 
           return Assembly.GetExecutingAssembly();
        }
    }
}
