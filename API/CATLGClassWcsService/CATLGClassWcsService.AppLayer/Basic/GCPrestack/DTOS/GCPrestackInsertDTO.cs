

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:16:19
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CATLGClassWcsService.AppLayer.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class GCPrestackInsertDTO
    {
            /// <summary>
        ///  虚拟模组码
        /// </summary>
        [Description("虚拟模组码")]
                  [MinLength(0)]
                   [MaxLength(200)]
                  public string VirtualCode {get;set;}
        
          /// <summary>
        ///  模组类型
        /// </summary>
        [Description("模组类型")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string MoudleType {get;set;}
        
          /// <summary>
        ///  资源号
        /// </summary>
        [Description("资源号")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string Resource {get;set;}
        
          /// <summary>
        ///  工单
        /// </summary>
        [Description("工单")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string WorkOrder {get;set;}
        
     

    }
}
