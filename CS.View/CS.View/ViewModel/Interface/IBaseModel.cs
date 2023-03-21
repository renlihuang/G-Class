using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CS.View.ViewModel.Interface
{
    interface IBaseModel
    {
        /// <summary>
        /// 绑定viewModel
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        void BindViewModel<Tvm>(Tvm viewModel);

        /// <summary>
        /// 绑定默认的viewModel
        /// </summary>
        void BindDefaultViewModel();

        /// <summary>
        /// 获取默认view
        /// </summary>
        /// <returns></returns>
        UserControl GetView();

    }
}
