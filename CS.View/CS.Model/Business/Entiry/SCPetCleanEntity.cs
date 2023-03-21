using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class SCPetCleanEntity
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
        ///  等离子功率
        /// </summary>
        public string Power { get; set; }

        /// <summary>
        ///  伺服轴速度
        /// </summary>
        public string Speed { get; set; }

        /// <summary>
        ///  清洗高度
        /// </summary>
        public string Height { get; set; }

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
