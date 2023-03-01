

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/19 9:14:30
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CSViewAdminService.AppLayer.UserManage.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuRightsInsertDTO
    {
            /// <summary>
        ///  角色ID
        /// </summary>
        [Description("角色ID")]
                  [Range(0, long.MaxValue)]
                  public long? RoleID {get;set;}
        
          /// <summary>
        ///  菜单ID
        /// </summary>
        [Description("菜单ID")]
                  [Range(0, long.MaxValue)]
                  public long? MenuID {get;set;}
        
     

    }
}
