using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ModuleCellBindingEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 模组编码
        /// </summary>
        public string ModuleBarcode { get; set; }

        /// <summary>
        ///  电芯编码
        /// </summary>       
        public string CellBarcode { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>      
        public int LocationNumber { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>       
        public DateTime BindingTime { get; set; }
    }
}
