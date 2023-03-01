

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using CSViewAdminService.Model;
using CSViewAdminService.Utility;
using CSViewAdminService.Model.UserManage.Conditions;
using CSViewAdminService.Core;
using CSViewAdminService.IBLL.UserManage;
namespace CSViewAdminService.BLL.UserManage
{
    internal class CommandUsersServiceImpl : ICommandUsersService, IAutoInject
    {
        
		private readonly IQueryUsersService  queryUsersService;
        private readonly RequestToHttpHelper requestToHttpHelper;
        private readonly string apiHost  = "";

        public CommandUsersServiceImpl(RequestToHttpHelper requestToHttpHelper,IQueryUsersService  queryUsersService)
        {
            this.requestToHttpHelper = requestToHttpHelper;
			this.queryUsersService=queryUsersService;

        }

        #region 插入

        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<long>> InsertAsync(UsersEntity entity)
        {
            HttpResponseResultModel<long> httpResponseResultModel = new HttpResponseResultModel<long> { IsSuccess = false };
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.InsertPath;
            httpRequestModel.Data = entity;
            var result = await requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            httpResponseResultModel = result.BackResult;
            return httpResponseResultModel;
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> InsertBatchAsync(List<UsersEntity> entityList)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.InsertBatchPath;
            httpRequestModel.Data = entityList;
            var result = await requestToHttpHelper.PostAsync<HttpResponseResultModel<bool>>(httpRequestModel).ConfigureAwait(false);
            httpResponseResultModel = result.BackResult;
            return httpResponseResultModel;
        }


        #endregion

	    #region 更新
        /// <summary>
        /// 根据主键更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdateAsync(UsersEntity entity)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.UpdatePath;
            httpRequestModel.Data = entity;
            var result = await requestToHttpHelper.PostAsync<HttpResponseResultModel<bool>>(httpRequestModel).ConfigureAwait(false);
            httpResponseResultModel = result.BackResult;
            return httpResponseResultModel;
        }


       

		#endregion

        #region 删除

        /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteAsync(long id)
        {
		    HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.DeletePath;
            httpRequestModel.Data = id.ToString();
            var result = await requestToHttpHelper.DeleteAsync<HttpResponseResultModel<bool>>(httpRequestModel).ConfigureAwait(false);
            httpResponseResultModel = result.BackResult;
            return httpResponseResultModel;
	   }

        /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="idList">主键集合</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteBatchAsync(IList<long> idList)
        {
		    HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool> { IsSuccess = false };
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = apiHost.ToString();
            httpRequestModel.Path = UsersApiConstant.DeleteBatchPath;
            httpRequestModel.Data = idList;
            var result = await requestToHttpHelper.DeleteAsync<HttpResponseResultModel<bool>>(httpRequestModel).ConfigureAwait(false);
            httpResponseResultModel = result.BackResult;
            return httpResponseResultModel;
		
		}


        #endregion



    }
}
