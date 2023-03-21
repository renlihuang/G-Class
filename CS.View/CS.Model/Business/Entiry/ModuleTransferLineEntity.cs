using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ModuleTransferLineEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// 组号
        /// </summary>
        public string ModuleGroupNumber { get; set; }

        /// <summary>
        /// 模组条码
        /// </summary>
        public string ModuleBarCode { get; set; }
        /// <summary>
        /// 顺序号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 托盘号
        /// </summary>
        public string TrayNumber { get; set; }
        /// <summary>
        /// 模组类型
        /// </summary>
        public string ModuleType { get; set; }
        /// <summary>
        /// 模组类型编码
        /// </summary>
        public string ModuleCode { get; set; }
        /// <summary>
        /// 配方编号
        /// </summary>
        public string ModuleAssembleRecipeCode { get; set; }
        /// <summary>
        /// 模组状态
        /// </summary>
        public string ModuleStatus { get; set; }

        /// <summary>
        /// NG说明
        /// </summary>
        public string NGInstructions { get; set; }
        /// <summary>
        /// 转移目标
        /// </summary>
        public string TransferTarget { get; set; }
        /// <summary>
        /// 转移时间
        /// </summary>
        public string TransferTime { get; set; }
    }
}
