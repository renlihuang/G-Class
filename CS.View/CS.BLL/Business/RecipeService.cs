using CS.Base.AppSet;
using CS.Base.HttpHelper;
using CS.Core;
using CS.IBLL.Basic;
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
    class RecipeService : IRecipeService, IAutoInjectScoped
    {
        /// <summary>
        /// 
        /// </summary>
        readonly RequestToHttpHelper _requestToHttpHelper;

        public RecipeService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddRecipeAync(ProductofLineEntity entity)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ProductofLine/insert";
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeleteRecipeAync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ProductofLine/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<ProductofLineEntity>> GetRecipeListAync(int pageIndex,int pageSize,ModuleAssembleRecipeCondition condition)
        {
            QueryPagedResponseModel<ProductofLineEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(condition.RecipeName))
            {
                ConditionStringBuilder.Append($"&ProductType={condition.RecipeName}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ProductofLine/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ProductofLineEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ProductofLineEntity>();
            }

            return responseModel;
        }

        public async Task<bool> UpdateRecipeAync(long id, ProductofLineEntity entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.ProductLine))
            { 
                string Lines = string.Empty;
                List<ProductEntiry> productLines = JsonHelper.DeserializeObject<List<ProductEntiry>>(entity.ProductLine);
                foreach (ProductEntiry entiry in productLines)
                {
                    Lines += entiry.ProductName + ",";
                }
                entity.ProductLineD = Lines.Substring(0, Lines.Length - 1);
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/ProductofLine/UpdateById/" + id;
            httpRequestModel.Data = entity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }


        public async Task<List<ProductEntity>> GetRecipeListAsync()
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/Product/GetList";
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<List<ProductEntity>>(httpRequestModel).ConfigureAwait(false);

            return result.BackResult;
        }

     

        public async Task<HttpResponseResultModel<List<ProductEntity>>> GetRecipeListByIDAsync(List<long> ids)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/Product/GetListByIds";
            httpRequestModel.Data = ids;
            //发送POST请求
            return await _requestToHttpHelper.PostAsync<List<ProductEntity>>(httpRequestModel).ConfigureAwait(false);


        }
    }
}
