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
    class ModuleGroupService : IModuleGroupService, IAutoInjectScoped
    {
        readonly RequestToHttpHelper _requestToHttpHelper;

        public ModuleGroupService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<QueryPagedResponseModel<ModuleGroupEntity>> GetModuleGroupsAync(int pageIndex, int pageSize, ModuleGroupCondition condtion)
        {
            QueryPagedResponseModel<ModuleGroupEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(condtion.GroupCode))
            {
                ConditionStringBuilder.Append($"&GroupCode={condtion.GroupCode}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ModuleGroup/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ModuleGroupEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ModuleGroupEntity>();
            }

            return responseModel;
        }

        public async Task<bool> InsertModuleGroupBatch(List<ModuleGroupEntity> entities)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ModuleGroup/InsertBatch";
            httpRequestModel.Data = entities;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }
    }
}
