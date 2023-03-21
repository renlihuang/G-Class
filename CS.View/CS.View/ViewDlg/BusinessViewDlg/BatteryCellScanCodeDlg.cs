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
    internal class BatteryCellScanCodeDlg : IBaseModel
    {
        private UserControl _userControl;
        private BatteryCellScanCodeViewModel _viewModel;
        public  BatteryCellScanCodeDlg(IBatteryCellSanCodeService batteryCellSanCodeService)
        {
            _viewModel = new BatteryCellScanCodeViewModel(batteryCellSanCodeService);
        }

        public void BindDefaultViewModel()
        {
            if (_viewModel != null)
            {
                GetView().DataContext = _viewModel;
            }
        }

        public void BindViewModel<Tvm>(Tvm viewModel)
        {
            GetView().DataContext = _viewModel;
        }

        public UserControl GetView()
        {
            if (_userControl == null)
            {
                _userControl = new BatteryCellSanCodeView();
            }

            return _userControl;
        }
    }
}
