using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel.ParamEdit
{
    public class GridItem : ViewModelBase
    {
        /// <summary>
        /// 参数值
        /// </summary>
        string _key;
        public string Key
        {
            set { _key = value; RaisePropertyChanged(); }
            get { return _key; }
        }

        //参数值
        string _value;
        public string Value
        {
            set { _value = value; RaisePropertyChanged(); }
            get { return _value; }
        }
    }
}
