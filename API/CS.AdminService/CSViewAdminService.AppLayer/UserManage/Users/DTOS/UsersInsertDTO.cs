

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/9/7 15:56:49
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CSViewAdminService.AppLayer.UserManage.DTOS
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersInsertDTO
    {
            /// <summary>
        ///  用户名
        /// </summary>
        [Description("用户名")]
            [MinLength(0)]
            [MaxLength(50)]
        public string? UserName {get;set;}
        
          /// <summary>
        ///  密码
        /// </summary>
        [Description("密码")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string UserPassword {get;set;}
        
          /// <summary>
        ///  用户角设
        /// </summary>
        [Description("用户角设")]
                  [MinLength(0)]
                   [MaxLength(50)]
                  public string UserRole {get;set;}
        
          /// <summary>
        ///  角色ID
        /// </summary>
        [Description("角色ID")]
                  [Range(0, long.MaxValue)]
                  public long? UserRoleID {get;set;}
        
          /// <summary>
        ///  最后登录时间
        /// </summary>
        [Description("最后登录时间")]
                 public DateTime? LastLoginTime {get;set;}
        
     

    }
}
