using CS.IBLL;
using CS.View.View;
using CS.View.ViewModel;
using CS.View.ViewModel.Interface;
using System.Windows.Controls;

namespace CS.View.ViewDlg
{
    internal class UserManageViewDlg : IBaseModel
    {
        //当前窗口
        private UserControl _view;

        //当前窗口对应的viewModel
        private UserManageViewModel _userManageViewModel;

        /// <summary>
        ///
        /// </summary>
        /// <param name="usersService"></param>
        public UserManageViewDlg(IUsersService usersService, IRolesService rolesService)
        {
            _userManageViewModel = new UserManageViewModel(usersService, rolesService);
        }

        /// <summary>
        /// 绑定默认的viewModel
        /// </summary>
        public void BindDefaultViewModel()
        {
            if (_userManageViewModel != null)
            {
                GetView().DataContext = _userManageViewModel;
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
                _view = new UserManageView();
            }

            return _view;
        }
    }
}