using CS.Base.AppSet;
using CS.Base.HttpHelper;
using CS.Core;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using DCS.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL.Business
{
    class WorkOrderManagementService : IWorkOrderManagementService, IAutoInjectScoped
    {
        readonly RequestToHttpHelper _requestToHttpHelper;

        public WorkOrderManagementService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<QueryPagedResponseModel<WorkOrderManagementEntity>> GetWorkOrderManagementAync(int pageIndex, int pageSize, WorkOrderManagementCondition condtion)
        {
            QueryPagedResponseModel<WorkOrderManagementEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(condtion.WorkOrderNo))
            {
                ConditionStringBuilder.Append($"&WorkOrderNo={condtion.WorkOrderNo}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/WorkOrderManagement/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<WorkOrderManagementEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<WorkOrderManagementEntity>();
            }

            return responseModel;
        }

        public async Task<bool> InsertAsync(WorkOrderManagementEntity entity, bool isCommit = true)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/WorkOrderManagement/Insert";
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            return result.IsSuccess;
        }
        public async Task<bool> UpdateAsync(WorkOrderManagementEntity entity, bool isCommit = true)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/WorkOrderManagement/UpdateById/" + entity.Id;
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            return result.IsSuccess;
        }

        public async Task<bool> InsertWorkOrderManagementBatch(List<WorkOrderManagementEntity> entities)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/WorkOrderManagement/InsertBatch";
            httpRequestModel.Data = entities;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeleteAsync(WorkOrderManagementEntity entity, bool isCommit = true)
        {

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            entity.IsDeleted = 1;
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/WorkOrderManagement/UpdateById/" + entity.Id;
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

    }
}
