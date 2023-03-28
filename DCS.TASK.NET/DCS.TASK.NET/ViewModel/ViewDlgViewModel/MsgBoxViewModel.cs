using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DCS.TASK.NET.ViewModel.ViewDlgViewModel
{
    internal class MsgBoxViewModel : BaseDIalogViewModel
    {
        public MsgBoxViewModel(Window ownerWindow) : base(ownerWindow)
        {
        }


        protected override void OnConfrim()
        {
            DialogResult = true;
            base.OnConfrim();
        }


        /// <summary>
        /// 图标
        /// </summary>
        string _icon { set; get; }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 颜色
        /// </summary>
        string _color { set; get; }

        public string Color
        {
            get { return _color; }
            set { _color = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 文本
        /// </summary>
        string _text { set; get; }

        public string Text
        {
            get { return _text; }
            set { _text = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 是否隐藏取消按钮
        /// </summary>
        bool _isHide { set; get; }
        public bool IsHide
        {
            get { return _isHide; }
            set { _isHide = value; RaisePropertyChanged(); }
        }
    }
}
