using CS.Base.AppSet;
using CS.Base.HttpHelper;
using CS.Core;
using CS.IBLL;
using CS.Model.Entiry;
using CS.Model.QueryCondition;
using DCS.BASE;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UsersService : IUsersService, IAutoInjectScoped
    {
        /// <summary>
        /// HTTP请求
        /// </summary>
        private readonly RequestToHttpHelper _requestToHttpHelper;

        public UsersService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userEntiry"></param>
        /// <returns></returns>
        public async Task<bool> AddUserAync(UserEntiry userEntiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Users/insert";
            httpRequestModel.Data = userEntiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userQueryCondition"></param>
        /// <returns></returns>
        public async Task<QueryPagedResponseModel<UserEntiry>> GetUsersAync(int pageIndex, int pageSize, UserQueryCondition userQueryCondition)
        {
            QueryPagedResponseModel<UserEntiry> responseModel;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(userQueryCondition.UserName))
            {
                ConditionStringBuilder.Append($"&UserName={userQueryCondition.UserName}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/Users/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<UserEntiry>>(httpRequestModel).ConfigureAwait(false);
            //判断是否
            responseModel = result.IsSuccess == true ? result.BackResult : new QueryPagedResponseModel<UserEntiry>();

            return responseModel;
        }

        /// <summary>
        /// 更新条码
        /// </summary>
        /// <param name="userEntiry"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserAync(UserEntiry userEntiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Users/UpdateById/" + userEntiry.ID;
            httpRequestModel.Data = userEntiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userEntiry"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAync(UserEntiry userEntiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //伪删除
            userEntiry.IsDeleted = 1;
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Users/UpdateById/" + userEntiry.ID;
            httpRequestModel.Data = userEntiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<List<UserEntiry>> GetUserAync(string userName)
        {
            List<UserEntiry> userEntiries;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/Users/GetList?&UserName={userName}";
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<List<UserEntiry>>(httpRequestModel).ConfigureAwait(false);
            //判断是否
            userEntiries = result.IsSuccess == true ? result.BackResult : new List<UserEntiry>();

            return userEntiries;
        }

        public async Task<List<UserEntiry>> GetAllUsersAync()
        {
            List<UserEntiry> userEntiries;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/Users/GetList";
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<List<UserEntiry>>(httpRequestModel).ConfigureAwait(false);
            //判断是否
            userEntiries = result.IsSuccess == true ? result.BackResult : new List<UserEntiry>();

            return userEntiries;
        }
    }
}