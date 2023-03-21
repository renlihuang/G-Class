using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel.ParamEdit
{
    class ParamModifyViewModel: ViewModelBase
    {
        /// <summary>
        /// 参数值
        /// </summary>
        private string _paramKey;
        public string ParamKey
        {
            set { _paramKey = value; RaisePropertyChanged(); }
            get { return _paramKey; }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        private string _paramValue;
        public string ParamValue
        {
            set { _paramValue = value; RaisePropertyChanged(); }
            get { return _paramValue; }
        }
    }
}
