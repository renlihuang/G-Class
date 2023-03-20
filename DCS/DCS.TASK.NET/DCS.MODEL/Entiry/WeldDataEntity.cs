using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class WeldDataEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  模组条码
        /// </summary>
        public string MoudleCode { get; set; }

        /// <summary>
        ///  x坐标
        /// </summary>
        public float? X { get; set; }

        /// <summary>
        ///  y坐标
        /// </summary>
        public float? Y { get; set; }

        /// <summary>
        ///  z坐标
        /// </summary>
        public float? Z { get; set; }

        /// <summary>
        ///  坐标值集
        /// </summary>
        public string XYZvalue { get; set; }
    }
}
