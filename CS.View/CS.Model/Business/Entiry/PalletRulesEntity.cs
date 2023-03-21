using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class PalletRulesEntity
    {
        public long? Id { get; set; }
        /// <summary>
        ///  产品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        ///  长度
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        ///  开始节点
        /// </summary>
        public string StartPosition { get; set; }

        /// <summary>
        ///  结束节点
        /// </summary>
        public string EndPosition { get; set; }

        /// <summary>
        ///  校验字符
        /// </summary>
        public string CheckField { get; set; }

        /// <summary>
        ///  是否启用
        /// </summary>
        public string UseFlag { get; set; }

        /// <summary>
        ///  校验类型
        /// </summary>
        public string CheckType { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public string Field_1 { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public string Field_2 { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public string Field_3 { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public string Field_4 { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public string Field_5 { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public decimal? Field_6 { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public decimal? Field_7 { get; set; }

        /// <summary>
        ///  Field_1
        /// </summary>
        public decimal? Field_8 { get; set; }

    }
}
