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
    public interface IWorkOrderManagementService
    {
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> InsertWorkOrderManagementBatch(List<WorkOrderManagementEntity> entities);


        /// <summary>
        /// 查询模组进站记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userQueryCondition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<WorkOrderManagementEntity>> GetWorkOrderManagementAync(int pageIndex, int pageSize, WorkOrderManagementCondition condtion);


        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(WorkOrderManagementEntity entity, bool isCommit = true);

        Task<bool> UpdateAsync(WorkOrderManagementEntity entity, bool isCommit = true);

        Task<bool> DeleteAsync(WorkOrderManagementEntity entity, bool isCommit = true);
    }
}
