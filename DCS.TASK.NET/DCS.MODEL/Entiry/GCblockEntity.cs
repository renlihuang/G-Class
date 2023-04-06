using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class GCblockEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  模组码
        /// </summary>
        public string VirtualCode { get; set; }

        /// <summary>
        ///  电芯条码json
        /// </summary>
        public string BatteryCoreCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        private string _batteryCodeDetil = "请查看详细";

        public string BatteryCodeDetil
        {
            get { return _batteryCodeDetil; }
            set { _batteryCodeDetil = value; }
        }


    }
}
