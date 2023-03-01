

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/9/7 15:56:49
using NPoco;

using CSViewAdminService.Core;
using CSViewAdminService.UserManage.Abstractions;

namespace CSViewAdminService.Repository
{
    internal class UsersRepositoryImpl : AbstractRepository<UsersEntity,BaseUsersCondition,long>,IQueryUsersRepository,ICommandUsersRepository<long>, IAutoInject
    {

	     public UsersRepositoryImpl (IScopeDBFactory scopeDBFactory) : base(scopeDBFactory)
        {

        }

         
        public override Sql TrunConditionToSql(Sql sql,BaseUsersCondition condition)
        {
            if (condition != null)
            {
                if (!string.IsNullOrEmpty(condition.UserName))
                {
                    sql.Append(" AND UserName=@UserName", new { UserName = condition.UserName });
                }
            }
            return sql;
        }
    }
}

