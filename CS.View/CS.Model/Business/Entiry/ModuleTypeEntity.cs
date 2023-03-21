using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ModuleTypeEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        ///  模组名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        ///  模组编码规则
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  模组描述
        /// </summary>
        public string ModuleDescibe { get; set; }

        /// <summary>
        ///  模组图片
        /// </summary>
        public string ModuleImage { get; set; }

        /// <summary>
        /// 电芯数量
        /// </summary>
        public int BatteryCount { set; get; }

        /// <summary>
        /// 图片字节流
        /// </summary>
        public byte[] ImageBytes { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
