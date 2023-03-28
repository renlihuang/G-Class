using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class PasteBoxEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  模组条码1
        /// </summary>
        public string ModuleCode1 { get; set; }
        /// <summary>
        ///  模组条码2
        /// </summary>
        public string ModuleCode2 { get; set; }

        /// <summary>
        ///  PACK码
        /// </summary>
        public string PackCode { get; set; }

        /// <summary>
        ///  托盘RFid
        /// </summary>
        public string RFID { get; set; }

    }
}
