using CS.Base.AppSet;
using CS.Core;
using CS.IBLL;
using CS.Model.Entiry;
using DCS.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.BLL
{
    internal class MenuRightsService : IMenuRightsService, IAutoInjectScoped
    {
        /// <summary>
        /// HTTP请求
        /// </summary>
        private readonly RequestToHttpHelper _requestToHttpHelper;

        public MenuRightsService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<long> AddMenuRights(MenuRightsEntity menuRightsEntity)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/MenuRights/insert";
            httpRequestModel.Data = menuRightsEntity;
            //
            long id = -1;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel);
            //插入数据成功保存ID
            if (result.IsSuccess)
            {
                //接受ID
                id = result.BackResult.BackResult;
            }

            return id;
        }

        public async Task<bool> DeleteMenuRights(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/MenuRights/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel);
            //
            return result.IsSuccess;
        }

        public async Task<List<MenuRightsEntity>> GetMenuIds(long roleID)
        {
            List<MenuRightsEntity> menuEntiries = null;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/MenuRights/GetList?RoleID={roleID}";
            //get请求获取数据
            var result = await _requestToHttpHelper.GetAsync<List<MenuRightsEntity>>(httpRequestModel);
            //判断是否请求成功
            menuEntiries = result.IsSuccess == true ? result.BackResult.OrderBy(x => x.Id).ToList() : new List<MenuRightsEntity>();

            return menuEntiries;
        }

        public async Task<bool> UpdateMenuRights(long id, MenuRightsEntity menuRightsEntity)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/MenuRights/UpdateById/" + id;
            httpRequestModel.Data = menuRightsEntity;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel);
            //
            return result.IsSuccess;
        }
    }
}