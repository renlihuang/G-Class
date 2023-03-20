using DCS.TASK.NET.ViewModel.Base;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DCS.TASK.NET.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCS.TaskManage.FrameWork;

namespace DCS.TASK.NET.ViewModel.TaskStatus
{
    /// <summary>
    /// 所有任务状态viewModel
    /// </summary>
    internal class AllTaskStatusViewModel:ViewModelBase, IUpdateStatus
    {

        public AllTaskStatusViewModel(BaseTreeViewModel baseTree)
        {
            //创建列表
            TaskStatusItems = new ObservableCollection<TaskStatusItemViewModel>();
            //创建树节点
            _dicTreeItems = new Dictionary<BaseTreeViewModel, TaskStatusItemViewModel>();
            //保存节点
            _baseTree = baseTree;
        }

   

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public void ReloadData()
        {
            //查找树的根节点
            var taskTrees = _baseTree.FindTaskItems();
            //清空已经加载数据
            TaskStatusItems.Clear();
            _dicTreeItems.Clear();
            //重新加载数据
            foreach (var item in taskTrees)
            {
               var taskStatusItem = new  TaskStatusItemViewModel();
                _dicTreeItems[item] = taskStatusItem;
                TaskStatusItems.Add(taskStatusItem);
            }
            //加载完成之后更新一下数据
            UpdateData();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateData()
        {
            foreach (var item in _dicTreeItems)
            {
                var treeNode = item.Key;
                var statusItem = item.Value;
                //得到任务信息
                var taskItem  = treeNode.NodeParam as CollectTaskItem;
                //填充信息
                if (taskItem != null)
                {
                    //获取消息文本
                    var textItem = taskItem.TaskMsg.GetText();
                    //获取任务信息
                    var taskInfo = taskItem.GetCollectTaskInfo();
                    //设置信息
                    statusItem.TaskName = taskInfo.TaskName;
                    //任务运行
                    statusItem.Status = taskInfo.IsRunming;
                    //是否正在运行
                    if (taskInfo.IsRunming)
                    {
                        //获取任务运行时间
                        statusItem.TaskMsg = textItem.Text;
                        statusItem.UpdateTime = textItem.CreateTime;
                        statusItem.TaskRuntime = taskItem.TaskRuntime; ;
                    }
                }
            }
        }

        /// <summary>
        /// 保存任务树的根节点
        /// </summary>
        private BaseTreeViewModel _baseTree;

        /// <summary>
        /// 保存书节点对应的
        /// </summary>
        Dictionary<BaseTreeViewModel, TaskStatusItemViewModel> _dicTreeItems;
        /// <summary>
        /// 任务状态
        /// </summary>
        public ObservableCollection<TaskStatusItemViewModel> TaskStatusItems { private set; get; } = new ObservableCollection<TaskStatusItemViewModel>();
    }
}
