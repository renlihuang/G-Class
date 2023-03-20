
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:15:43
using System;
using CATLGClassWcsService.Core;
namespace CATLGClassWcsService.AppLayer.ViewObject
{
    /// <summary>
    /// 
    /// </summary>
	[MyTableName("OCVTEST")]
    [MyPrimaryKey("ID",AutoIncrement =false)]
    public class OCVTESTView:ViewBaseField
    {
            /// <summary>
        ///  id
        /// </summary>
        public long ID {get;set;}
        
          /// <summary>
        ///  电芯条码
        /// </summary>
        public string BatteryCode {get;set;}
        
          /// <summary>
        ///  ocv电压
        /// </summary>
        public float? OcvVoltage {get;set;}
        
          /// <summary>
        ///  温度
        /// </summary>
        public float? Temperature {get;set;}
        
          /// <summary>
        ///  测试结果
        /// </summary>
        public string OcvResult {get;set;}
        
     

    }
}
