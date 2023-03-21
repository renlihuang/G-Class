using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ModuleOfflineEntity
    {
        /// <summary>
        ///  模组条码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  总成条码
        /// </summary>
        public string AssemblyCode { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
