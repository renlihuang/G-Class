using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CS.View.ViewModel.Base
{
    /// <summary>
    /// 左边菜单组
    /// </summary>
    class MenuGroup : ViewModelBase
    {
        //菜单组标题
        string _groupName;
        //菜单组图标
        PackIconKind _groupIcon;
        //菜单项目
        ObservableCollection<MenuItem> _menuItems = new ObservableCollection<MenuItem>();

        //菜单项目
        public ObservableCollection<MenuItem> MenuItems
        {
            get { return _menuItems; }
        }

        public string GroupName
        {
            set { _groupName = value; RaisePropertyChanged();}
            get { return _groupName;}
        }

        public PackIconKind GroupIcon
        {
            set { _groupIcon = value; RaisePropertyChanged(); }
            get { return _groupIcon; }
        }

    }

    /// <summary>
    /// 左边菜单组
    /// </summary>
    class MenuItem : ViewModelBase
    {

        string _itemName;

        /// <summary>
        ///菜单组标题
        /// </summary>
        public string ItemName
        {
            set { _itemName = value; RaisePropertyChanged(); }
            get { return _itemName; }
        }


        PackIconKind _itemIcon;

        //菜单组图标
        public PackIconKind ItemIcon
        {
            set { _itemIcon = value; RaisePropertyChanged(); }
            get { return _itemIcon; }
        }

        string _className;
        /// <summary>
        /// 关联类名
        /// </summary>
        public string ClassName
        {
            set { _className = value; RaisePropertyChanged(); }
            get { return _className; }
        }
    }
}
