using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class PreWeldAddrEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  SFC1模组号1
        /// </summary>
        public string ModuleCode1 { get; set; }

        /// <summary>
        ///  SFC2模组号2
        /// </summary>
        public string ModuleCode2 { get; set; }

        /// <summary>
        ///  虚拟模组条码
        /// </summary>
        public string VirtualCode { get; set; }

        /// <summary>
        ///  托盘RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        ///  位置数据
        /// </summary>
        public string SateData { get; set; }

        /// <summary>
        ///  测距值
        /// </summary>
        public string RangeValue { get; set; }

        /// <summary>
        ///  测距值与基准值差值
        /// </summary>
        public string RangeDiff { get; set; }

        /// <summary>
        ///  极差值
        /// </summary>
        public string PoorValue { get; set; }


    }
}
