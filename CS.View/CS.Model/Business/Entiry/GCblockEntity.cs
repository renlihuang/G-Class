using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class GCblockEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  模组码
        /// </summary>
        public string VirtualCode { get; set; }

        /// <summary>
        ///  电芯条码json
        /// </summary>
        public string BatteryCoreCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

    }
}
