using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    class DeviceNetworkViewModel: DataProcess<DeviceNetworkInfoEntity>
    {
        /// <summary>
        /// 数据调用接口
        /// </summary>
        readonly IDeviceNetworkInfoService _deviceNetworkInfoService;

        public DeviceNetworkViewModel(IDeviceNetworkInfoService deviceNetworkInfoService)
        {
            _deviceNetworkInfoService = deviceNetworkInfoService;
            //初始化
            this.Init();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            var result =  await _deviceNetworkInfoService.GetDeviceNetworkInfoListAsync(pageInex, PageSize, new DeviceNetworkInfoCondition() { DeviceName = DeviceName });
            //设置页数
            TotailCount = result.Total;
            DataGridDatas.Clear();

            if (result.Data != null)
            {
                foreach (var dataItem in result.Data)
                {
                    DataGridDatas.Add(dataItem);
                }
            }
            else
            {
                SkinMessageBox.Error("调用接口失败,请检查网络");
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected async override void Save()
        {
            bool result = false;

            if (Mode == ActionMode.Add)
            {
                result = await _deviceNetworkInfoService.AddDeviceNetworkInfoAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _deviceNetworkInfoService.UpdateDeviceNetworkInfoAsync(Model.Id, Model);
            }

            if (result)
            {
                base.Save();
            }
 
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model"></param>
        protected async override void Delete(DeviceNetworkInfoEntity model)
        {
           bool dlgResult = await SkinMessageBox.Question("确认删除?");

            if (dlgResult)
            {
                //删除数据
                bool result = await _deviceNetworkInfoService.DeteleDeviceNetworkInfoAsync(model.Id);
                //删除数据
                if (result)
                {
                    base.Delete(model);
                }
            }

        }


        /// <summary>
        ///  设备名称
        /// </summary>
        private string _deviceName;
        public string DeviceName 
        {
            set { _deviceName = value;RaisePropertyChanged();}
            get { return _deviceName; }
        }

    }
}
