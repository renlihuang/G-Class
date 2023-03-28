using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class PasteModuleEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCoreCode { get; set; }

        /// <summary>
        ///  SFC模组号
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  托盘RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        ///  SFC2
        /// </summary>
        public string ModuleCode2 { get; set; }


    }
}
