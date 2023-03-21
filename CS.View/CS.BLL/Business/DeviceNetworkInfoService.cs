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
    class DeviceNetworkInfoService : IDeviceNetworkInfoService, IAutoInjectScoped
    {
        /// <summary>
        /// 
        /// </summary>
        readonly RequestToHttpHelper _requestToHttpHelper;

        public DeviceNetworkInfoService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddDeviceNetworkInfoAsync(DeviceNetworkInfoEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/DeviceNetworkInfo/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeteleDeviceNetworkInfoAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/DeviceNetworkInfo/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }

        public async Task<QueryPagedResponseModel<DeviceNetworkInfoEntity>> GetDeviceNetworkInfoListAsync(int pageIndex, int pageSize, DeviceNetworkInfoCondition condition)
        {
            QueryPagedResponseModel<DeviceNetworkInfoEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //添加用户名查询条件
            if (!string.IsNullOrEmpty(condition.DeviceName))
            {
                ConditionStringBuilder.Append($"&DeviceName={condition.DeviceName}");
            }

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/DeviceNetworkInfo/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<DeviceNetworkInfoEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<DeviceNetworkInfoEntity>();
            }

            return responseModel;
        }

        public async Task<List<DeviceNetworkInfoEntity>> GetListAync()
        {
            List<DeviceNetworkInfoEntity> deviceNetworkInfoEntities;

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/DeviceNetworkInfo/GetList";
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<List<DeviceNetworkInfoEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                deviceNetworkInfoEntities = result.BackResult;
            }
            else
            {
                deviceNetworkInfoEntities = new  List<DeviceNetworkInfoEntity>();
            }

            return deviceNetworkInfoEntities;
        }

        public async Task<bool> UpdateDeviceNetworkInfoAsync(long id, DeviceNetworkInfoEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/DeviceNetworkInfo/UpdateById/" + id;
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }
    }
}
