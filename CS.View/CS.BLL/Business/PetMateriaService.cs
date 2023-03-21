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
    public class PetMateriaService : IPetMateriaService, IAutoInjectScoped
    {
        readonly RequestToHttpHelper _requestToHttpHelper;

        public PetMateriaService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddPetMateriaAsync(PetMateriaEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/PetMateria/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DetelePetMateriaAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/PetMateria/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<PetMateriaEntity>> GetPetMateriaAsync(int pageIndex, int pageSize, PetMateriaCondition condition)
        {
            QueryPagedResponseModel<PetMateriaEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //物料代码
            if (!string.IsNullOrEmpty(condition.MaterialCode))
            {
                ConditionStringBuilder.Append($"&MaterialCode={condition.MaterialCode}");
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
            httpRequestModel.Path = $"/PetMateria/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<PetMateriaEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<PetMateriaEntity>();
            }

            return responseModel;
        }

        public async Task<bool> UpdatePetMateriaAsync(long id, PetMateriaEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/PetMateria/UpdateById/" + id;
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }


    }
}
