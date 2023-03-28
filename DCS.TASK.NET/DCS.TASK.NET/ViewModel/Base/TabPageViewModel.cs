using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.Base
{
    /// <summary>
    /// tab也
    /// </summary>
    internal class TabPageViewModel : ViewModelBase
    {
        /// <summary>
        /// tab页图标
        /// </summary>
        private string _pageIcon;
        public string PageIcon
        {
            set { _pageIcon = value; RaisePropertyChanged(); }
            get { return _pageIcon; }
        }

        /// <summary>
        /// tab
        /// </summary>
        private string _pageText;
        public string PageText
        {
            set{ _pageText = value;RaisePropertyChanged();}
            get { return _pageText; }
        }

        /// <summary>
        /// 
        /// </summary>
        private object _pageItem;
        public object PageItem
        {
            set { _pageItem = value; RaisePropertyChanged(); }
            get { return _pageItem; }
        }

        /// <summary>
        /// TAB页附加参数
        /// </summary>
        public object ItemParam { set; get; }
    }
}
