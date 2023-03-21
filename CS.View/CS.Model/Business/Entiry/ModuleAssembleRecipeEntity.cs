using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    /// <summary>
    /// 模组组装
    /// </summary>
    public class ModuleAssembleRecipeEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  配方名称
        /// </summary>
        public string RecipeName { get; set; }

        /// <summary>
        ///  配方编号
        /// </summary>
        public string RecipeCode { get; set; }

        /// <summary>
        ///  配方列表
        /// </summary>
        public string RecipeList { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
