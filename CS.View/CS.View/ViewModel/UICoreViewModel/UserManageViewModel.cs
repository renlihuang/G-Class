using CS.Base.Password;
using CS.IBLL;
using CS.Model.Entiry;
using CS.Model.QueryCondition;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace CS.View.ViewModel
{

    internal class LogoutTimeInfo
    { 
        public string Text { set; get;}

        public int Value { set; get; }
    }

    internal class UserManageViewModel : DataProcess<UserEntiry>
    {
        /// <summary>
        /// 查询文本
        /// </summary>
        private string _queryText = string.Empty;

        public string QueryText
        {
            set { _queryText = value; RaisePropertyChanged(); }
            get { return _queryText; }
        }

        /// <summary>
        /// 下拉框选中值
        /// </summary>
        private RoleEntiry _selectedItem;

        public RoleEntiry SelectedItem
        {
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged();
                }
            }

            get { return _selectedItem; }
        }

        /// <summary>
        /// 超时登录时间
        /// </summary>
        private LogoutTimeInfo _logoutTimeInfo;
        public LogoutTimeInfo LogoutTimeInfo
        {
            set 
            {
                if (_logoutTimeInfo != value)
                {
                    _logoutTimeInfo = value;
                    RaisePropertyChanged();
                }
            }
            get { return _logoutTimeInfo; }
        }

        /// <summary>
        /// 角色选择下拉框
        /// </summary>
        public ObservableCollection<RoleEntiry> RoleCombBoxItems { get; private set; } = new ObservableCollection<RoleEntiry>();

        /// <summary>
        /// 自动退出时间选择对话框
        /// </summary>
        public ObservableCollection<LogoutTimeInfo> logoutTimeInfos { get; private set; } = new ObservableCollection<LogoutTimeInfo>();

        /// <summary>
        /// 用户管理操作接口
        /// </summary>
        private readonly IUsersService _usersService;

        /// <summary>
        ///权限管理接口
        /// </summary>
        private readonly IRolesService _rolesService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="usersService"></param>
        /// <param name="rolesService"></param>
        public UserManageViewModel(IUsersService usersService, IRolesService rolesService)
        {
            _usersService = usersService;
            _rolesService = rolesService;

            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Init()
        {
            base.Init();
            //加载角色
            LoadRoles();

            for (int i = 1; i <= 3; i++)
            {
                logoutTimeInfos.Add(new LogoutTimeInfo 
                {
                    Text = $"{i}分钟后自动注销",
                    Value = i
                 });
            }

        }

        /// <summary>
        /// 加载角色
        /// </summary>
        private async void LoadRoles()
        {
            //加载所有角色
            var roles = await _rolesService.GetRoles();

            RoleCombBoxItems.Add(new RoleEntiry() { RoleName = "请选择用户角色" });
            //添加到列表
            if (roles != null)
            {
                foreach (var item in roles)
                {
                    RoleCombBoxItems.Add(item);
                }
            }
        }

        /// <summary>
        ///添加用户
        /// </summary>
        /// <param name="model"></param>
        protected override void Add(UserEntiry model)
        {
            //设置默认值
            SelectedItem = RoleCombBoxItems[0];

            LogoutTimeInfo = logoutTimeInfos[logoutTimeInfos.Count - 1];

            base.Add(model);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="model"></param>
        protected override void Edit(UserEntiry model)
        {
            //设置下拉框编辑的值
            SelectedItem = RoleCombBoxItems.FirstOrDefault(x => x.Id == model.UserRoleID);
            //解密字符串
            model.UserPassword = CEncoder.Decode(model.UserPassword);
            //注销时间选项
            LogoutTimeInfo = logoutTimeInfos.FirstOrDefault(x => x.Value == model.LogoutTime);

            base.Edit(model);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected override async void GetPageData(int pageInex)
        {
            //查询获取查询结果
            var queryResult = await _usersService.GetUsersAync(pageInex, this.PageSize, new UserQueryCondition() { UserName = QueryText });
            //设置分页数
            this.TotailCount = queryResult.Total;
            //清除之前的显示数据
            DataGridDatas.Clear();
            //
            if (queryResult.Total > 0)
            {
                //循环添加数据
                foreach (var item in queryResult.Data)
                {
                    DataGridDatas.Add(item);
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected override async void Save()
        {
            bool result = false;

            if (string.IsNullOrEmpty(Model.UserName))
            {
                SkinMessageBox.Error("用户名称不能为空");
                return;
            }

            if (string.IsNullOrEmpty(Model.UserPassword))
            {
                SkinMessageBox.Error("用户密码不能为空");
                return;
            }

            if (SelectedItem == RoleCombBoxItems[0])
            {
                SkinMessageBox.Error("请选择角色");
                return;
            }

            //加密字符串
            Model.UserPassword = CEncoder.Encode(Model.UserPassword);
            //
            Model.UserRole = SelectedItem.RoleName;
            Model.UserRoleID = SelectedItem.Id.Value;
            Model.LogoutTime = LogoutTimeInfo.Value;

            if (Mode == ActionMode.Add)
            {
                //添加数据
                result = await _usersService.AddUserAync(Model);

                if (result)
                {
                    base.Save();
                }
            }
            else if (Mode == ActionMode.Edit)
            {
                //更新数据
                result = await _usersService.UpdateUserAync(Model);

                if (result)
                {
                    base.Save();
                }
            }

        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model"></param>
        protected override async void Delete(UserEntiry model)
        {
            bool dialogResult = await SkinMessageBox.Question("确认删除该用户信息");

            if (dialogResult)
            {
                //删除数据
                bool result = await _usersService.DeleteUserAync(model);

                if (result)
                {
                    //删除表格数据
                    base.Delete(model);
                }
            }
        }
    }
}