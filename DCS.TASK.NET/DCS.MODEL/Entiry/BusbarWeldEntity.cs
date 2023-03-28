using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.MODEL.Entiry
{
    public class BusbarWeldEntity
    {
        public long Id { get; set; }
        /// <summary>
        ///  托盘RFID
        /// </summary>
        public string RFID { get; set; }

        /// <summary>
        ///  输出功率
        /// </summary>
        public string OutputPower { get; set; }

        /// <summary>
        ///  内环功率
        /// </summary>
        public string InPower { get; set; }

        /// <summary>
        ///  外环功率
        /// </summary>
        public string OutPower { get; set; }

        /// <summary>
        ///  内外环功率比
        /// </summary>
        public string PowerRatio { get; set; }

        /// <summary>
        ///  离焦量
        /// </summary>
        public string FocusValue { get; set; }

        /// <summary>
        ///  振幅
        /// </summary>
        public string Swing { get; set; }

        /// <summary>
        ///  频率
        /// </summary>
        public string BusbarHz { get; set; }

        /// <summary>
        ///  线宽
        /// </summary>
        public string LineWidth { get; set; }

        /// <summary>
        ///  步长
        /// </summary>
        public string BusbarSize { get; set; }

        /// <summary>
        ///  螺旋半径
        /// </summary>
        public string BusbarScrew { get; set; }

        /// <summary>
        ///  螺距
        /// </summary>
        public string BusbarLength { get; set; }

        /// <summary>
        ///  焊接速度
        /// </summary>
        public string BusbarSpeed { get; set; }

        /// <summary>
        ///  保护气流量
        /// </summary>
        public string BusbarAir { get; set; }

        /// <summary>
        ///  负压值
        /// </summary>
        public string BusbarPa { get; set; }

        /// <summary>
        ///  测距值（补偿前）
        /// </summary>
        public string BeforeRange { get; set; }

        /// <summary>
        ///  测距值（补偿前）与基准
        /// </summary>
        public string BeforeRangeToNorm { get; set; }

        /// <summary>
        ///  测距值（补偿前）极值
        /// </summary>
        public string ExtremeValue { get; set; }

        /// <summary>
        ///  测距仪补偿值
        /// </summary>
        public string SetValue { get; set; }

        /// <summary>
        ///  机械手Z轴坐标（补偿前）
        /// </summary>
        public string RobotzValue { get; set; }

        /// <summary>
        ///  机械手Z轴坐标（补偿后）
        /// </summary>
        public string RobotSetzValue { get; set; }

        /// <summary>
        ///  补偿前后机械手Z轴坐标差值
        /// </summary>
        public string SetRobotDiffValue { get; set; }

        /// <summary>
        ///  测距值（补偿后）
        /// </summary>
        public string LaterRangeToNorm { get; set; }

        /// <summary>
        ///  机器人第一组补偿
        /// </summary>
        public string Setvalue_Group1 { get; set; }

        /// <summary>
        ///  setvalue_Group2
        /// </summary>
        public string Setvalue_Group2 { get; set; }

        /// <summary>
        ///  setvalue_Group3
        /// </summary>
        public string Setvalue_Group3 { get; set; }

        /// <summary>
        ///  setvalue_Group4
        /// </summary>
        public string Setvalue_Group4 { get; set; }

        /// <summary>
        ///  setvalue_Group5
        /// </summary>
        public string Setvalue_Group5 { get; set; }

        /// <summary>
        ///  A补偿值
        /// </summary>
        public string Phto_A_offset { get; set; }

        /// <summary>
        ///  B补偿值
        /// </summary>
        public string Phto_B_offset { get; set; }

        /// <summary>
        ///  模组A条码
        /// </summary>
        public string Module_acode { get; set; }

        /// <summary>
        ///  模组B条码
        /// </summary>
        public string Module_bcode { get; set; }

        /// <summary>
        ///  A组补偿值
        /// </summary>
        public string Module_A { get; set; }

        /// <summary>
        ///  B组补偿值
        /// </summary>
        public string Module_B { get; set; }

        /// <summary>
        ///  员工号
        /// </summary>
        public string CurrentUserName { get; set; }

        /// <summary>
        ///  最大输出功率1
        /// </summary>
        public string RingPower_MAX1 { get; set; }

        /// <summary>
        ///  OutputPower_MAX2
        /// </summary>
        public string RingPower_MAX2 { get; set; }

        /// <summary>
        ///  OutputPower_MAX3
        /// </summary>
        public string RingPower_MAX3 { get; set; }

        /// <summary>
        ///  OutputPower_MAX4
        /// </summary>
        public string RingPower_MAX4 { get; set; }

        /// <summary>
        ///  OutputPower_MAX5
        /// </summary>
        public string RingPower_MAX5 { get; set; }

        /// <summary>
        ///  最小输出功率1
        /// </summary>
        public string RingPower_MIN1 { get; set; }

        /// <summary>
        ///  OutputPower_MIN2
        /// </summary>
        public string RingPower_MIN2 { get; set; }

        /// <summary>
        ///  OutputPower_MIN3
        /// </summary>
        public string RingPower_MIN3 { get; set; }

        /// <summary>
        ///  OutputPower_MIN4
        /// </summary>
        public string RingPower_MIN4 { get; set; }

        /// <summary>
        ///  OutputPower_MIN5
        /// </summary>
        public string RingPower_MIN5 { get; set; }

        /// <summary>
        ///  输出平均功率1
        /// </summary>
        public string RingPower_group1 { get; set; }

        /// <summary>
        ///  OutputPower_group2
        /// </summary>
        public string RingPower_group2 { get; set; }

        /// <summary>
        ///  OutputPower_group3
        /// </summary>
        public string RingPower_group3 { get; set; }

        /// <summary>
        ///  OutputPower_group4
        /// </summary>
        public string RingPower_group4 { get; set; }

        /// <summary>
        ///  OutputPower_group5
        /// </summary>
        public string RingPower_group5 { get; set; }

        /// <summary>
        ///  最大输出功率1
        /// </summary>
        public string CenterPower_MAX1 { get; set; }

        /// <summary>
        ///  OutputPower_MAX2
        /// </summary>
        public string CenterPower_MAX2 { get; set; }

        /// <summary>
        ///  OutputPower_MAX3
        /// </summary>
        public string CenterPower_MAX3 { get; set; }

        /// <summary>
        ///  OutputPower_MAX4
        /// </summary>
        public string CenterPower_MAX4 { get; set; }

        /// <summary>
        ///  OutputPower_MAX5
        /// </summary>
        public string CenterPower_MAX5 { get; set; }

        /// <summary>
        ///  最小输出功率1
        /// </summary>
        public string CenterPower_MIN1 { get; set; }

        /// <summary>
        ///  OutputPower_MIN2
        /// </summary>
        public string CenterPower_MIN2 { get; set; }

        /// <summary>
        ///  OutputPower_MIN3
        /// </summary>
        public string CenterPower_MIN3 { get; set; }

        /// <summary>
        ///  OutputPower_MIN4
        /// </summary>
        public string CenterPower_MIN4 { get; set; }

        /// <summary>
        ///  OutputPower_MIN5
        /// </summary>
        public string CenterPower_MIN5 { get; set; }

        /// <summary>
        ///  输出平均功率1
        /// </summary>
        public string CenterPower_group1 { get; set; }

        /// <summary>
        ///  OutputPower_group2
        /// </summary>
        public string CenterPower_group2 { get; set; }

        /// <summary>
        ///  OutputPower_group3
        /// </summary>
        public string CenterPower_group3 { get; set; }

        /// <summary>
        ///  OutputPower_group4
        /// </summary>
        public string CenterPower_group4 { get; set; }

        /// <summary>
        ///  OutputPower_group5
        /// </summary>
        public string CenterPower_group5 { get; set; }

        /// <summary>
        ///  Photo1
        /// </summary>
        public string Photo1 { get; set; }

        /// <summary>
        ///  Photo2
        /// </summary>
        public string Photo2 { get; set; }

        /// <summary>
        ///  Photo3
        /// </summary>
        public string Photo3 { get; set; }

        /// <summary>
        ///  Photo4
        /// </summary>
        public string Photo4 { get; set; }

        /// <summary>
        ///  Mark
        /// </summary>
        public string PackCode { get; set; }

        /// <summary>
        ///  Mark
        /// </summary>
        public string Mark { get; set; }

    }
}
