using CS.IBLL;
using CS.Model.Entiry;
using CS.View.Common;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CS.View.ViewModel
{
    /// <summary>
    /// 菜单选择viewModel
    /// </summary>
    internal class SelectMenuViewModel : ViewModelBase
    {
        /// <summary>
        /// 当前操作
        /// </summary>
        private RoleEntiry _model;

        public RoleEntiry Model
        {
            set { _model = value; RaisePropertyChanged(); }
            get { return _model; }
        }

        /// <summary>
        /// 菜单树根节点
        /// </summary>
        public ObservableCollection<SelectMenuTreeItem> MenuTreeItems { get; private set; }

        /// <summary>
        /// 单选按钮命令
        /// </summary>
        public RelayCommand<object> ButtonDownCommand { get; private set; }

        /// <summary>
        ///菜单管理接口
        /// </summary>
        private readonly IMenusService _menusService;

        /// <summary>
        /// 权限管理接口
        /// </summary>
        private readonly IMenuRightsService _menuRightsService;

        /// <summary>
        /// 菜单ID
        /// </summary>
        private List<long> _menuIds;

        /// <summary>
        /// 菜单权限列表
        /// </summary>
        private List<MenuRightsEntity> _menuRightsEntities;

        /// <summary>
        /// 泛型约束编译通过
        /// </summary>
        public SelectMenuViewModel() { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleEntiry"></param>
        public SelectMenuViewModel(RoleEntiry roleEntiry)
        {
            Model = roleEntiry;
            //菜单管理接口
            _menusService = ServiceProviderInstacnce.GetService<IMenusService>();
            //权限管理接口
            _menuRightsService = ServiceProviderInstacnce.GetService<IMenuRightsService>();
            //单选按钮命令
            ButtonDownCommand = new RelayCommand<object>(OnButtonDown);
            //树菜单列表
            MenuTreeItems = new ObservableCollection<SelectMenuTreeItem>();
            //菜单ID
            _menuIds = new List<long>();
            //初始化
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private async void Init()
        {
            //创建根节点
            SelectMenuTreeItem selectMenuTreeItem = new SelectMenuTreeItem() { Children = MenuTreeItems };
            //加载菜单列表
            await LoadMenuIds(Model.Id.Value);
            //加载菜单
            await LoadMenus(selectMenuTreeItem);
            //展开菜单树
            ExpandMenu(selectMenuTreeItem);
        }

        /// <summary>
        ///菜单树单选按钮点击事件
        /// </summary>
        /// <param name="model"></param>
        public async void OnButtonDown(object model)
        {
            //获取当前选中的菜单
            SelectMenuTreeItem selectMenuTreeItem = model as SelectMenuTreeItem;
            //是否选择该菜单节点
            if (selectMenuTreeItem.IsChecked)
            {
                MenuRightsEntity entity = new MenuRightsEntity()
                {
                    RoleID = Model.Id,
                    MenuID = selectMenuTreeItem.ID
                };

                //数据没返回之前设为不选定
                selectMenuTreeItem.IsChecked = false;

                //保存角色ID和菜单ID的对应关系
                long resultID = await _menuRightsService.AddMenuRights(entity);

                if (resultID > 0)
                {
                    selectMenuTreeItem.IsChecked = true;
                    entity.Id = resultID;
                    _menuRightsEntities.Add(entity);
                }
            }
            else
            {
                selectMenuTreeItem.IsChecked = true;
                //查找到ID
                var result = _menuRightsEntities.FirstOrDefault(x => x.MenuID == selectMenuTreeItem.ID);
                if (result != null)
                {
                    //删除角色ID和菜单ID的关系
                    if (await _menuRightsService.DeleteMenuRights(result.Id.Value))
                    {
                        selectMenuTreeItem.IsChecked = false;
                    }
                }
            }
        }

        /// <summary>
        /// 加载菜单ID
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private async Task LoadMenuIds(long roleID)
        {
            //调用接口获取数据
            _menuRightsEntities = await _menuRightsService.GetMenuIds(roleID);

            //循环添加ID
            _menuRightsEntities.ForEach(x =>
            {
                _menuIds.Add(x.MenuID.Value);
            });
        }

        /// <summary>
        /// 加载菜单树，使用非递归的方式
        /// </summary>
        private async Task LoadMenus(SelectMenuTreeItem rootTreeItem)
        {
            List<TreeNodeInfo<SelectMenuTreeItem>> treeNodes = new List<TreeNodeInfo<SelectMenuTreeItem>>();
            //保存根节点
            treeNodes.Add(new TreeNodeInfo<SelectMenuTreeItem>() { ID = 0, MenuTreeItem = rootTreeItem });
            //队列不为空
            while (treeNodes.Count > 0)
            {
                //取出第一个元素
                TreeNodeInfo<SelectMenuTreeItem> treeNodeInfo = treeNodes[0];
                //删除第一个元素
                treeNodes.RemoveAt(0);
                //获取菜单ID下的子菜单
                var menus = await _menusService.GetMenusByParentID(treeNodeInfo.ID);
                //遍历所有的节点
                foreach (var menu in menus)
                {
                    long id = menu.Id.Value;
                    //创建菜单节点
                    SelectMenuTreeItem menuTreeItemData = new SelectMenuTreeItem
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
                        treeNodes.Add(new TreeNodeInfo<SelectMenuTreeItem>() { ID = id, MenuTreeItem = menuTreeItemData });
                    }
                }
            }
        }

        /// <summary>
        /// 展开菜单
        /// </summary>
        /// <param name="rootTreeItem"></param>
        private void ExpandMenu(SelectMenuTreeItem rootTreeItem)
        {
            List<SelectMenuTreeItem> treeItems = new List<SelectMenuTreeItem>();
            //保存根节点
            treeItems.Add(rootTreeItem);
            //不为空就循环遍历
            while (treeItems.Count > 0)
            {
                var menuItem = treeItems[0];
                treeItems.RemoveAt(0);
                //循环子节点
                foreach (var item in menuItem.Children)
                {
                    //检查单选按钮是否要选中
                    item.IsChecked = _menuIds.Contains(item.ID);
                    //有子节点就需要加入队列
                    if (item.Children.Count > 0)
                    {
                        //展开节点
                        item.IsExpand = true;
                        //加入参数
                        treeItems.Add(item);
                    }
                }
            }
        }
    }
}