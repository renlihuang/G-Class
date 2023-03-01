
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/19 9:14:30
using System;
using CSViewAdminService.Core;
namespace CSViewAdminService.AppLayer.UserManage.ViewObject
{
    /// <summary>
    /// 
    /// </summary>
	[MyTableName("MenuRights")]
    [MyPrimaryKey("ID",AutoIncrement =false)]
    public class MenuRightsView:ViewBaseField
    {
            /// <summary>
        ///  ID
        /// </summary>
        public long ID {get;set;}
        
          /// <summary>
        ///  角色ID
        /// </summary>
        public long? RoleID {get;set;}
        
          /// <summary>
        ///  菜单ID
        /// </summary>
        public long? MenuID {get;set;}
        
     

    }
}
