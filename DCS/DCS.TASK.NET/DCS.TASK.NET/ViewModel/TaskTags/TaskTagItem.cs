using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.TaskTags
{
    internal class TaskTagItem : ViewModelBase
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        private string _tagName;
        public string TagName
        {
            set { _tagName = value; }
            get { return _tagName; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        private string _updateTime;
        public string UpdateTime
        {
            set 
            {
                if (_updateTime != value)
                {
                    _updateTime = value;
                    RaisePropertyChanged();
                }
            }
            get { return _updateTime; }
        }


        /// <summary>
        /// 当前设置的值
        /// </summary>
        private string _tagValue;
        public string TagValue
        {
            set
            {
                if (_tagValue != value)
                {
                    _tagValue = value;
                    RaisePropertyChanged();
                }
            }
            get { return _tagValue; }
        }
    }
}
