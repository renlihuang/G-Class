using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class PetGummingEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  托盘条码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        ///  产品类型
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        ///  工作压力
        /// </summary>
        public string WorkPressure { get; set; }

        /// <summary>
        ///  温度
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        ///  涂胶量
        /// </summary>
        public string GlueAmount { get; set; }

        /// <summary>
        ///  流量
        /// </summary>
        public string Traffic { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
