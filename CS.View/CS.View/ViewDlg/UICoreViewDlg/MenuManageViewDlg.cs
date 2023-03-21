using CS.IBLL;
using CS.View.View;
using CS.View.ViewModel;
using CS.View.ViewModel.Interface;
using System.Windows.Controls;

namespace CS.View.ViewDlg
{
    internal class MenuManageViewDlg : IBaseModel
    {
        private UserControl _view;

        private MenuManageViewModel _viewModel;

        public MenuManageViewDlg(IMenusService MenusService)
        {
            _viewModel = new MenuManageViewModel(MenusService);
        }

        public void BindDefaultViewModel()
        {
            GetView().DataContext = _viewModel;
        }

        public void BindViewModel<Tvm>(Tvm viewModel)
        {
            GetView().DataContext = viewModel;
        }

        public UserControl GetView()
        {
            if (_view == null)
            {
                _view = new MenuManageView();
            }

            return _view;
        }
    }
}