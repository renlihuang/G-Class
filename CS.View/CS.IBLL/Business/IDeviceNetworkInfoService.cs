using CS.Base.HttpHelper;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.IBLL.Business
{
    /// <summary>
    /// 设备网络操作
    /// </summary>
    public interface IDeviceNetworkInfoService
    {
        /// <summary>
        /// 获取所有设备数据
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceNetworkInfoEntity>> GetListAync();
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<DeviceNetworkInfoEntity>> GetDeviceNetworkInfoListAsync(int pageIndex, int pageSize, DeviceNetworkInfoCondition condition);

        /// <summary>
        /// 添加参数名
        /// </summary>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> AddDeviceNetworkInfoAsync(DeviceNetworkInfoEntity entiry);

        /// <summary>
        /// 更新参数名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> UpdateDeviceNetworkInfoAsync(long id, DeviceNetworkInfoEntity entiry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> DeteleDeviceNetworkInfoAsync(long id);
    }
}
