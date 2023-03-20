using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class PackStandingEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCoreCode { get; set; }

        /// <summary>
        ///  虚拟模组条码
        /// </summary>
        public string VirtualCode { get; set; }

        /// <summary>
        ///  静置工位位置
        /// </summary>
        public string PackSite { get; set; }

        /// <summary>
        ///  静置时间
        /// </summary>
        public DateTime? PackTime { get; set; }

        /// <summary>
        ///  加压压力F
        /// </summary>
        public string PackMPAF { get; set; }

        /// <summary>
        ///  保压时间T
        /// </summary>
        public DateTime? PackTimeT { get; set; }

        /// <summary>
        ///  模组长度L
        /// </summary>
        public string PackLength { get; set; }

        /// <summary>
        ///  模组条码1
        /// </summary>
        public string ModuleCode1 { get; set; }

        /// <summary>
        ///  模组条码2
        /// </summary>
        public string ModuleCode2 { get; set; }
    }
}
