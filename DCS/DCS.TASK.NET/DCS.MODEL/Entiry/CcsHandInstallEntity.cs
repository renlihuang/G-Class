using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class CcsHandInstallEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  SFC模组号
        /// </summary>
        public string ModuleCode { get; set; }

        /// <summary>
        ///  虚拟模组条码
        /// </summary>
        public string VirtualCode { get; set; }

        /// <summary>
        ///  托盘RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        ///  CCS物料编码
        /// </summary>
        public string CcsCode { get; set; }

    }
}
