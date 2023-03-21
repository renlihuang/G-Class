using CS.View.ViewModel.Interface;
using System.Windows;

namespace CS.View.ViewModel.Base
{
    internal class BaseModiaDialog<TView> : IModalDialog where TView : new()
    {
        public BaseModiaDialog()
        {
            //绑定默认的viewModel
            BindDefaultViewModel();
        }

        /// <summary>
        /// 对话框返回值
        /// </summary>
        protected string _dialogResult;

        /// <summary>
        ///当前窗口
        /// </summary>
        private TView _view;

        /// <summary>
        /// 绑定默认的viewModel
        /// </summary>
        public virtual void BindDefaultViewModel() { }

        /// <summary>
        /// 绑定viewModle
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        public virtual void BindViewModle<TViewModel>(TViewModel viewModel)
        {
            Window winDlg = GetDialog() as Window;
            //绑定ViewModel
            if (winDlg != null)
            {
                winDlg.DataContext = viewModel;
            }
        }

        /// <summary>
        ///注册消息
        /// </summary>
        public virtual void RegisterEvenMessage() { }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <returns></returns>
        public virtual bool ShowDialog()
        {
            return true;
        }

        /// <summary>
        /// 获取对话框
        /// </summary>
        /// <returns></returns>
        public virtual object GetDialog()
        {
            if (_view == null)
            {
                _view = new TView();
                //注册默认事件
                RegisterEvenMessage();
            }
            return _view;
        }

        /// <summary>
        /// 返回对话框信息
        /// </summary>
        /// <returns></returns>
        public virtual string GetDialogResult()
        {
            return _dialogResult;
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        public virtual void CloseDialog() { }
    }
}