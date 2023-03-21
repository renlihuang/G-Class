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
    public class ModuleCellBindingService: IModuleCellBindingService, IAutoInjectScoped
    {
        /// <summary>
        /// 
        /// </summary>
        readonly RequestToHttpHelper _requestToHttpHelper;

        public ModuleCellBindingService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        public async Task<QueryPagedResponseModel<ModuleCellBindingEntity>> GetModuleCellBindingListAsync(int pageIndex, int pageSize, ModuleCellBindingCondition condition)
        {
            QueryPagedResponseModel<ModuleCellBindingEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            //if (!string.IsNullOrEmpty(condition.ModuleCode))
            //{
            //    ConditionStringBuilder.Append($"&ModuleCode={condition.ModuleCode}");
            //}

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ModuleCellBinding/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ModuleCellBindingEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ModuleCellBindingEntity>();
            }

            return responseModel;
        }
    }
}
