using CS.View.ViewModel;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;

namespace CS.View.ViewDlg
{
    internal class MainViewDialog : BaseModiaDialog<MainWindow>
    {
        /// <summary>
        /// 设置默认的viewModel
        /// </summary>
        public override void BindDefaultViewModel()
        {
            GetWindow().DataContext = new MainViewModel();
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
            Messenger.Default.Register<string>(GetWindow(), "minWindow", (string argv) =>
            {
                GetWindow().WindowState = WindowState.Minimized;
            });

            //窗体最大化
            Messenger.Default.Register<bool>(GetWindow(), "maxWindow", (bool argv) =>
            {
                var window = GetWindow();

                if (window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                }
            });

            //窗体退出窗口
            Messenger.Default.Register<string>(GetWindow(), "exitWindow", (string argv) =>
            {
                //设置退出代码
                _dialogResult = argv;
                CloseDialog();
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