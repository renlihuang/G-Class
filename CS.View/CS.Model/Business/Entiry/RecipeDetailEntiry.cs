using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    /// <summary>
    /// 配方明细
    /// </summary>
    public class RecipeDetailEntiry
    {
        /// <summary>
        /// ID
        /// </summary>
        public long id { set; get; }
        /// <summary>
        ///  模组名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        ///  模组编码规则
        /// </summary>
        public string ModuleCode { get; set; }
    }
}
