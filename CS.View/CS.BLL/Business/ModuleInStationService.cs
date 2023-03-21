using CS.Base.AppSet;
using CS.Base.HttpHelper;
using CS.Core;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.Model.QueryCondition;
using DCS.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL.Business
{
    /// <summary>
    /// 模组进站
    /// </summary>
    public class ModuleInStationService : IModuleInStationService, IAutoInjectScoped
    {
        /// <summary>
        /// 
        /// </summary>
        readonly RequestToHttpHelper _requestToHttpHelper;

        public ModuleInStationService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<QueryPagedResponseModel<ModuleInStationEntity>> GetModuleInStationsAync(int pageIndex, int pageSize, ModuleInStationQueryCondtion moduleInStationQueryCondtion)
        {
            QueryPagedResponseModel<ModuleInStationEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(moduleInStationQueryCondtion.ModuleCode))
            {
                ConditionStringBuilder.Append($"&ModuleCode={moduleInStationQueryCondtion.ModuleCode}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ModuleInStation/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ModuleInStationEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ModuleInStationEntity>();
            }

            return responseModel;
        }
    }
}
