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
    public interface IModuleTransferLineService
    {

        Task<QueryPagedResponseModel<ModuleTransferLineEntity>> GetModuleTransferLineListAsync(int pageIndex, int pageSize, ModuleTransferLineCondition condition);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> AddModuleTransferLineAsync(ModuleTransferLineEntity moduleTypeEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> UpdateModuleTransferLineAsync(long id, ModuleTransferLineEntity moduleTypeEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> DeleteModuleTransferLineAsync(long id);
    }
}
