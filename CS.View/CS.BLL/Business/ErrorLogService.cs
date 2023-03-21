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
    public class ErrorLogService:IErrorLogService, IAutoInjectScoped
    {
        readonly RequestToHttpHelper _requestToHttpHelper;

        public ErrorLogService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddErrorLogAsync(ErrorLogEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ErrorLog/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeteleErrorLogAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ErrorLog/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<ErrorLogEntity>> GetErrorLogAsync(int pageIndex, int pageSize, ErrorLogCondition condition)
        {
            QueryPagedResponseModel<ErrorLogEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //报错代码
            if (!string.IsNullOrEmpty(condition.ErrorCode))
            {
                ConditionStringBuilder.Append($"&ErrorCode={condition.ErrorCode}");
            }
            //// 开始时间
            //if (!string.IsNullOrEmpty(condition.CreateTimeStart))
            //{
            //    ConditionStringBuilder.Append($"&CreateTimeStart={condition.CreateTimeStart}");
            //}
            //// 结束时间
            //if (!string.IsNullOrEmpty(condition.CreateTimeEnd))
            //{
            //    DateTime end = Convert.ToDateTime(condition.CreateTimeEnd);
            //    end = end.AddDays(1);
            //    ConditionStringBuilder.Append($"&CreateTimeEnd={end.ToString()}");
            //}
            //添加模组号查询条件
            //if (!string.IsNullOrEmpty(condition.ModuleCode))
            //{
            //    ConditionStringBuilder.Append($"&ModuleCode={condition.ModuleCode}");
            //}

            //按父节点ID
            //if (condition.ID > 0)
            //{
            //    ConditionStringBuilder.Append($"&ID={condition.ID}");
            //}

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ErrorLog/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ErrorLogEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ErrorLogEntity>();
            }

            return responseModel;
        }

        public async Task<bool> UpdateErrorLogAsync(long id, ErrorLogEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ErrorLog/UpdateById/" + id;
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }

    }
}
