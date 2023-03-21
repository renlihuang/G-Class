using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ModuleWeightEntity
    {
        /// <summary>
        ///  模组条码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  模组重量
        /// </summary>
        public float? Weight { get; set; }

        /// <summary>
        ///  称重判断结果
        /// </summary>
        public int? Result { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
