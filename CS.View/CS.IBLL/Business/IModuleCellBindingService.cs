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
    public interface IModuleCellBindingService
    {
        Task<QueryPagedResponseModel<ModuleCellBindingEntity>> GetModuleCellBindingListAsync(int pageIndex, int pageSize, ModuleCellBindingCondition condition);

    }
}
