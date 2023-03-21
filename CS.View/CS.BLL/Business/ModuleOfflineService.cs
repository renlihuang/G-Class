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
    class ModuleOfflineService : IModuleOfflineService, IAutoInjectScoped
    {

        readonly RequestToHttpHelper _requestToHttpHelper;

        public ModuleOfflineService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddModuleOfflineAsync(ModuleOfflineEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ModuleOffline/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

  
        public async Task<QueryPagedResponseModel<ModuleOfflineEntity>> GetModuleOfflinesAsync(int pageIndex, int pageSize, ModuleOfflineCondition condition)
        {
            QueryPagedResponseModel<ModuleOfflineEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(condition.ModuleCode))
            {
                ConditionStringBuilder.Append($"&ModuleCode={condition.ModuleCode}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ModuleOffline/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ModuleOfflineEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ModuleOfflineEntity>();
            }

            return responseModel;
        }
    }
}
