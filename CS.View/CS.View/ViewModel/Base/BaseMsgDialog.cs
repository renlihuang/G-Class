using CS.View.ViewModel.Interface;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CS.View.ViewModel.Base
{
    class BaseMsgDialog : IShowContent
    {
        UserControl _view;

        /// <summary>
        /// 绑定上下文
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="view"></param>
        /// <param name="viewModel"></param>
        public void BindDataContex<TView, TViewModel>(TView view, TViewModel viewModel) where TView : UserControl where TViewModel : class
        {
            _view = view;
            _view.DataContext = viewModel;
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ShowDialog()
        {
           bool blResult = false;
           //显示对话框  
           object objResult = await DialogHost.Show(_view, "RootDialog"); //位于顶级窗口

            if (objResult != null)
            {
                blResult = (bool)objResult;
            }
            return blResult;
        }
    }
}
