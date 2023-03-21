using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.QueryCondition
{
    public class SCTrayCleanCondition
    {
        /// <summary>
        ///  托盘条码
        /// </summary>
        public string TrayCode { get; set; }

        /// <summary>
        ///  产品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        ///  查询开始时间
        /// </summary>
        public string CreateTimeStart { get; set; }

        /// <summary>
        ///  查询结束时间
        /// </summary>
        public string CreateTimeEnd { get; set; }
    }
}
