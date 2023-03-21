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
    class MesLogService: IMesLogService, IAutoInjectScoped
    {
        /// <summary>
        /// 
        /// </summary>
        readonly RequestToHttpHelper _requestToHttpHelper;

        public MesLogService(RequestToHttpHelper requestToHttpHelper)
        {
            this._requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<QueryPagedResponseModel<MesLogEntity>> GetMesLogsAync(int pageIndex, int pageSize, MesLogQueryCondtion mesLogQueryCondtion)
        {
            QueryPagedResponseModel<MesLogEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(mesLogQueryCondtion.ModuleCode))
            {
                ConditionStringBuilder.Append($"&ModuleCode={mesLogQueryCondtion.ModuleCode}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/MesLog/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<MesLogEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<MesLogEntity>();
            }

            return responseModel;
        }
    }
}
