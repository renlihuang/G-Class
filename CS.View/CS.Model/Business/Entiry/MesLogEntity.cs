using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class MesLogEntity
    {
        /// <summary>
        ///  别名
        /// </summary>
        public string Aliases { get; set; }

        /// <summary>
        ///  接口名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        ///  模组条码
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  返回状态码
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        ///  返回消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}
