

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:19:11
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CATLGClassWcsService.AppLayer.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class GCPDPUpdateDTO
    {
            /// <summary>
        ///  id
        /// </summary>
        [Description("id")]
                  [Required]
                   [Range(0, long.MaxValue)]
                  public long ID {get;set;}
        
          /// <summary>
        ///  模组条码
        /// </summary>
        [Description("模组条码")]
                  [MinLength(0)]
                   [MaxLength(200)]
                  public string ModuleCode {get;set;}
        
          /// <summary>
        ///  等离子转速
        /// </summary>
        [Description("等离子转速")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string PDPrpm {get;set;}
        
          /// <summary>
        ///  等离子电压
        /// </summary>
        [Description("等离子电压")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string PDPvoltage {get;set;}
        
          /// <summary>
        ///  等离子电流
        /// </summary>
        [Description("等离子电流")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string PDPelectricity {get;set;}
        
          /// <summary>
        ///  等离子气压
        /// </summary>
        [Description("等离子气压")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string PDPkPa {get;set;}
        
          /// <summary>
        ///  等离子速度
        /// </summary>
        [Description("等离子速度")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string PDPspeed {get;set;}
        
          /// <summary>
        ///  等离子功率
        /// </summary>
        [Description("等离子功率")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string PDPpower {get;set;}
        
     

    }
}
