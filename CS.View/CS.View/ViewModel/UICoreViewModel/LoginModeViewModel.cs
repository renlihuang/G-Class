using CS.View.ViewDlg;
using CS.View.ViewDlg.UICoreViewDlg;
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
    /// <summary>
    /// 选中登录模式
    /// </summary>
    class LoginModeViewModel : ViewModelBase
    {
        public LoginModeViewModel()
        {
            //设置命令
            PasswordLoginCommand = new RelayCommand(PasswordLogin);
            ScanCardLoginCommand = new RelayCommand(ScanCardLogin);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void CloseWindow()
        {
            _closeMsg = "close";
            //关闭消息
            Messenger.Default.Send<string>(_closeMsg, "exitDialog");
        }

        /// <summary>
        /// 扫码登录
        /// </summary>
        private void ScanCardLogin()
        {
            DialogVisible(false);

            SanCardDlg sanCardDlg = new SanCardDlg();
            sanCardDlg.ShowDialog();

            _closeMsg = "SanCardMode";
            //关闭消息
            Messenger.Default.Send<string>(_closeMsg, "exitDialog");
        }

        /// <summary>
        /// 密码登录
        /// </summary>
        private void PasswordLogin()
        {
            DialogVisible(false);
            //创建对话框
            LoginDlg loginDlg = new LoginDlg();
            //显示登录窗口
            loginDlg.ShowDialog();

            _closeMsg = "PasswordLogin";
            //关闭消息
            Messenger.Default.Send<string>(_closeMsg, "exitDialog");
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

        private string _closeMsg = string.Empty;
        /// <summary>
        /// 密码登录
        /// </summary>
        public RelayCommand PasswordLoginCommand { get; private set; }

        /// <summary>
        /// 刷卡登录
        /// </summary>
        public RelayCommand ScanCardLoginCommand { get; private set; }

        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public RelayCommand CloseWindowCommand { get; private set; }
    }
}
