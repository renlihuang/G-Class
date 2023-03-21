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
    internal class RolesService : IRolesService, IAutoInjectScoped
    {
        /// <summary>
        ///
        /// </summary>
        private readonly RequestToHttpHelper _requestToHttpHelper;

        public RolesService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddRole(RoleEntiry roleEntiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Roles/insert";
            httpRequestModel.Data = roleEntiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeleteRole(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Roles/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<bool>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<RoleEntiry> GetRole(long id)
        {
            RoleEntiry roleEntiry;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/Roles/Get?id={id}";
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<RoleEntiry>(httpRequestModel).ConfigureAwait(false);

            roleEntiry = result.IsSuccess == true ? result.BackResult : new RoleEntiry();

            return roleEntiry;
        }

        public async Task<List<RoleEntiry>> GetRoles()
        {
            List<RoleEntiry> roleEntiries;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Roles/GetList";
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<List<RoleEntiry>>(httpRequestModel).ConfigureAwait(false);
            //判断是否
            roleEntiries = result.IsSuccess == true ? result.BackResult : new List<RoleEntiry>();

            return roleEntiries;
        }

        public async Task<QueryPagedResponseModel<RoleEntiry>> GetRolesPage(int pageIndex, int pageSize, RoleQueryCondition condition)
        {
            QueryPagedResponseModel<RoleEntiry> responseModel;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(condition.RoleName))
            {
                ConditionStringBuilder.Append($"&RoleName={condition.RoleName}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/Roles/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<RoleEntiry>>(httpRequestModel).ConfigureAwait(false);
            //判断是否
            responseModel = result.IsSuccess == true ? result.BackResult : new QueryPagedResponseModel<RoleEntiry>();

            return responseModel;
        }

        public async Task<bool> UpdateRole(long id, RoleEntiry roleEntiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Roles/UpdateById/" + id;
            httpRequestModel.Data = roleEntiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<bool>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }
    }
}