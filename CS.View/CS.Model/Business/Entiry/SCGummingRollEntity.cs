using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class SCGummingRollEntity
    {
        public long ID { get; set; }
        /// <summary>
        ///  托盘条码
        /// </summary>
        public string TrayCode { get; set; }

        /// <summary>
        ///  产品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        ///  滚压左边压力
        /// </summary>
        public string LeftRollPressure { get; set; }

        /// <summary>
        ///  滚压右边压力
        /// </summary>
        public string RightRollPressure { get; set; }

        /// <summary>
        ///  工序站点
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
