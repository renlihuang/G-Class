using CS.Base.HttpHelper;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.IBLL.Business
{
    public interface IModuleGroupService
    {
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> InsertModuleGroupBatch(List<ModuleGroupEntity> entities);


        /// <summary>
        /// 查询模组进站记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userQueryCondition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<ModuleGroupEntity>> GetModuleGroupsAync(int pageIndex, int pageSize, ModuleGroupCondition condtion);
    }
}
