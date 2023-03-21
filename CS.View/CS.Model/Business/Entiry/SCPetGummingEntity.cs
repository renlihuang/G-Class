using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class SCPetGummingEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  托盘条码
        /// </summary>
        public string TrayCode { get; set; }

        /// <summary>
        ///  产品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        ///  反馈理论胶量A(g)
        /// </summary>
        public string TheoryWeightA { get; set; }

        /// <summary>
        ///  反馈理论胶量B(g)
        /// </summary>
        public string TheoryWeightB { get; set; }

        /// <summary>
        ///  反馈监控胶量A(g)
        /// </summary>
        public string MonitorWeightA { get; set; }

        /// <summary>
        ///  反馈监控胶量B(g)
        /// </summary>
        public string MonitorWeightB { get; set; }

        /// <summary>
        ///  反馈理论总胶量(g)
        /// </summary>
        public string TheoryWeightAB { get; set; }

        /// <summary>
        ///  反馈监控总胶量(g)
        /// </summary>
        public string MonitorWeightAB { get; set; }

        /// <summary>
        ///  A胶压力(bar)
        /// </summary>
        public string GluePressureA { get; set; }

        /// <summary>
        ///  B胶压力(bar)
        /// </summary>
        public string GluePressureB { get; set; }

        /// <summary>
        ///  速度
        /// </summary>
        public string Speed { get; set; }

        /// <summary>
        ///  胶水总胶量
        /// </summary>
        public string GlueMetering_V { get; set; }

        /// <summary>
        ///  工序站点
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
