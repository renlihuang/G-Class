using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class DeviceNetworkInfoEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        ///  IP地址
        /// </summary>
        public string IpAddr { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
