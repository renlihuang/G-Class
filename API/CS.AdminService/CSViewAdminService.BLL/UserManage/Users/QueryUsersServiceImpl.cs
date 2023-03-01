

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using CSViewAdminService.Model;
using CSViewAdminService.IBLL;
using CSViewAdminService.Utility;
using CSViewAdminService.Model.UserManage.Conditions;
using CSViewAdminService.Core;
using CSViewAdminService.IBLL.UserManage;
namespace CSViewAdminService.BLL.UserManage
{
    internal class QueryUsersServiceImpl : IQueryUsersService, IAutoInject
    {
        private readonly RequestToHttpHelper requestToHttpHelper;
        private readonly string apiHost  = "";
        public QueryUsersServiceImpl(RequestToHttpHelper requestToHttpHelper)
        {
            this.requestToHttpHelper = requestToHttpHelper;
        }

 

        /// <summary>
        /// 查询数据(分页) 返回指定实体T
        /// </summary>
        ///<typeparam name="T">返回实体类型</typeparam>
        /// <param name="page">页码</param>
        /// <param name="size">每页数量</param>
        /// <param name="condition">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public async Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseUsersCondition condition=null, string field = null, string orderBy = null)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.QueryPagedListPath+ $"?page={page}&size={size}" + QueryConditionHelper.GetUrlQueryCondition(condition);
            var result = await requestToHttpHelper.GetAsync<QueryPagedResponseModel<T>>(httpRequestModel).ConfigureAwait(false);
            return result.BackResult;
        }


        


        /// <summary>
        /// 查询数据(不分页) 返回指定实体T
        /// </summary>
        ///<typeparam name="T">返回实体类型</typeparam>
        /// <param name="page">页码</param>
        /// <param name="size">每页数量</param>
        /// <param name="condition">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<T>(BaseUsersCondition  condition=null, string field = null, string orderBy = null)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.QueryListPath+"?"+ QueryConditionHelper.GetUrlQueryCondition(condition);
            var result = await requestToHttpHelper.GetAsync<List<T>>(httpRequestModel).ConfigureAwait(false);
            return result.BackResult;
         }

        /// <summary>
        /// 根据主键获取单个实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.QuerySingleByIdPath + $"?id={id}";
            var result = await requestToHttpHelper.GetAsync<T>(httpRequestModel).ConfigureAwait(false);
            return result.BackResult;
        }

        /// <summary>
        /// 根据主键集合获取多个实体
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
         public async Task<List<T>> GetListByIdsAsync<T>(List<long> idList)
         {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.QueryListByIdsPath;
            httpRequestModel.Data = idList;
            var result = await requestToHttpHelper.PostAsync<HttpResponseResultModel<List<T>>>(httpRequestModel).ConfigureAwait(false);
            return  result.BackResult.BackResult;
      
         }

		 
                

    }
}
