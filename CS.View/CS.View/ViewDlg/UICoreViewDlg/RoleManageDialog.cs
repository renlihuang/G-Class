using CS.IBLL;
using CS.View.View;
using CS.View.ViewModel;
using CS.View.ViewModel.Interface;
using System.Windows.Controls;

namespace CS.View.ViewDlg
{
    //权限管理对话框
    internal class RoleManageDialog : IBaseModel
    {
        //当前窗口
        private UserControl _view;

        //当前窗口对应的viewModel
        private RoleManageViewModel _ViewModel;

        /// <summary>
        ///
        /// </summary>
        /// <param name="usersService"></param>
        public RoleManageDialog(IRolesService menusService)
        {
            _ViewModel = new RoleManageViewModel(menusService);
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
                _view = new RoleManageView();
            }

            return _view;
        }
    }
}