using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class BlockPressInBoxEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCoreCode { get; set; }

        /// <summary>
        ///  模组条码1
        /// </summary>
        public string ModuleCode1 { get; set; }
        /// <summary>
        ///  模组条码2
        /// </summary>
        public string ModuleCode2 { get; set; }


        /// <summary>
        ///  加压压力F1
        /// </summary>
        public string BlockF1 { get; set; }

        /// <summary>
        ///  保压时间T1
        /// </summary>
        public DateTime? BlockT1 { get; set; }

        /// <summary>
        ///  模组长度L1
        /// </summary>
        public string BlockL1 { get; set; }

        /// <summary>
        ///  加压压力F2
        /// </summary>
        public string BlockF2 { get; set; }

        /// <summary>
        ///  保压时间T2
        /// </summary>
        public DateTime? BlockT2 { get; set; }

        /// <summary>
        ///  模组长度L2
        /// </summary>
        public string BlockL2 { get; set; }

        /// <summary>
        ///  入箱加压压力
        /// </summary>
        public string BlockInMPA { get; set; }

        /// <summary>
        ///  实时压力
        /// </summary>
        public string BlockNowMPA { get; set; }

        /// <summary>
        ///  加压时间
        /// </summary>
        public DateTime? BlockTime { get; set; }

        /// <summary>
        ///  加压台编号
        /// </summary>
        public string BlockCode { get; set; }
    }
}
