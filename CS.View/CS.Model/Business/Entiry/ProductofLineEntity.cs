using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ProductofLineEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  产品类型
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        ///  产品识别码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        ///  产品路线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        ///  产品路线说明
        /// </summary>
        public string ProductLineD { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 电气识别码
        /// </summary>
        public string  PLCCode { get; set; }
    }
}
