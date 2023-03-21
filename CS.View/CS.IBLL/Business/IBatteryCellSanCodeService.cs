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
    
    public interface IBatteryCellSanCodeService
    {
        Task<bool> AddBatteryCellSanCodeAsync(BatteryCellScanCodeEntity entiry);
        Task<QueryPagedResponseModel<BatteryCellScanCodeEntity>> GetBatteryCellSanCodeAync(int pageIndex, int pageSize, BatteryCellSanCodeQueryCondition condtion);
    }
}
