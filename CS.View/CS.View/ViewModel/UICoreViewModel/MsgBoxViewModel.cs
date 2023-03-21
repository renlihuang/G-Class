using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.View.ViewModel
{
    class MsgBoxViewModel : ViewModelBase
    {
        string _icon { set; get; }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; RaisePropertyChanged(); }
        }

        string _color { set; get; }

        public string Color
        {
            get { return _color; }
            set { _color = value; RaisePropertyChanged(); }
        }

        string _text { set; get; }

        public string Text
        {
            get { return _text; }
            set { _text = value; RaisePropertyChanged(); }
        }

        bool _isHide { set; get; }
        public bool IsHide
        {
            get { return _isHide; }
            set { _isHide = value; RaisePropertyChanged(); }
        }

    }
}
