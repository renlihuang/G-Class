
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/19 9:09:55
using System;
using CSViewAdminService.Core;
namespace CSViewAdminService.AppLayer.UserManage.ViewObject
{
    /// <summary>
    /// 
    /// </summary>
	[MyTableName("Roles")]
    [MyPrimaryKey("ID",AutoIncrement =false)]
    public class RolesView:ViewBaseField
    {
            /// <summary>
        ///  ID
        /// </summary>
        public long ID {get;set;}
        
          /// <summary>
        ///  角色名称
        /// </summary>
        public string RoleName {get;set;}
        
          /// <summary>
        ///  是否是管理员
        /// </summary>
        public int? IsManage {get;set;}
        
     

    }
}
