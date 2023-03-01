//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/9 9:23:36
using CSViewAdminService.Core;
using CSViewAdminService.UserManage.Abstractions;
using NPoco;

namespace CSViewAdminService.Repository
{
    internal class MenusRepositoryImpl : AbstractRepository<MenusEntity, BaseMenusCondition, long>, IQueryMenusRepository, ICommandMenusRepository<long>, IAutoInject
    {
        public MenusRepositoryImpl(IScopeDBFactory scopeDBFactory) : base(scopeDBFactory)
        {
        }

        public override Sql TrunConditionToSql(Sql sql, BaseMenusCondition condition)
        {
            if (condition != null)
            {
                if (condition.ParentID.HasValue)
                {
                    if (condition.ParentID >= 0)
                    {
                        sql.Append(" And ParentID=@ParentID", new { ParentID = condition.ParentID.Value });
                    }
                }
            }
            return sql;
        }
    }
}