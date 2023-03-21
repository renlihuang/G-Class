using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ModuleInStationEntity
    {
        /// <summary>
        ///  模组编号
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  客户条码
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        ///  调用接口返回客户条码
        /// </summary>
        public string ResponsClientCode { get; set; }

        /// <summary>
        ///  客户条码判断接口
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
