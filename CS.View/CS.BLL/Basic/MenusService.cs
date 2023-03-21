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
    public class MenusService : IMenusService, IAutoInjectScoped
    {
        /// <summary>
        /// HTTP请求
        /// </summary>
        private readonly RequestToHttpHelper _requestToHttpHelper;

        public MenusService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<long> AddMenuAsync(MenuEntiry mensEntiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Menus/insert";
            httpRequestModel.Data = mensEntiry;
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

        public async Task<bool> UpdateMenuAsync(long id, MenuEntiry mensEntiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Menus/UpdateById/" + id;
            httpRequestModel.Data = mensEntiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel);
            //
            return result.IsSuccess;
        }

        public async Task<MenuEntiry> GetMenuByID(long id)
        {
            MenuEntiry menuEntiry = null;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/Menus/Get?ID={id}";
            //get请求获取数据
            var result = await _requestToHttpHelper.GetAsync<MenuEntiry>(httpRequestModel);
            //判断是否请求成功
            menuEntiry = result.IsSuccess == true ? result.BackResult : new MenuEntiry();

            return menuEntiry;
        }

        public async Task<List<MenuEntiry>> GetMenusByParentID(long parentID)
        {
            List<MenuEntiry> menuEntiries = null;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = $"/Menus/GetList?ParentID={parentID}";
            //get请求获取数据
            var result = await _requestToHttpHelper.GetAsync<List<MenuEntiry>>(httpRequestModel);
            //判断是否请求成功
            menuEntiries = result.IsSuccess == true ? result.BackResult.OrderBy(x => x.Id).ToList() : new List<MenuEntiry>();

            return menuEntiries;
        }

        public async Task<bool> HasChildren(long id)
        {
            var result = await GetMenusByParentID(id);

            return result.Count > 0;
        }

        public async Task<bool> DeleteMenuAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.WebApiHost;
            httpRequestModel.Path = "/Menus/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel);
            //
            return result.IsSuccess;
        }
    }
}