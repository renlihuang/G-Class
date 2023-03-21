using CS.IBLL;
using CS.Model.Entiry;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.FrameControl;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CS.View.ViewModel
{
    /// <summary>
    /// 菜单树节点信息
    /// </summary>
    internal class TreeNodeInfo<T>
    {
        public long ID { set; get; }

        public T MenuTreeItem { set; get; }
    }

    /// <summary>
    /// 菜单管理viewModel
    /// </summary>
    internal partial class MenuManageViewModel : ViewModelBase
    {
        /// <summary>
        /// 菜单访问接口
        /// </summary>
        private readonly IMenusService _menusService;

        /// <summary>
        ///
        /// </summary>
        public MenuManageViewModel(IMenusService menusService)
        {
            _menusService = menusService;
            //初始化
            Init();
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public async void Init()
        {
            //父节点名称不可见
            ParentMenuNameIsVisible = false;
            //按钮不可见
            ButtonIsVisible = false;
            //设置为只读
            TextBoxIsReadonly = true;
            //添加命令
            SelectedItemChangeCommand = new RelayCommand<object>(SelectedItemChange);
            //添加根节点
            MenuTreeItems.Add(_selectedMenuTreeItem = new MenuTreeItemData { MenuName = "菜单树根节点", MenuIcon = "Register" });

            //初始化工具栏
            ToolBarButtons.Add(new BaseButtonBar() { Text = "添加", Icon = "Plus", Command = new RelayCommand(AddMenu) });
            ToolBarButtons.Add(new BaseButtonBar() { Text = "编辑", Icon = "Edit", Command = new RelayCommand(EditMenu) });
            ToolBarButtons.Add(new BaseButtonBar() { Text = "删除", Icon = "BookmarkRemove", Command = new RelayCommand(DeleteMenu) });

            //关联确认命令
            ConfirmCommand = new RelayCommand(Confirm);
            //取消命令
            CancelCommand = new RelayCommand(Cancel);
            //选择图标命令
            SelectImageCommand = new RelayCommand(SelectImage);
            //加载菜单
            await LoadMenus(MenuTreeItems[0]);
            //展开菜单树
            ExpandMenuTree(MenuTreeItems[0]);
        }

        /// <summary>
        /// 展开菜单树，使用非递归的方式
        /// </summary>
        /// <param name="rootTreeItem"></param>
        public void ExpandMenuTree(MenuTreeItemData rootTreeItem)
        {
            List<MenuTreeItemData> treeNodes = new List<MenuTreeItemData>();
            //展开根节点
            if (rootTreeItem.Children.Count > 0)
            {
                rootTreeItem.IsExpand = true;
            }

            //保存根节点
            treeNodes.Add(rootTreeItem);
            //队列不为空
            while (treeNodes.Count > 0)
            {
                //取出第一个元素
                MenuTreeItemData itemData = treeNodes[0];
                //删除第一个元素
                treeNodes.RemoveAt(0);
                //
                foreach (var item in itemData.Children)
                {
                    if (item.Children.Count > 0)
                    {
                        treeNodes.Add(item);
                        //展开节点
                        item.IsExpand = true;
                    }
                }
            }
        }

        /// <summary>
        /// 加载菜单树，使用非递归的方式
        /// </summary>
        public async Task LoadMenus(MenuTreeItemData rootTreeItem)
        {
            List<TreeNodeInfo<MenuTreeItemData>> treeNodes = new List<TreeNodeInfo<MenuTreeItemData>>();
            //保存根节点
            treeNodes.Add(new TreeNodeInfo<MenuTreeItemData>() { ID = 0, MenuTreeItem = rootTreeItem });
            //队列不为空
            while (treeNodes.Count > 0)
            {
                //取出第一个元素
                TreeNodeInfo<MenuTreeItemData> treeNodeInfo = treeNodes[0];
                //删除第一个元素
                treeNodes.RemoveAt(0);
                //获取菜单ID下的子菜单
                var menus = await _menusService.GetMenusByParentID(treeNodeInfo.ID);
                //遍历所有的节点
                foreach (var menu in menus)
                {
                    long id = menu.Id.Value;
                    //创建菜单节点
                    MenuTreeItemData menuTreeItemData = new MenuTreeItemData
                    {
                        ID = id,
                        MenuName = menu.MenuName,
                        MenuIcon = menu.MenuIcon,
                        MenuInstance = menu.MenuInstance,
                        ParentID = treeNodeInfo.ID,
                        ParentNode = treeNodeInfo.MenuTreeItem
                    };
                    //添加到父菜单
                    treeNodeInfo.MenuTreeItem.Children.Add(menuTreeItemData);
                    //判断当前ID是否有子节点
                    if (await _menusService.HasChildren(id))
                    {
                        //添加到父节点
                        treeNodes.Add(new TreeNodeInfo<MenuTreeItemData>() { ID = id, MenuTreeItem = menuTreeItemData });
                    }
                }
            }
        }

        /// <summary>
        /// 确认确认按钮
        /// </summary>
        public async void Confirm()
        {
            if (string.IsNullOrEmpty(MenuItemData.MenuName))
            {
                SkinMessageBox.Error("菜单名称不能空");
                return;
            }

            if (string.IsNullOrEmpty(MenuItemData.MenuIcon))
            {
                SkinMessageBox.Error("请选择菜单图片");
                return;
            }

            //添加节点
            if (_actionMode == ActionMode.Add)
            {
                //添加到数据库
                if (await OnAddMenuAysnc(MenuItemData))
                {
                    //添加到父菜单
                    _selectedMenuTreeItem.Children.Add(MenuItemData);
                    //展开节点
                    if (!_selectedMenuTreeItem.IsExpand)
                    {
                        _selectedMenuTreeItem.IsExpand = true;
                    }
                }
            }
            else if (_actionMode == ActionMode.Edit)
            {
                //更新到数据库
                if (await OnEditMenu(MenuItemData))
                {
                    _selectedMenuTreeItem.MenuName = MenuItemData.MenuName;
                    _selectedMenuTreeItem.MenuIcon = MenuItemData.MenuIcon;
                    _selectedMenuTreeItem.MenuInstance = MenuItemData.MenuInstance;
                }
            }

            ParentMenuNameIsVisible = false;
            //按钮不可见
            ButtonIsVisible = false;
            //设置为只读
            TextBoxIsReadonly = true;
        }

        /// <summary>
        /// 添加菜单数据到数据库
        /// </summary>
        /// <param name="menuTreeItemData"></param>
        private async Task<bool> OnAddMenuAysnc(MenuTreeItemData menuTreeItemData)
        {
            bool result = false;
            //填充模型
            MenuEntiry menuEntiry = new MenuEntiry()
            {
                MenuName = menuTreeItemData.MenuName,
                MenuIcon = menuTreeItemData.MenuIcon,
                ParentID = menuTreeItemData.ParentID,
                MenuInstance = menuTreeItemData.MenuInstance,
            };

            //调用接口添加到数据
            long id = await _menusService.AddMenuAsync(menuEntiry);

            if (id > 0)
            {
                menuTreeItemData.ID = id;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuTreeItemData"></param>
        /// <returns></returns>
        private async Task<bool> OnDeleteMenuAysnc(MenuTreeItemData menuTreeItemData)
        {
            return await _menusService.DeleteMenuAsync(menuTreeItemData.ID);
        }

        /// <summary>
        /// 添加菜单数据到数据库
        /// </summary>
        /// <param name="menuTreeItemData"></param>
        private async Task<bool> OnEditMenu(MenuTreeItemData menuTreeItemData)
        {
            bool result = false;

            long id = menuTreeItemData.ID;

            //填充模型
            MenuEntiry menuEntiry = new MenuEntiry()
            {
                MenuName = menuTreeItemData.MenuName,
                MenuIcon = menuTreeItemData.MenuIcon,
                ParentID = menuTreeItemData.ParentID,
                MenuInstance = menuTreeItemData.MenuInstance,
            };
            //更新菜单信息
            result = await _menusService.UpdateMenuAsync(id, menuEntiry);

            return result;
        }

        /// <summary>
        /// 取消添加
        /// </summary>
        public void Cancel()
        {
            ParentMenuNameIsVisible = false;
            //当前选中模型
            MenuItemData = _selectedMenuTreeItem;
            //按钮不可见
            ButtonIsVisible = false;
            //设置为只读
            TextBoxIsReadonly = true;
        }

        /// <summary>
        /// 选择图标
        /// </summary>
        public async void SelectImage()
        {
            BaseMsgDialog imageDialog = new BaseMsgDialog();

            SelectImageViewModel selectImageViewModel = new SelectImageViewModel();
            //绑定
            imageDialog.BindDataContex(new SelectImageWindow(), selectImageViewModel);
            //显示图片选择对话框
            await imageDialog.ShowDialog();
            //保存图片地址
            if (!string.IsNullOrEmpty(selectImageViewModel.SelectedImageName))
            {
                MenuItemData.MenuIcon = selectImageViewModel.SelectedImageName;
            }
        }

        /// <summary>
        ///树节点选择
        /// </summary>
        /// <param name="newItem"></param>
        public void SelectedItemChange(object newItem)
        {
            if (newItem != null)
            {
                _selectedMenuTreeItem = newItem as MenuTreeItemData;
            }

            //当前选中模型
            MenuItemData = _selectedMenuTreeItem;
            //父节点不可见
            ParentMenuNameIsVisible = false;
            //按钮不可加
            ButtonIsVisible = false;
            //编辑框只读
            TextBoxIsReadonly = true;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        public void AddMenu()
        {
            if (_selectedMenuTreeItem == null)
            {
                SkinMessageBox.Error("请选择要添加菜单的上级");
                return;
            }

            //父节点名称可见
            ParentMenuNameIsVisible = true;
            //按钮可见
            ButtonIsVisible = true;
            //编辑框不只读
            TextBoxIsReadonly = false;

            MenuItemData = new MenuTreeItemData()
            {
                ParentNode = _selectedMenuTreeItem,
                ParentID = _selectedMenuTreeItem.ID
            };

            //父节点名称
            ParentNodeName = _selectedMenuTreeItem.MenuName;
            //添加模式
            _actionMode = ActionMode.Add;
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        public void EditMenu()
        {
            //不能编辑根节点
            if (_selectedMenuTreeItem == null ||  MenuTreeItems[0] == _selectedMenuTreeItem)
            {
                return;
            }
            //添加模式
            _actionMode = ActionMode.Edit;

            //创建编辑数据
            MenuItemData = new MenuTreeItemData()
            {
                ID = _selectedMenuTreeItem.ID,
                MenuName = _selectedMenuTreeItem.MenuName,
                MenuIcon = _selectedMenuTreeItem.MenuIcon,
                MenuInstance = _selectedMenuTreeItem.MenuInstance,
                ParentID = _selectedMenuTreeItem.ParentID
            };

            //父节点名称
            ParentNodeName = _selectedMenuTreeItem.MenuName;
            //按钮可见
            ButtonIsVisible = true;
            //取消只读
            TextBoxIsReadonly = false;
            //父节点不可见
            ParentMenuNameIsVisible = false;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        public async void DeleteMenu()
        {
            //不能删除根节点
            if (_selectedMenuTreeItem == null || MenuTreeItems[0] == _selectedMenuTreeItem)
            {
                return;
            }

            if (_selectedMenuTreeItem.Children.Count > 0)
            {
                SkinMessageBox.Error("请删该节点的所有子节点以后再删除该节点");
                return;
            }

            //获取获父节点
            MenuTreeItemData parentNode = _selectedMenuTreeItem.ParentNode;

            if (parentNode != null)
            {
                bool dialogResult = await SkinMessageBox.Question("确认删除菜单信息");
                //对话框返回值
                if (dialogResult)
                {
                    //删除数据库信息
                    if (await OnDeleteMenuAysnc(_selectedMenuTreeItem))
                    {
                        var children = parentNode.Children;
                        //删除对应的菜单
                        if (children.Contains(_selectedMenuTreeItem))
                        {
                            children.Remove(_selectedMenuTreeItem);
                            //设置删除以后为空
                            MenuItemData = _selectedMenuTreeItem = null;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 属性这些玩意儿放在这里声明
    /// </summary>
    internal partial class MenuManageViewModel
    {
        /// <summary>
        /// 工具栏按钮
        /// </summary>
        public ObservableCollection<BaseButtonBar> ToolBarButtons { get; } = new ObservableCollection<BaseButtonBar>();

        /// <summary>
        /// 菜单树根节点
        /// </summary>
        public ObservableCollection<MenuTreeItemData> MenuTreeItems { get; } = new ObservableCollection<MenuTreeItemData>();

        /// <summary>
        /// 菜单树选择改变命令
        /// </summary>
        public RelayCommand<object> SelectedItemChangeCommand { get; private set; }

        /// <summary>
        /// 确认按钮
        /// </summary>
        public RelayCommand ConfirmCommand { get; private set; }

        /// <summary>
        /// 取消按钮
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public RelayCommand SelectImageCommand { get; private set; }

        /// <summary>
        /// 当前选中菜单
        /// </summary>
        private MenuTreeItemData _selectedMenuTreeItem;

        /// <summary>
        ///操作类型
        /// </summary>
        private ActionMode _actionMode;

        /// <summary>
        ///当前操作菜单模型
        /// </summary>
        private MenuTreeItemData _menuItemData;

        public MenuTreeItemData MenuItemData
        {
            set
            {
                if (_menuItemData != value)
                {
                    _menuItemData = value;
                    RaisePropertyChanged();
                }
            }
            get { return _menuItemData; }
        }

        /// <summary>
        /// 父菜单可可见性
        /// </summary>
        private bool _parentMenuNameIsVisible;

        public bool ParentMenuNameIsVisible
        {
            set
            {
                if (_parentMenuNameIsVisible != !value)
                {
                    _parentMenuNameIsVisible = !value;
                    RaisePropertyChanged();
                }
            }
            get { return _parentMenuNameIsVisible; }
        }

        /// <summary>
        /// 按钮是否可见
        /// </summary>
        private bool _buttonIsVisible;

        public bool ButtonIsVisible
        {
            set
            {
                if (_buttonIsVisible != !value)
                {
                    _buttonIsVisible = !value;
                    RaisePropertyChanged();
                }
            }
            get { return _buttonIsVisible; }
        }

        /// <summary>
        /// 父菜单名称
        /// </summary>
        private string _parentNodeName;

        public string ParentNodeName
        {
            set { _parentNodeName = value; RaisePropertyChanged(); }
            get { return _parentNodeName; }
        }

        /// <summary>
        ///编辑框是否禁用
        /// </summary>
        private bool _textBoxIsReadonly;

        public bool TextBoxIsReadonly
        {
            set
            {
                if (_textBoxIsReadonly != value)
                {
                    _textBoxIsReadonly = value;
                    RaisePropertyChanged();
                }
            }
            get { return _textBoxIsReadonly; }
        }
    }
}