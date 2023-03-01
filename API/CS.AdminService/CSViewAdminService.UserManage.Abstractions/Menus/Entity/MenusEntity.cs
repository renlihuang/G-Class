
//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/9 9:23:36
using System;
using CSViewAdminService.Core;
namespace CSViewAdminService.UserManage.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    [MyTableName("Menus")]
    [MyPrimaryKey("ID",AutoIncrement =false)]
    public class MenusEntity: BaseField,IEntity<long>
    {

       public  MenusEntity()
       {
                    Id = GeneratePrimaryKeyIdHelper.GetPrimaryKeyId();
                         }
       public long Id{get;set;}
            /// <summary>
        ///  菜单名称
        /// </summary>
                  public string MenuName {get;set;}
        
          /// <summary>
        ///  菜单图标
        /// </summary>
                  public string MenuIcon {get;set;}
        
          /// <summary>
        ///  菜单实例
        /// </summary>
                  public string MenuInstance {get;set;}
        
          /// <summary>
        ///  父菜单ID
        /// </summary>
                  public long? ParentID {get;set;}
        
     

    }

   
}
