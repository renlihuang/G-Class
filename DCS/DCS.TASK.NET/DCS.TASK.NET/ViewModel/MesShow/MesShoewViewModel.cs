using DCS.CORE.Interface;
using DCS.TASK.NET.ViewModel.TaskStatus;
using DCS.TASK.NET.ViewModel.TaskTags;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.MesShow
{
    internal class MesShoewViewModel:ViewModelBase, IUpdateStatus
    {
        public MesShoewViewModel()
        {
            TaskTagItems = new ObservableCollection<MesTagItem>();
            _dicTaskTags = new Dictionary<string, MesTagItem>();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="msgItems"></param>
        public void SetTaskData(IDictionary<string, MesItem> msgItems)
        {
            _msgItems = msgItems;
            //重新加载数据
            ReloadData();
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public void ReloadData()
        {
            TaskTagItems.Clear();
            _dicTaskTags.Clear();

            if (_msgItems != null)
            {
                //更新状态状态
                UpdateData();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void UpdateData()
        {
            if (_msgItems != null)
            {
                foreach (var item in _msgItems)
                {
                    if (!_dicTaskTags.ContainsKey(item.Key))
                    {
                        var tagItem = new MesTagItem()
                        {
                            UpdateTime = item.Value.CreateTime
                        };
                        TaskTagItems.Add(tagItem);
                        _dicTaskTags[item.Key] = tagItem;
                    }
                    else
                    {
                        var tagItem = _dicTaskTags[item.Key];
                        //tagItem.TagValue = item.Value.Text;
                        tagItem.UpdateTime = item.Value.CreateTime;
                    }
                }
            }
        }

        /// <summary>
        /// 当前操作的接口
        /// </summary>
        IDictionary<string, MesItem> _msgItems;

        /// <summary>
        /// 需要监控的标签
        /// </summary>
        public ObservableCollection<MesTagItem> TaskTagItems { get; private set; }

        /// <summary>
        /// 标签对应
        /// </summary>
        private Dictionary<string, MesTagItem> _dicTaskTags;
    }
}
