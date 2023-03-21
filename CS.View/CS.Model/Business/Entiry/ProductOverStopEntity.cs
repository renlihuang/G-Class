using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ProductOverStopEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  托盘码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        ///  产品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        ///  过站信息
        /// </summary>
        public string StandInfo { get; set; }

        /// <summary>
        ///  过站结果
        /// </summary>
        public string OverStop { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///  进站出站标志
        /// </summary>
        public string IEFlag { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Count { get; set; }
    }
}
