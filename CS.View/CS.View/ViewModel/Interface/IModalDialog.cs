using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.Interface
{
    //模态对话框
    interface IModalDialog
    {
        /// <summary>
        /// 绑定默认的viewModel
        /// </summary>
        void BindDefaultViewModel();

        /// <summary>
        /// 绑定viewModle
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        void BindViewModle<TViewModel>(TViewModel viewModel);

        /// <summary>
        ///注册消息
        /// </summary>
        void RegisterEvenMessage();

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <returns></returns>
        bool ShowDialog();

        /// <summary>
        /// 获取对话框
        /// </summary>
        /// <returns></returns>
        object GetDialog();

        /// <summary>
        /// 关闭对话框
        /// </summary>
        void CloseDialog();
    }
}
