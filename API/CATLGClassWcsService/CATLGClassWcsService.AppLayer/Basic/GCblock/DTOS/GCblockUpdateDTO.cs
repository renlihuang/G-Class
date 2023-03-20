

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:17:52
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CATLGClassWcsService.AppLayer.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class GCblockUpdateDTO
    {
            /// <summary>
        ///  id
        /// </summary>
        [Description("id")]
                  [Required]
                   [Range(0, long.MaxValue)]
                  public long ID {get;set;}
        
          /// <summary>
        ///  模组码
        /// </summary>
        [Description("模组码")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string VirtualCode {get;set;}
        
          /// <summary>
        ///  电芯条码json
        /// </summary>
        [Description("电芯条码json")]
                  [MinLength(0)]
                   [MaxLength(-1)]
                  public string BatteryCoreCode {get;set;}
        
     

    }
}
