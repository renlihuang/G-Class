using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ErrorLogEntity
    {
        public long? Id { get; set; }
        /// <summary>
        ///  报错代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        ///  报错信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        ///  报错站点
        /// </summary>
        public string ErrorSite { get; set; }

        /// <summary>
        ///  报错类型
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///  是否是管理员
        /// </summary>
        public int IsManage { get; set; }
    }
}
