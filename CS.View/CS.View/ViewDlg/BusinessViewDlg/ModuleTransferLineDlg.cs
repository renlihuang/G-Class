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
    internal class ModuleTransferLineDlg : IBaseModel
    {
        UserControl _view;

        ModuleTransferLineViewModel _viewModel;

        public ModuleTransferLineDlg(IModuleTransferLineService moduleTransferLineService)
        {
            _viewModel = new ModuleTransferLineViewModel(moduleTransferLineService);

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
                _view = new ModuleTransferLineView();
            }

            return _view;
        }
    }
}
