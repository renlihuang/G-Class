using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.QueryCondition
{
    public class GCblockCondition
    {
        /// <summary>
        ///  模组码
        /// </summary>
        public string VirtualCode { get; set; }

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
