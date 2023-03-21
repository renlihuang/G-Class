using CS.View.View.UICoreView;
using CS.View.ViewModel.Base;
using CS.View.ViewModel.UICoreViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CS.View.ViewDlg.UICoreViewDlg
{
    class SanCardDlg: BaseModiaDialog<SanCardLoginWindow>
    {
        /// <summary>
        /// 设置默认的viewModel
        /// </summary>
        public override void BindDefaultViewModel()
        {
            GetWindow().DataContext = new SanCardViewModel();
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <returns></returns>
        public override bool ShowDialog()
        {
            bool? isShow = GetWindow().ShowDialog();

            return isShow.HasValue;
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        public override void RegisterEvenMessage()
        {
            //鼠标移动时拖动窗口
            GetWindow().MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    GetWindow().DragMove();
                }
            };

            //窗体最小化
            Messenger.Default.Register<string>(GetWindow(), "DialogVisble", (string argv) =>
            {
                if (argv == "hide")
                {
                    GetWindow().Hide();
                }
                else if (argv == "show")
                {
                    GetWindow().Show();
                }
            });

            //窗体退出窗口
            Messenger.Default.Register<string>(GetWindow(), "exitDialog", (string argv) =>
            {
                _dialogResult = argv;
                GetWindow().Close();
            });
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        public override void CloseDialog()
        {
            GetWindow().Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private Window GetWindow()
        {
            return GetDialog() as Window;
        }
    }
}
