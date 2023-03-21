using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CS.View.ViewModel.Interface
{
    interface IShowContent
    {
        /// <summary>
        /// 绑定上下文
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="view"></param>
        /// <param name="viewModel"></param>
        void BindDataContex<TView, TViewModel>(TView view, TViewModel viewModel) where TView : UserControl where TViewModel : class;

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <returns></returns>
        Task<bool> ShowDialog();
    }
}
