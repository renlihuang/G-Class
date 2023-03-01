using CATLGClassWcsService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CATLGClassWcsService.Core
{
    public class BaseField
    {
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        /// 
        [MyComputedColumn(ComputedColumnType = MyComputedColumnTypeEnum.ComputedOnUpdate)]
        public long? CreaterId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        /// 
        [MyComputedColumn(ComputedColumnType = MyComputedColumnTypeEnum.ComputedOnUpdate)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long? ModifierId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        [MyComputedColumn(ComputedColumnType = MyComputedColumnTypeEnum.ComputedOnUpdate)]
        public string CreaterName { set; get; }

        public string ModifierName { set; get; }

    }
}
