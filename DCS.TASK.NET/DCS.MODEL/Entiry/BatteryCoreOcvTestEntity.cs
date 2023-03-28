using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class BatteryCoreOcvTestEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCoreCode { get; set; }

        /// <summary>
        ///  探针次数
        /// </summary>
        public string needlenumber { get; set; }

        /// <summary>
        ///  OCV电芯电压
        /// </summary>
        public string OcvVoltage { get; set; }

        /// <summary>
        ///  极性位置
        /// </summary>
        public string BatteryState { get; set; }

        /// <summary>
        ///  电芯下线时间
        /// </summary>
        public DateTime? OcvOfflineTime { get; set; }

        /// <summary>
        ///  电芯上线测试时间
        /// </summary>
        public DateTime? OcvOnlineTime { get; set; }

        /// <summary>
        ///  结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        ///  OcvTEMP温度
        /// </summary>
        public string OcvTEMP { get; set; }

    }
}
