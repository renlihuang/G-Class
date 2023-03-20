using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class PasteBoxShimEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  SFC模组号
        /// </summary>
        public string ModuleCode1 { get; set; }
        /// <summary>
        ///  模组条码2
        /// </summary>
        public string ModuleCode2 { get; set; }

        /// <summary>
        ///  垫片数量
        /// </summary>
        public string ShimNum { get; set; }

    }
}
