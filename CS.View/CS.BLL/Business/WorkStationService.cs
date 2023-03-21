using CS.Base.AppSet;
using CS.Base.HttpHelper;
using CS.Core;
using CS.IBLL.Basic;
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
    class WorkStationService : IWorkStationService, IAutoInjectScoped
    {
        /// <summary>
        /// 
        /// </summary>
        readonly RequestToHttpHelper _requestToHttpHelper;

        public WorkStationService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddRepairOrderAync(RepairOrderEntity entity)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/RepairOrder/insert";
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeleteRepairOrderAync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/RepairOrder/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<RepairOrderEntity>> GetRepairOrderListAync(int pageIndex, int pageSize, RepairOrderCondition condition)
        {
            QueryPagedResponseModel<RepairOrderEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加工单
            if (!string.IsNullOrEmpty(condition.WorkOrderNo))
            {
                ConditionStringBuilder.Append($"&WorkOrderNo={condition.WorkOrderNo}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/RepairOrder/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<RepairOrderEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<RepairOrderEntity>();
            }

            return responseModel;
        }

        public async Task<bool> UpdateRepairOrderAync(long id, RepairOrderEntity entity)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/RepairOrder/UpdateById/" + id;
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }


        public async Task<List<ProductofLineEntity>> GetRecipeListAsync()
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ProductofLine/GetList";
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<List<ProductofLineEntity>>(httpRequestModel).ConfigureAwait(false);

            return result.BackResult;
        }



        public async Task<HttpResponseResultModel<List<ProductofLineEntity>>> GetRecipeListByIDAsync(List<long> ids)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ProductofLine/GetListByIds";
            httpRequestModel.Data = ids;
            //发送POST请求
            return await _requestToHttpHelper.PostAsync<List<ProductofLineEntity>>(httpRequestModel).ConfigureAwait(false);


        }

    }
}
