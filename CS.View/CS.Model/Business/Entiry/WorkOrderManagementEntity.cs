using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class WorkOrderManagementEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 工单说明
        /// </summary>
        public string WorkOrderDescription { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        public int ProductionQuantity { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public int WorkOrderStatus { get; set; }
        public string WorkOrderStatusName { get; set; }

        /// <summary>
        /// 模组编号
        /// </summary>
        public string ModuleNumber { get; set; }

        /// <summary>
        /// 模组名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>

        public long? CreaterId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>      
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long? ModifierId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        public string CreaterName { set; get; }

        public string ModifierName { set; get; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public int IsDeleted { get; set; }
    }
}
