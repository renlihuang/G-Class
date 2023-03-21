using CS.Base.Password;
using CS.IBLL;
using CS.View.Common;
using CS.View.ViewDlg;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CS.View.ViewModel
{
    /// <summary>
    /// 登录viewModel
    /// </summary>
    internal class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// 用户管理接口
        /// </summary>
        private readonly IUsersService _usersService;

        /// <summary>
        /// 权限管理接口
        /// </summary>
        private readonly IRolesService _rolesService;

        public LoginViewModel()
        {
            //用户管理接口
            _usersService = ServiceProviderInstacnce.GetService<IUsersService>();
            //角色管理接口
            _rolesService = ServiceProviderInstacnce.GetService<IRolesService>();
    
            //关联命令
            MinusWindowCommand = new RelayCommand(MinusWindow);
            CloseWindowCommand = new RelayCommand(CloseWindow);
            //登录命令
            LoginCommand = new RelayCommand(Login);
            //加载用户信息
            LoadUserInfo();
        }

        /// <summary>
        /// 登录
        /// </summary>
        private async void Login()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                HintText = "用户名不能为空!";
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                HintText = "用户密码不能为空!";
                return;
            }

            //设置文本
            HintText = "正在验证用户信息....";
            //获取所有用户task
            var userTask = _usersService.GetAllUsersAync();
            //创建延时Task
            var delayTask = Task.Delay(10000);
            //判断那个Task先执行完
            var waitTask =  await Task.WhenAny(userTask, delayTask);
            //说明调用接口超时
            if (waitTask == delayTask)
            {
                HintText = "调用接口超时,请检查网络";
                return;
            }

            //获取所有用户task
            var users = await userTask;

            //如果没有获取到用户说明接口调用失败了
            if (users.Count == 0)
            {
                HintText = "获取用户信息失败,请检查网络";
                return;
            }

            //得到用户信息
            var userInfo = users.FirstOrDefault(x => x.UserName == UserName);

            if (userInfo == null)
            {
                HintText = "用户名不存在";
                return;
            }
            //解密字符串
            string password = CEncoder.Decode(userInfo.UserPassword);
            //比较密码
            if (password != Password)
            {
                HintText = "密码错误!";
                return;
            }

     

            //保存用户信息
            LoginUserInfo.UserInfo = userInfo;
            //保存角色信息
            LoginUserInfo.RoleInfo = await _rolesService.GetRole(userInfo.UserRoleID);

            //登录成功开始计时
            App.countDownAppcation.Start();

            //更新最后登录时间
            userInfo.LastLoginTime = DateTime.Now;
            await _usersService.UpdateUserAync(userInfo);

            //处理记住密码操作
            if (IsRemember)
            {
                //保存密码
                SaveUserInfo();
            }

            //隐藏对话框
            DialogVisible(false);

            //创建主窗体
            MainViewDialog viewDialog = new MainViewDialog();
            //显示主窗口
            viewDialog.ShowDialog();

            //获取对话框返回值
            string result = viewDialog.GetDialogResult();
            //关闭登录对话框
            Close(result);
        }

        /// <summary>
        /// 从配置文件加载用户信息
        /// </summary>
        private void LoadUserInfo()
        {
            IniFile iniFile = new IniFile("Login.ini");

            string isRemember = iniFile.IniReadValue("UserInfo", "IsRemember");

            if (isRemember != string.Empty &&
                bool.TryParse(isRemember, out bool result))
            {
                //设置按钮状态
                IsRemember = result;

                if (result)
                {
                    //写入配置信息
                    UserName = iniFile.IniReadValue("UserInfo", "UserName");
                    //读取配置文件里的密码
                    string password = iniFile.IniReadValue("UserInfo", "Password");
                    //解密字符串
                    Password = CEncoder.Decode(password);
                }
            }
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        private void SaveUserInfo()
        {
            IniFile iniFile = new IniFile("Login.ini");
            //写入配置信息
            iniFile.IniWriteValue("UserInfo", "UserName", UserName);
            iniFile.IniWriteValue("UserInfo", "Password", CEncoder.Encode(Password));
            iniFile.IniWriteValue("UserInfo", "IsRemember", IsRemember.ToString());
        }

        /// <summary>
        /// 设置当前对话框是否隐藏
        /// </summary>
        /// <param name="isVisible"></param>
        private void DialogVisible(bool isVisible)
        {
            if (!isVisible)
            {
                Messenger.Default.Send<string>("hide", "DialogVisble");
            }
            else
            {
                Messenger.Default.Send<string>("show", "DialogVisble");
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>

        private void MinusWindow()
        {
            Messenger.Default.Send<string>("最小化", "minueDialog");
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>

        private void CloseWindow()
        {
            //退出对话框
            Close("exit");
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="CloseMsg"></param>
        private void Close(string CloseMsg)
        {
            //关闭消息
            Messenger.Default.Send<string>(CloseMsg, "exitDialog");
        }

        /// <summary>
        /// 最小化窗口命令
        /// </summary>
        public RelayCommand LoginCommand { get; private set; }

        /// <summary>
        /// 最小化窗口命令
        /// </summary>
        public RelayCommand MinusWindowCommand { get; private set; }

        /// <summary>
        /// 最小化窗口命令
        /// </summary>
        public RelayCommand CloseWindowCommand { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;

        public string UserName
        {
            set { _userName = value; RaisePropertyChanged(); }
            get { return _userName; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        private string _password;

        public string Password
        {
            set { _password = value; RaisePropertyChanged(); }
            get { return _password; }
        }

        /// 是否记住密码
        /// </summary>
        private bool _isRemember;

        public bool IsRemember
        {
            set { _isRemember = value; RaisePropertyChanged(); }
            get { return _isRemember; }
        }

        /// 提示文本
        /// </summary>
        private string _hintText;

        public string HintText
        {
            set { _hintText = value; RaisePropertyChanged(); }
            get { return _hintText; }
        }
    }
}