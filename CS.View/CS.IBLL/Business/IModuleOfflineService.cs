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
    /// 模组下线
    /// </summary>
    public interface IModuleOfflineService
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<ModuleOfflineEntity>> GetModuleOfflinesAsync(int pageIndex, int pageSize, ModuleOfflineCondition condition);

        /// <summary>
        /// 添加模组下线记录
        /// </summary>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> AddModuleOfflineAsync(ModuleOfflineEntity entiry);
    }
}
