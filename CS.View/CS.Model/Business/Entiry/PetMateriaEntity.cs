using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class PetMateriaEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        ///  物料类型用途
        /// </summary>
        public string MaterialType { get; set; }

        /// <summary>
        ///  物料验证结果
        /// </summary>
        public string MateriaResult { get; set; }

        /// <summary>
        ///  物料数量
        /// </summary>
        public string MateriaCount { get; set; }

        /// <summary>
        ///  工站编号
        /// </summary>
        public string WorkStation { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
