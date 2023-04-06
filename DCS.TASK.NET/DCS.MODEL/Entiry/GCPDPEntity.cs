using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class GCPDPEntity
    {

        public long Id { get; set; }

        /// <summary>
        ///  模组条码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  等离子转速
        /// </summary>
        public string PDPrpm { get; set; }

        /// <summary>
        ///  等离子电压
        /// </summary>
        public string PDPvoltage { get; set; }

        /// <summary>
        ///  等离子电流
        /// </summary>
        public string PDPelectricity { get; set; }

        /// <summary>
        ///  等离子气压
        /// </summary>
        public string PDPkPa { get; set; }

        /// <summary>
        ///  等离子速度
        /// </summary>
        public string PDPspeed { get; set; }

        /// <summary>
        ///  等离子功率
        /// </summary>
        public string PDPpower { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
