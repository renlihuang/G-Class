using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.QueryCondition
{
    public class ProductOverStopCondition
    {
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
        ///  创建时间起始时间
        /// </summary>
        public string CreateTimeStart { get; set; }

        /// <summary>
        ///  创建时间结束时间
        /// </summary>
        public string CreateTimeEnd { get; set; }
    }
}
