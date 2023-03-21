using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace CS.View.ViewModel.Base
{
    public class MenuTreeItemBase : ViewModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        ///  菜单名称
        /// </summary>
        private string _menuName;

        public string MenuName
        {
            set { _menuName = value; RaisePropertyChanged(); }
            get { return _menuName; }
        }

        /// <summary>
        ///  菜单图标
        /// </summary>
        private string _menuIcon = string.Empty;

        public string MenuIcon
        {
            set { _menuIcon = value; RaisePropertyChanged(); }
            get { return _menuIcon; }
        }

        /// <summary>
        ///  菜单实例
        /// </summary>
        private string _menuInstance = string.Empty;

        public string MenuInstance
        {
            set { _menuInstance = value; RaisePropertyChanged(); }
            get { return _menuInstance; }
        }

        /// <summary>
        ///  父菜单ID
        /// </summary>
        private long _parentID;

        public long ParentID
        {
            set { _parentID = value; RaisePropertyChanged(); }
            get { return _parentID; }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        private bool _isExpand;

        public bool IsExpand
        {
            set
            {
                if (_isExpand != value)
                {
                    _isExpand = value; RaisePropertyChanged();
                }
            }
            get { return _isExpand; }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        private bool _isSelected;

        public bool IsSelected
        {
            set { _isSelected = value; RaisePropertyChanged(); }
            get { return _isSelected; }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class MenuTreeItemData : MenuTreeItemBase
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public MenuTreeItemData ParentNode { set; get; }

        /// <summary>
        /// 当前菜单子节点
        /// </summary>
        ///
        private ObservableCollection<MenuTreeItemData> _children;

        public ObservableCollection<MenuTreeItemData> Children
        {
            set { _children = value; }
            get
            {
                if (_children == null)
                {
                    _children = new ObservableCollection<MenuTreeItemData>();
                }
                return _children;
            }
        }
    }

    /// <summary>
    /// 增加单选
    /// </summary>
    internal class SelectMenuTreeItem : MenuTreeItemBase
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public SelectMenuTreeItem ParentNode { set; get; }

        /// <summary>
        /// 当前菜单子节点
        /// </summary>
        ///
        private ObservableCollection<SelectMenuTreeItem> _children;

        public ObservableCollection<SelectMenuTreeItem> Children
        {
            set { _children = value; }
            get
            {
                if (_children == null)
                {
                    _children = new ObservableCollection<SelectMenuTreeItem>();
                }
                return _children;
            }
        }

        //单选按钮
        private bool _isChecked;

        public bool IsChecked
        {
            set { _isChecked = value; RaisePropertyChanged(); }
            get { return _isChecked; }
        }
    }
}