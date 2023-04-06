

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/4/6 10:57:28
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CATLGClassWcsService.AppLayer.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class OCVTESTInsertDTO
    {
            /// <summary>
        ///  电芯条码
        /// </summary>
        [Description("电芯条码")]
                  [MinLength(0)]
                   [MaxLength(200)]
                  public string BatteryCode {get;set;}
        
          /// <summary>
        ///  ocv电压
        /// </summary>
        [Description("ocv电压")]
                  [Range(0, float.MaxValue)]
                  public float? OcvVoltage {get;set;}
        
          /// <summary>
        ///  位置编号
        /// </summary>
        [Description("位置编号")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string LocationCode {get;set;}
        
          /// <summary>
        ///  温度
        /// </summary>
        [Description("温度")]
                  [Range(0, float.MaxValue)]
                  public float? Temperature {get;set;}
        
          /// <summary>
        ///  测试结果
        /// </summary>
        [Description("测试结果")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string OcvResult {get;set;}
        
     

    }
}
