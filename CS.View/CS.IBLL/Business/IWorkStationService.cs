using CS.Base.HttpHelper;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using DCS.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.IBLL.Business
{
    public interface IWorkStationService
    {
        /// <summary>
        /// 获取所有模组列表
        /// </summary>
        /// <returns></returns>
        Task<List<ProductofLineEntity>> GetRecipeListAsync();

        /// <summary>
        /// 获取所有模组列表
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseResultModel<List<ProductofLineEntity>>> GetRecipeListByIDAsync(List<long> ids);

        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddRepairOrderAync(RepairOrderEntity entity);

        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateRepairOrderAync(long id, RepairOrderEntity entity);


        /// <summary>
        /// 添加工单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteRepairOrderAync(long id);

        /// <summary>
        /// 查询工单列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<RepairOrderEntity>> GetRepairOrderListAync(int pageIndex, int pageSize, RepairOrderCondition condition);

    }
}
