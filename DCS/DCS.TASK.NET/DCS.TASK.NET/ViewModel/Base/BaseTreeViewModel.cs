using DCS.TASK.NET.Config;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DCS.TASK.NET.ViewModel.Base
{
    /// <summary>
    /// 节点定义
    /// </summary>
    public enum TaskTreeNodeType
    {
        //根目录
        TaskRoot,
        //任务目录
        TaskDicrectory,
        //数据收集任务组
        TaskCollectGroup,
        //定时任务组
        TaskTimerGroup,
        //定时任务
        TaskCollectItem,
        //定时任务
        TaskTimerItem,
    };

    //基本树节点
    public class BaseTreeViewModel : ViewModelBase
    {
        public BaseTreeViewModel()
        {
            //所有子节点
            Children = new ObservableCollection<BaseTreeViewModel>();
        }
        /// <summary>
        /// 名称
        /// </summary>
        string _name;
        public string Name
        {
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }

            get
            {
                return _name;
            }
        }

        /// <summary>
        /// 状态图标
        /// </summary>
        string _statusIcon;
        public string StatusIcon 
        {
            set
            {
                if (_statusIcon != value)
                {
                    _statusIcon = value;
                    RaisePropertyChanged();
                }
            }

            get
            {
                return _statusIcon;
            }
        }

        /// <summary>
        /// 节点图标
        /// </summary>
        string _nodeIcon;
        public string NodeIcon
        {
            set
            {
                if (_nodeIcon != value)
                {
                    _nodeIcon = value;
                    RaisePropertyChanged();
                }
            }

            get
            {
                return _nodeIcon;
            }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        bool _isExpand;
        public bool IsExpand
        {
            set
            {
                if (_isExpand != value)
                {
                    _isExpand = value;
                    RaisePropertyChanged();
                }
            }

            get
            {
                return _isExpand;
            }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        bool _isSelected;
        public bool IsSelected
        {
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged();
                }
            }

            get
            {
                return _isSelected;
            }
        }







        /// <summary>
        /// 设置状态
        /// </summary>
        private bool _status = true;

        public bool Status
        {
            set
            {
                switch (NodeType)
                {
                    case TaskTreeNodeType.TaskCollectGroup:
                    case TaskTreeNodeType.TaskCollectItem:
                    case TaskTreeNodeType.TaskTimerItem:
                        {
                            _status = value;
                            //状态
                            if (_status)
                            {
                                StatusIconColor = "Green";
                                StatusIcon = TaskTreeNodeType.TaskCollectGroup == NodeType ? "Check" : "Play";

                            }
                            else
                            {
                                StatusIconColor = "Red";
                                StatusIcon = TaskTreeNodeType.TaskCollectGroup == NodeType ? "Close" : "Stop";
                            }
                        }
                        break;
                    default:
                        //设置宽度
                        StatusIconWidth = 0;
                        break;
                }

            }
            get { return _status; }
        }

        /// <summary>
        /// 状态图标是否可见
        /// </summary>
        private bool _statusIconVisible;

        public bool StatusIconVisible
        {
            set { _statusIconVisible = value; StatusIconWidth = _statusIconVisible == true ? 30 : 0; }
            get { return _statusIconVisible; }
        }


        /// <summary>
        /// 状态图标宽度
        /// </summary>
        private int _statusIconWidth;
        public int StatusIconWidth
        {
            set { _statusIconWidth = value; }
            get { return _statusIconWidth; }
        }

        /// <summary>
        /// 工具栏是否可见
        /// </summary>
        private bool _toolBarIsVisible;

        public bool ToolBarIsVisible
        {
            set { _toolBarIsVisible = value; RaisePropertyChanged(); }
            get { return _toolBarIsVisible; }
        }

        /// <summary>
        /// 按钮栏
        /// </summary>
        public ObservableCollection<PopupBoxViewModel> ToolButtons { set; get; }


        /// <summary>
        /// 所有子节点
        /// </summary>
        public ObservableCollection<BaseTreeViewModel> Children { set; get; }

        /// <summary>
        /// 状态图标颜色
        /// </summary>
        private string _statusIconColor = "Green";
        public string StatusIconColor
        {
            private set { _statusIconColor = value; RaisePropertyChanged(); }
            get { return _statusIconColor; }
        }

        /// <summary>
        /// 节点图标颜色
        /// </summary>

        private string _nodeIconColor;
        public string NodeIconColor
        {
            set { _nodeIconColor = value; }
            get { return _nodeIconColor; }
        }


        /// <summary>
        /// 节点类型
        /// </summary>
        TaskTreeNodeType _nodeType;
        public TaskTreeNodeType NodeType
        {
            set
            {
                _nodeType = value;

                switch (value)
                {
                    case TaskTreeNodeType.TaskRoot:
                        NodeIconColor = "#4876FF";
                        break;
                    case TaskTreeNodeType.TaskDicrectory:
                        NodeIconColor = "#FFA500";
                        break;
                    case TaskTreeNodeType.TaskTimerGroup:
                        NodeIconColor = "#836FFF";
                        break;
                    case TaskTreeNodeType.TaskCollectGroup:
                        NodeIconColor = "#1C86EE";
                        break;
                    case TaskTreeNodeType.TaskCollectItem:
                    case TaskTreeNodeType.TaskTimerItem:
                        NodeIconColor = "#43CD80";
                        break;
                }
            }
            get { return _nodeType; }
        }

        /// <summary>
        /// 当前的节点的父节点
        /// </summary>
        public BaseTreeViewModel ParentNode { set; get; }

        /// <summary>
        /// 附加参数
        /// </summary>
        public object NodeParam { set; get; }
        /// <summary>
        /// 节点数据
        /// </summary>
        public TaskBaseModel NodeModel { set; get; }
    }
}
