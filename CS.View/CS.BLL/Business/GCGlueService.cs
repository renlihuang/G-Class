using CS.Base.AppSet;
using CS.Base.HttpHelper;
using CS.Core;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using DCS.BASE;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL.Business
{
    internal class GCGlueService : IGCGlueService, IAutoInjectScoped
    {
        private readonly RequestToHttpHelper _requestToHttpHelper;

        public GCGlueService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddGCGlueAsync(GCGlueEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/GCGlue/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeteleGCGlueAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/GCGlue/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<GCGlueEntity>> GetGCGlueAsync(int pageIndex, int pageSize, GCGlueCondition condition)
        {
            QueryPagedResponseModel<GCGlueEntity> responseModel = null;
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
            httpRequestModel.Path = $"/GCGlue/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<GCGlueEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<GCGlueEntity>();
            }

            return responseModel;
        }

        public async Task<bool> UpdateGCGlueAsync(long id, GCGlueEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/GCGlue/UpdateById/" + id;
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }
    }
}