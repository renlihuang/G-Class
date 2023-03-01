

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/9 9:23:36
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CSViewAdminService.AppLayer.UserManage.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class MenusInsertDTO
    {
            /// <summary>
        ///  菜单名称
        /// </summary>
        [Description("菜单名称")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string MenuName {get;set;}
        
          /// <summary>
        ///  菜单图标
        /// </summary>
        [Description("菜单图标")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string MenuIcon {get;set;}
        
          /// <summary>
        ///  菜单实例
        /// </summary>
        [Description("菜单实例")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string MenuInstance {get;set;}
        
          /// <summary>
        ///  父菜单ID
        /// </summary>
        [Description("父菜单ID")]
                  [Range(0, long.MaxValue)]
                  public long? ParentID {get;set;}
        
     

    }
}
