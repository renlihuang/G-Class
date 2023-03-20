

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:19:01
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CATLGClassWcsService.AppLayer.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class GCGlueUpdateDTO
    {
            /// <summary>
        ///  ID
        /// </summary>
        [Description("ID")]
                  [Required]
                   [Range(0, long.MaxValue)]
                  public long ID {get;set;}
        
          /// <summary>
        ///  模组码
        /// </summary>
        [Description("模组码")]
                  [MinLength(0)]
                   [MaxLength(200)]
                  public string ModuleCode {get;set;}
        
          /// <summary>
        ///  涂胶开始时间
        /// </summary>
        [Description("涂胶开始时间")]
                 public DateTime? GlueStartTime {get;set;}
        
          /// <summary>
        ///  涂胶结束时间
        /// </summary>
        [Description("涂胶结束时间")]
                 public DateTime? GlueEndTime {get;set;}
        
          /// <summary>
        ///  涂胶重量
        /// </summary>
        [Description("涂胶重量")]
                  [Range(0, float.MaxValue)]
                  public float? GlueWeight {get;set;}
        
     

    }
}
