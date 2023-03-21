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
    class AlarmInfoDlg : IBaseModel
    {
        private UserControl _view;

        private AlarmInfoViewModel _viewModel;

        public AlarmInfoDlg(IErrorLogService errorLogService)
        {
            _viewModel = new AlarmInfoViewModel(errorLogService);
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
            GetView().DataContext = viewModel;
        }

        public UserControl GetView()
        {
            if (_view == null)
            {
                _view = new AlarmInfoView();
            }

            return _view;
        }
    }
}
