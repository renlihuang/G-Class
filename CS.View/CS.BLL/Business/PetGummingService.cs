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
    public class PetGummingService: IPetGummingService, IAutoInjectScoped
    {
        readonly RequestToHttpHelper _requestToHttpHelper;

        public PetGummingService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddPetGummingAsync(PetGummingEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/PetGumming/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DetelePetGummingAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/PetGumming/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<PetGummingEntity>> GetPetGummingAsync(int pageIndex, int pageSize, PetGummingCondition condition)
        {
            QueryPagedResponseModel<PetGummingEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //托盘码
            if (!string.IsNullOrEmpty(condition.ProductCode))
            {
                ConditionStringBuilder.Append($"&ProductCode={condition.ProductCode}");
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
            httpRequestModel.Path = $"/PetGumming/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<PetGummingEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<PetGummingEntity>();
            }

            return responseModel;
        }

        public async Task<bool> UpdatePetGummingAsync(long id, PetGummingEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/PetGumming/UpdateById/" + id;
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }

    }
}
