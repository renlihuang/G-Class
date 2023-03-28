using DCS.CORE.Interface;
using DCS.TASK.NET.ViewModel.TaskStatus;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.TaskTags
{
    /// <summary>
    /// 任务状态监控
    /// </summary>
    internal class TaskTagsViewModel:ViewModelBase, IUpdateStatus
    {
        public TaskTagsViewModel()
        {
            TaskTagItems = new ObservableCollection<TaskTagItem>();
            _dicTaskTags = new Dictionary<string, TaskTagItem>();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="msgItems"></param>
        public void SetTaskData(IDictionary<string, MsgItem> msgItems)
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
                        var tagItem = new TaskTagItem()
                        {
                            TagName = item.Key,
                            TagValue = item.Value.Text,
                            UpdateTime = item.Value.CreateTime
                        };
                        TaskTagItems.Add(tagItem);
                        _dicTaskTags[item.Key] = tagItem;
                    }
                    else
                    {
                        var tagItem = _dicTaskTags[item.Key];
                        tagItem.TagValue = item.Value.Text;
                        tagItem.UpdateTime = item.Value.CreateTime;
                    }
                }
            }
        }

        /// <summary>
        /// 当前操作的接口
        /// </summary>
        IDictionary<string, MsgItem> _msgItems;

        /// <summary>
        /// 需要监控的标签
        /// </summary>
        public ObservableCollection<TaskTagItem> TaskTagItems { get; private set; }

        /// <summary>
        /// 标签对应
        /// </summary>
        private Dictionary<string, TaskTagItem> _dicTaskTags;
    }
}
