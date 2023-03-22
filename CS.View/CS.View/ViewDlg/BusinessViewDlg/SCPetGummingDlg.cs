﻿using CS.IBLL.Business;
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
    public class SCPetGummingDlg : IBaseModel
    {
        //当前窗口
        private UserControl _view;

        //当前窗口对应的viewModel
        internal SCPetGummingViewModel _ViewModel;

        public SCPetGummingDlg(ISCPetGummingService paramNameService)
        {
            _ViewModel = new SCPetGummingViewModel(paramNameService);
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
                _view = new SCPetGummingView();
            }
            return _view;
        }
    }
}