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
    public interface IModuleTypeService
    {

        Task<QueryPagedResponseModel<ModuleTypeEntity>> GetModuleTypeListAsync(int pageIndex, int pageSize, ModuleTypeCondition condition);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> AddModuleTypeAsync(ModuleTypeEntity moduleTypeEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> UpdateModuleTypeAsync(long id,ModuleTypeEntity moduleTypeEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> DeleteModuleTypeAsync(long id);
    }
}
