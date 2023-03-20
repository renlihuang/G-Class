using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class PoleTestEntity
    {
        public long Id { get; set; }
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

        /// <summary>
        ///  检验结果
        /// </summary>
        public string TestResult { get; set; }

    }
}
