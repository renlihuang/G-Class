using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class BlockVirtualCodeEntity
    {
        /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCoreCode { get; set; }

        /// <summary>
        ///  模组条码1
        /// </summary>
        public string VirtualCode { get; set; }
    }
}
