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
    public class BatteryCellSanCodeService : IBatteryCellSanCodeService,IAutoInjectScoped
    {

        readonly RequestToHttpHelper _requestToHttpHelper;

        public BatteryCellSanCodeService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        public async Task<bool> AddBatteryCellSanCodeAsync(BatteryCellScanCodeEntity entity)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/BatteryCellScanCode/insert";
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }


        public async Task<QueryPagedResponseModel<BatteryCellScanCodeEntity>> GetBatteryCellSanCodeAync(int pageIndex, int pageSize, BatteryCellSanCodeQueryCondition condition)
        {
            QueryPagedResponseModel<BatteryCellScanCodeEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();
            
            //添加用户名查询条件
            if (!string.IsNullOrEmpty(condition.BatteryCellCode))
            {
                ConditionStringBuilder.Append($"&BatteryCellCode={condition.BatteryCellCode}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/BatteryCellScanCode/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<BatteryCellScanCodeEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<BatteryCellScanCodeEntity>();
            }

            return responseModel;

        }
    }
}
