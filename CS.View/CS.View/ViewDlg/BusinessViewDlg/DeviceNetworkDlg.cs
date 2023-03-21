using CS.IBLL.Business;
using CS.View.View.BusinessView;
using CS.View.ViewModel.BusinessViewModel;
using CS.View.ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CS.View.ViewDlg.BusinessViewDlg
{
    class DeviceNetworkDlg: IBaseModel
    {
        //当前窗口
        private UserControl _view;

        //当前窗口对应的viewModel
        private DeviceNetworkViewModel _ViewModel;

        public DeviceNetworkDlg(IDeviceNetworkInfoService DeviceNetworkViewModel)
        {
            _ViewModel = new DeviceNetworkViewModel(DeviceNetworkViewModel);
        }

        /// <summary>
        /// 绑定默认的viewModel
        /// </summary>
        public void BindDefaultViewModel()
        {
            if (_ViewModel != null)
            {
                GetView().DataContext = _ViewModel;
            }
        }

        public void BindViewModel<Tvm>(Tvm viewModel)
        {
            GetView().DataContext = viewModel;
        }

        public UserControl GetView()
        {
            if (_view == null)
            {
                _view = new DeviceNetworkInfoView();
            }

            return _view;
        }
    }
}
