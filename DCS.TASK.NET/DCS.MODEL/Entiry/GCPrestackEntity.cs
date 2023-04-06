namespace DCS.MODEL.Entiry
{
    public class GCPrestackEntity
    {
        public long Id { get; set; }

        /// <summary>
        ///  虚拟模组码
        /// </summary>
        public string VirtualCode { get; set; }

        /// <summary>
        ///  模组类型
        /// </summary>
        public string MoudleType { get; set; }

        /// <summary>
        ///  资源号
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        ///  工单
        /// </summary>
        public string WorkOrder { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}