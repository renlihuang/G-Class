using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class RepairOrderEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  工单编号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        ///  工艺类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        ///  工单数量
        /// </summary>
        public decimal? WorkOrderNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
