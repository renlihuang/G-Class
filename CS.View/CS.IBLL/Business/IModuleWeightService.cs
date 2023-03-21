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
    /// <summary>
    /// 模组称重
    /// </summary>
    public interface IModuleWeightService
    {
        /// <summary>
        /// 查询模组进站记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userQueryCondition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<ModuleWeightEntity>> GetModuleWeightsAync(int pageIndex, int pageSize, ModuleWeightQueryCondtion condtion);
    }
}
