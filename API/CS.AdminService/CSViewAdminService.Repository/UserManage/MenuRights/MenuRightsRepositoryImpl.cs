

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/19 9:14:30
using System;
using System.Collections.Generic;
using NPoco;
using System.Data.SqlClient;
using System.Threading.Tasks;

using CSViewAdminService.Core;
using CSViewAdminService.UserManage.Abstractions;
using CSViewAdminService.Utility;


namespace CSViewAdminService.Repository
{
    internal class MenuRightsRepositoryImpl : AbstractRepository<MenuRightsEntity,BaseMenuRightsCondition,long>,IQueryMenuRightsRepository,ICommandMenuRightsRepository<long>, IAutoInject
    {

	     public MenuRightsRepositoryImpl (IScopeDBFactory scopeDBFactory) : base(scopeDBFactory)
        {

        }

         

        
            
        public override Sql TrunConditionToSql(Sql sql,BaseMenuRightsCondition condition)
        {
            if (condition != null)
            {
                        if(condition.RoleID.HasValue)
        {
             if (condition.RoleID > 0)
            {
                sql.Append(" And RoleID=@RoleID", new { RoleID=condition.RoleID.Value });
            }
        }

            }
            return sql;
        }
    }
}

