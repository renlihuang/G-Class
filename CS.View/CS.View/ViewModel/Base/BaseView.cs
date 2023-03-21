using CS.View.ViewModel.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CS.View.ViewModel.Base
{
    /// <summary>
    /// 基本view
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    class BaseView<TView,TViewModel> : IBaseModel
        where TView : UserControl ,new()
        where TViewModel : new()
    {
        TView _view;


        TViewModel _viewModel;

        /// <summary>
        /// 绑定viewModel
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        public void BindViewModel<Tvm>(Tvm viewModel)
        {
            GetView().DataContext = viewModel;
        }

        /// <summary>
        /// 绑定默认的viewModel
        /// </summary>
        public void BindDefaultViewModel()
        {
            if (_viewModel == null)
            {
                _viewModel = new TViewModel();
            }
            //绑定viewModel
            GetView().DataContext = _viewModel;
        }

        /// <summary>
        /// 获取默认view
        /// </summary>
        /// <returns></returns>
        public virtual UserControl GetView()
        {
            if (_view == null)
            {
                _view = new TView();
            }
            return _view;
        }
    }
}
