using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class GCGlueEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  模组码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  涂胶开始时间
        /// </summary>
        public DateTime? GlueStartTime { get; set; }

        /// <summary>
        ///  涂胶结束时间
        /// </summary>
        public DateTime? GlueEndTime { get; set; }

        /// <summary>
        ///  涂胶重量
        /// </summary>
        public float? GlueWeight { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

    }
}
