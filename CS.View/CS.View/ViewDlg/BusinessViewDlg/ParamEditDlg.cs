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
    class ParamEditDlg : IBaseModel
    {
        //当前窗口
        private UserControl _view;

        public void BindDefaultViewModel()
        {
            GetView().DataContext = new ParamEditViewModel();
        }

        public void BindViewModel<Tvm>(Tvm viewModel)
        {
            GetView().DataContext = viewModel;
        }

        public UserControl GetView()
        {
            if (_view == null)
            {
                _view = new ParamEditView();
            }

            return _view;
        }
    }
}
