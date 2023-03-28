using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.Base
{
    public class PopupBoxViewModel: ViewModelBase
    {
        public PopupBoxViewModel(object itemParam)
        {
            ItemParam = itemParam;
        }

        private string _iconName;

        /// <summary>
        /// 图片名称
        /// </summary>
        public string IconName
        {
            set { _iconName = value; RaisePropertyChanged(); }
            get { return _iconName; }
        }

        private string _text;

        /// <summary>
        /// 标题名称
        /// </summary>
        public string Text
        {
            set { _text = value; RaisePropertyChanged(); }
            get { return _text; }
        }

        /// <summary>
        /// 是否使能
        /// </summary>
        private bool _isEnable = true;

        public bool IsEnable
        { 
          set { _isEnable = value;RaisePropertyChanged(); }
           get { return _isEnable;}
        }

        /// <summary>
        /// 绑定参数
        /// </summary>
        public object ItemParam { private set; get; }

        /// <summary>
        /// 执行命令
        /// </summary>
        public RelayCommand<object> ExcuteCommand { set; get; }
    }
}
