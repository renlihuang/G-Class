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
    internal class GCPDPService : IGCPDPService, IAutoInjectScoped
    {
        readonly RequestToHttpHelper _requestToHttpHelper;

        public GCPDPService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddGCPDPAAsync(GCPDPEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/GCPDP/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeteleGCPDPAAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/GCPDP/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<GCPDPEntity>> GetGCPDPAsync(int pageIndex, int pageSize, GCPDPCondition condition)
        {
            QueryPagedResponseModel<GCPDPEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //托盘码
            if (!string.IsNullOrEmpty(condition.ModuleCode))
            {
                ConditionStringBuilder.Append($"&ModuleCode={condition.ModuleCode}");
            }
            //// 开始时间 结束时间
            //if (!string.IsNullOrEmpty(condition.CreateTimeStart) && !string.IsNullOrEmpty(condition.CreateTimeEnd))
            //{
            //    ConditionStringBuilder.Append($"&CreateTime={condition.CreateTimeStart}");
            //    ConditionStringBuilder.Append($"&CreateTimeEnd={condition.CreateTimeEnd}");
            //}

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/GCPDP/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<GCPDPEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<GCPDPEntity>();
            }

            return responseModel;
        }
        public async Task<bool> UpdateGCPDPAAsync(long id, GCPDPEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/GCPDP/UpdateById/" + id;
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }
    }
}
