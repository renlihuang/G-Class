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
    internal class ModelCellBindingDlg:IBaseModel
    {
        UserControl _view;

        ModuleCellBindingViewModel _viewModel;

        public ModelCellBindingDlg(IModuleCellBindingService moduleCellBindingService)
        {
            _viewModel = new ModuleCellBindingViewModel(moduleCellBindingService);

        }

        public void BindDefaultViewModel()
        {
            GetView().DataContext = _viewModel;
        }

        public void BindViewModel<Tvm>(Tvm viewModel)
        {

        }

        public UserControl GetView()
        {
            if (_view == null)
            {
                _view = new ModuleCellBindingView();
            }

            return _view;
        }
    }
}
