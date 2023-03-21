using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ProductEntity
    {
        public long? Id { get; set; }
        /// <summary>
        ///  模组名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///  模组编码规则
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///  是否是管理员
        /// </summary>
        public int IsManage { get; set; }
    }
}
