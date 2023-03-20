using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class AutoWeightEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCoreCode { get; set; }

        /// <summary>
        ///  SFC模组号
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  模组重量
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        ///  pack条码
        /// </summary>
        public string PackCode { get; set; }
    }
}
