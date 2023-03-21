using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.View.ViewModel.Base
{
    class BaseToolBar<T> : ViewModelBase
        where T : new()
    {
        string _text;
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            set { _text = value; RaisePropertyChanged(); }
            get { return _text;}
        }

        string _icon;

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            set { _icon = value; RaisePropertyChanged(); }
            get { return _icon; }
        }


        bool  _hide;

        /// <summary>
        /// 隐藏显示
        /// </summary>
        public bool Hide
        {
            set { _hide = value; RaisePropertyChanged(); }
            get { return _hide; }
        }

        int _auth;

        /// <summary>
        /// 权限值
        /// </summary>
        public int Auth
        {
            set { _auth = value; RaisePropertyChanged(); }
            get { return _auth; }
        }

        RelayCommand<T> _command;

        /// <summary>
        /// 工具栏关联命令
        /// </summary>
        public RelayCommand<T> Command
        {
            set { _command = value; RaisePropertyChanged(); }
            get { return _command; }
        }
    }
}
