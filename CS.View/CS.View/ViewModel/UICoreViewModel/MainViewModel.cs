using CS.IBLL;
using CS.View.Common;
using CS.View.View;
using CS.View.ViewModel.Base;
using CS.View.ViewModel.Interface;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CS.View.ViewModel
{
    /// <summary>
    /// 主窗体ViewModel
    /// </summary>
    internal partial class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            //菜单管理接口
            _menusService = ServiceProviderInstacnce.GetService<IMenusService>();
            //权限管理接口
            _menuRightsService = ServiceProviderInstacnce.GetService<IMenuRightsService>();
            //初始化
            Init();
            //加载首页
            LoadDefaultPage();
        }

        /// <summary>
        /// 初始化资源
        /// </summary>
        public async void Init()
        {
            //加载程序集
            await _dialogManage.LoadViews();

            var userInfo = LoginUserInfo.UserInfo;
            //用户名
            UserNameText = userInfo.UserName;
            //角色名
            RoleNameText = $"当前角色:{userInfo.UserRole}";
            //创建根节点
            var menuTreeRoot = new MenuTreeItemData() { Children = MenuTreeItems };
            //加载菜单ID
            await LoadMenuIds();
            //加载菜单树
            LoadMenus(menuTreeRoot);
      
        }

        /// <summary>
        /// 加载菜单ID列表
        /// </summary>
        private async Task LoadMenuIds()
        {
            //根据角色ID
            var menuIds = await _menuRightsService.GetMenuIds(LoginUserInfo.RoleInfo.Id.Value);
            //清空list
            _menuIds.Clear();
            //循环添加所有菜单ID
            menuIds.ForEach(x =>
            {
                _menuIds.Add(x.MenuID.Value);
            });
        }

        /// <summary>
        /// 检测菜单ID是否拥有ID
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        private bool ModifyMenuID(long menuID)
        {
            //如果是管理员默认拥有所有菜单的权限
            if (LoginUserInfo.RoleInfo.IsManage == 1)
            {
                return true;
            }
            else
            {
                //判断菜单ID是在ID列表中
                return _menuIds.Contains(menuID);
            }
        }

        /// <summary>
        /// 加载菜单
        /// </summary>
        public async void LoadMenus(MenuTreeItemData rootTreeItem)
        {
            List<TreeNodeInfo<MenuTreeItemData>> treeNodes = new List<TreeNodeInfo<MenuTreeItemData>>();
            //保存根节点
            treeNodes.Add(new TreeNodeInfo<MenuTreeItemData>()
            {
                ID = rootTreeItem.ID,
                MenuTreeItem = rootTreeItem
            });
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
                    //菜单ID
                    long id = menu.Id.Value;

                    if (ModifyMenuID(id))
                    {
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
        }

        /// <summary>
        /// 树菜单选择完成
        /// </summary>
        /// <param name="item"></param>
        private void SelectedItem(object item)
        {
            if (item != null)
            {
                MenuTreeItemData menuTreeItemData = item as MenuTreeItemData;
                //执行菜单切换
                Execute(menuTreeItemData);
            }
        }

        /// <summary>
        /// 执行菜单切换
        /// </summary>
        /// <param name="menuItem"></param>
        private void Execute(MenuTreeItemData menuItem)
        {
            //菜单名称
            string menuName = menuItem.MenuName;
            //实现类名
            string className = menuItem.MenuInstance;

            //判断是否已经打开
            TabPageInfo tabPageInfo = _openTabPages.FirstOrDefault(x => x.PageText == menuName);

            if (tabPageInfo == null)
            {
                if (!string.IsNullOrEmpty(className))
                {
                    try
                    {
                        //创建对话框
                        IBaseModel baseModel = _dialogManage.CreateDialog(className);

                        if (baseModel != null)
                        {
                            if (baseModel != null)
                            {
                                //绑定默认的viewModel
                                baseModel.BindDefaultViewModel();
                                //打开新页面
                                TabPageInfo newPage = new TabPageInfo() { PageText = menuName, PageBody = baseModel.GetView() };
                                //添加到tab页
                                _openTabPages.Add(newPage);
                                //设置为前台打开页面
                                CurrentOpenPage = _openTabPages[_openTabPages.Count - 1];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //显示错误信息
                        SkinMessageBox.Error(ex.Message);
                    }
                }
            }
            else
            {
                //切换到当前打开页面
                if (CurrentOpenPage != tabPageInfo)
                {
                    CurrentOpenPage = tabPageInfo;
                }
            }
        }

        /// <summary>
        /// 关闭tab标签
        /// </summary>
        /// <param name="tabPageInfo"></param>
        private void CloseTabPage(TabPageInfo tabPageInfo)
        {
            //判断是否已经打开
            if (_defaultPageName != tabPageInfo.PageText &&
                _openTabPages.FirstOrDefault(x => x.PageText == tabPageInfo.PageText) != null)
            {
                //关闭页面
                _openTabPages.Remove(tabPageInfo);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void LoadDefaultPage()
        {
            TabPageInfo homPage = new TabPageInfo()
            {
                PageText = _defaultPageName,
                PageBody = new HomPage(),
                CloseButtonVisible = true
            };
            _openTabPages.Add(homPage);
            //设置当前页
            CurrentOpenPage = homPage;
        }
    }

    internal partial class MainViewModel
    {
        /// <summary>
        /// 管理菜单接口
        /// </summary>
        private IMenusService _menusService;

        /// <summary>
        /// 菜单管理接口
        /// </summary>
        private IMenuRightsService _menuRightsService;

        /// <summary>
        /// 菜单ID列表
        /// </summary>
        private List<long> _menuIds = new List<long>();

        /// <summary>
        /// 对话框管理
        /// </summary>
        private DialogManage _dialogManage = new DialogManage();

        /// <summary>
        /// 显示用户信息文本
        /// </summary>
        private string _userNameText;

        public string UserNameText
        {
            set { _userNameText = value; RaisePropertyChanged(); }
            get { return _userNameText; }
        }

        /// <summary>
        /// 显示用户信息文本
        /// </summary>
        private string _roleNameText;

        public string RoleNameText
        {
            set { _roleNameText = value; RaisePropertyChanged(); }
            get { return _roleNameText; }
        }

        /// <summary>
        /// 菜单树选择命令
        /// </summary>
        private RelayCommand<object> _selectedItemCommand;

        public RelayCommand<object> SelectedItemCommand
        {
            get
            {
                if (_selectedItemCommand == null)
                {
                    _selectedItemCommand = new RelayCommand<object>(SelectedItem);
                }
                return _selectedItemCommand;
            }
        }

        /// <summary>
        /// 关闭页面命令
        /// </summary>
        private RelayCommand<TabPageInfo> _closePageCommand;

        public RelayCommand<TabPageInfo> ClosePageCommand
        {
            get
            {
                if (_closePageCommand == null)
                {
                    _closePageCommand = new RelayCommand<TabPageInfo>(CloseTabPage);
                }

                return _closePageCommand;
            }
        }

        /// <summary>
        /// 菜单树根节点
        /// </summary>
        public ObservableCollection<MenuTreeItemData> MenuTreeItems { get; } = new ObservableCollection<MenuTreeItemData>();

        /// <summary>
        /// 弹出菜单
        /// </summary>
        public MainPopupBoxViewModel MainPopupBox { get; private set; } = new MainPopupBoxViewModel();

        /// <summary>
        /// 窗体左上角工具栏，关闭按钮，最小~最大化按钮
        /// </summary>
        public MainToolBarViewModel MainToolBar { get; private set; } = new MainToolBarViewModel();

        /// <summary>
        /// 已经打开的tab页面
        /// </summary>

        private ObservableCollection<TabPageInfo> _openTabPages = new ObservableCollection<TabPageInfo>();

        public ObservableCollection<TabPageInfo> OpenTabPages
        {
            get { return _openTabPages; }
        }

        /// <summary>
        /// 当前打开页面
        /// </summary>
        private object _currentOpenPage;

        public object CurrentOpenPage
        {
            set { _currentOpenPage = value; RaisePropertyChanged(); }
            get { return _currentOpenPage; }
        }

        /// <summary>
        /// 默认主页名称
        /// </summary>
        private string _defaultPageName = "首页";
    }
}