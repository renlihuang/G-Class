using CS.Base.Password;
using CS.IBLL;
using CS.View.Common;
using CS.View.ViewDlg;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.UICoreViewModel
{
    class SanCardViewModel : ViewModelBase
    {
        public SanCardViewModel()
        {
            //用户管理接口
            _usersService = ServiceProviderInstacnce.GetService<IUsersService>();
            //角色管理接口
            _rolesService = ServiceProviderInstacnce.GetService<IRolesService>();
            //关联命令
            EnterKeyDownCommand = new RelayCommand(EnterKeyDown);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        /// <summary>
        /// 回车事件
        /// </summary>
        private async void EnterKeyDown()
        {
            if (!string.IsNullOrEmpty(ScanText))
            {
                string Password = CEncoder.Encode(ScanText); ;
                ScanText = string.Empty;
                //设置提示文本
                HintText = "正在验证登录....";
                //加载用户
                var users = await _usersService.GetAllUsersAync();
                //用户大于0
                if (users.Count > 0)
                {
                    //查找用户
                    var user = users.FirstOrDefault(x => x.UserPassword == Password);
                    //用户不等于空
                    if (user != null)
                    {
                        //保存用户信息
                        LoginUserInfo.UserInfo = user;
                        //保存角色信息
                        LoginUserInfo.RoleInfo = await _rolesService.GetRole(user.UserRoleID);

                        //登录成功开始计时
                        App.countDownAppcation.Start();

                        //更新最后登录时间
                        user.LastLoginTime = DateTime.Now;
                        await _usersService.UpdateUserAync(user);
                        //隐藏对话框
                        DialogVisible(false);

                        //创建主窗体
                        MainViewDialog viewDialog = new MainViewDialog();
                        //显示主窗口
                        viewDialog.ShowDialog();

                        //获取对话框返回值
                        string result = viewDialog.GetDialogResult();
                    }
                    else
                    {
                        //设置提示文本
                        HintText = "未找到用户，请确认该卡号是否录入";
                    }
                }
                else
                {
                    //设置提示文本
                    HintText = "连接服务器超时请检查网络";
                }
            }
            else
            {
                HintText = "刷卡字符不能为空";
            }
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
        /// 关闭窗口
        /// </summary>
        private void CloseWindow()
        {
            //关闭消息
            Messenger.Default.Send<string>("", "exitDialog");
        }

        /// <summary>
        /// 扫描到的条码
        /// </summary>
        private string _sanText;
        public string ScanText
        {
            set 
            {
                if (_sanText != value)
                {
                    _sanText = value;
                    RaisePropertyChanged();
                }
            }
            get { return _sanText; }
        }

        /// <summary>
        /// 用户管理接口
        /// </summary>
        private readonly IUsersService _usersService;

        /// <summary>
        /// 权限管理接口
        /// </summary>
        private readonly IRolesService _rolesService;

        /// <summary>
        /// 扫描到的条码
        /// </summary>
        private string _hintText;
        public string HintText
        {
            set { _hintText = value; RaisePropertyChanged();}
            get { return _hintText; }
        }

        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public RelayCommand EnterKeyDownCommand { get; private set; }

        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public RelayCommand CloseWindowCommand { get; private set; }
    }
}
