using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel.ParamEdit
{
    class SectionItem: ViewModelBase
    {
        string _name;
        public string Name
        {
            set { _name = value;RaisePropertyChanged(); }
            get { return _name; }
        }
    }
}
