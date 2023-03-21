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
    class ModuleTypeDlg : IBaseModel
    {
        UserControl _view;

        ModuleTypeViewModel _viewModel;

        public ModuleTypeDlg(IModuleTypeService moduleTypeService)
        {
            _viewModel = new ModuleTypeViewModel(moduleTypeService);

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
                _view = new ModuleTypeView();
            }

            return _view;
        }
    }
}
