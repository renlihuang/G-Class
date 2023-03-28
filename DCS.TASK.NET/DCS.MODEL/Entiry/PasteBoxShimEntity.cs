using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class PasteBoxShimEntity
    {
        /// <summary>
        ///  SFC模组号
        /// </summary>
        public string ModuleCode1 { get; set; }

        /// <summary>
        ///  垫片数量
        /// </summary>
        public decimal? ShimNum { get; set; }

        /// <summary>
        ///  BlockMPA_4
        /// </summary>
        public decimal? BlockMPA_4 { get; set; }

        /// <summary>
        ///  BlockTime_4
        /// </summary>
        public decimal? BlockTime_4 { get; set; }

        /// <summary>
        ///  BlockMPA_10
        /// </summary>
        public decimal? BlockMPA_10 { get; set; }

        /// <summary>
        ///  BlockTime_10
        /// </summary>
        public decimal? BlockTime_10 { get; set; }

        /// <summary>
        ///  模组长度1
        /// </summary>
        public decimal? BlockL1 { get; set; }

        /// <summary>
        ///  模组长度2
        /// </summary>
        public decimal? BlockL2 { get; set; }

        /// <summary>
        ///  静置压力
        /// </summary>
        public decimal? StandPressure { get; set; }

        /// <summary>
        ///  BlockLDbCz
        /// </summary>
        public decimal? BlockLDbCz { get; set; }

    }
}
