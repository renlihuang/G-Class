using System;
using CSViewAdminService.Core;
namespace CSViewAdminService.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersEntity: BaseField
    {
            /// <summary>
        ///  用户名
        /// </summary>
        public int? UserName {get;set;}
        
          /// <summary>
        ///  密码
        /// </summary>
        public string UserPassword {get;set;}
        
          /// <summary>
        ///  用户角设
        /// </summary>
        public string UserRole {get;set;}
        
          /// <summary>
        ///  角色ID
        /// </summary>
        public int? UserRoleID {get;set;}
        
          /// <summary>
        ///  最后登录时间
        /// </summary>
        public DateTime? LastLoginTime {get;set;}
        
     

    }
}
