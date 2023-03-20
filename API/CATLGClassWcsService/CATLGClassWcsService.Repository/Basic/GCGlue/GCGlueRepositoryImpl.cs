

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:19:01
using System;
using System.Collections.Generic;
using NPoco;
using System.Data.SqlClient;
using System.Threading.Tasks;

using CATLGClassWcsService.Core;
using CATLGClassWcsService.Basic.Abstractions;
using CATLGClassWcsService.Utility;


namespace CATLGClassWcsService.Repository
{
    internal class GCGlueRepositoryImpl : AbstractRepository<GCGlueEntity,BaseGCGlueCondition,long>,IQueryGCGlueRepository,ICommandGCGlueRepository<long>, IAutoInject
    {

	     public GCGlueRepositoryImpl (IScopeDBFactory scopeDBFactory) : base(scopeDBFactory)
        {

        }

         

        
            
        public override Sql TrunConditionToSql(Sql sql,BaseGCGlueCondition condition)
        {
            if (condition != null)
            {
                        if (!string.IsNullOrEmpty(condition.ModuleCode))
        {
            sql.Append(" And ModuleCode=@ModuleCode", new { ModuleCode=condition.ModuleCode });
        }

            }
            return sql;
        }
    }
}

