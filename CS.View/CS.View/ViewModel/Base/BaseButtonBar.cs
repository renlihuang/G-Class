using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CS.View.ViewModel.Base
{
    internal class BaseButtonBar : ViewModelBase
    {
        private string _text;

        /// <summary>
        ///
        /// </summary>
        public string Text
        {
            set { _text = value; RaisePropertyChanged(); }
            get { return _text; }
        }

        private string _icon;

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            set { _icon = value; RaisePropertyChanged(); }
            get { return _icon; }
        }

        private bool _hide;

        /// <summary>
        /// 隐藏显示
        /// </summary>
        public bool Hide
        {
            set { _hide = value; RaisePropertyChanged(); }
            get { return _hide; }
        }

        private int _auth;

        /// <summary>
        /// 权限值
        /// </summary>
        public int Auth
        {
            set { _auth = value; RaisePropertyChanged(); }
            get { return _auth; }
        }

        private RelayCommand _command;

        /// <summary>
        /// 工具栏关联命令
        /// </summary>
        public RelayCommand Command
        {
            set { _command = value; RaisePropertyChanged(); }
            get { return _command; }
        }
    }
}