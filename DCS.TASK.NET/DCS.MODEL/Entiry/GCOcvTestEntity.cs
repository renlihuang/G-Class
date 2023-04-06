namespace DCS.MODEL.Entiry
{
    public class GCOcvTestEntity
    {

        public long Id { get; set; }
        /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCode { get; set; }

        public string LocationCode { get; set; }

        /// <summary>
        ///  ocv电压
        /// </summary>
        public float? OcvVoltage { get; set; }

        /// <summary>
        ///  温度
        /// </summary>
        public float? Temperature { get; set; }

        /// <summary>
        ///  测试结果
        /// </summary>
        public string OcvResult { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
