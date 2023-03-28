using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DCS.TASK.NET.ViewModel.Base
{
    internal class BaseWindowViewModel:ViewModelBase
    {
        public BaseWindowViewModel(Window ownerWindow)
        {
            _ownerWindow = ownerWindow;
            //移动事件
            _ownerWindow.MouseMove += Window_MouseMove;
            _ownerWindow.Loaded += Window_Loaded;

            //关联命令
            MaxmizeCommand = new RelayCommand(Maxmize);
            MinmizeCommand = new RelayCommand(Minmize);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }
        /// <summary>
        /// 窗口加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OnStartup();
        }

        /// <summary>
        /// 设置UI线程定时器
        /// </summary>
        /// <param name="interval"></param>
        protected void SetTimer(double seconds)
        {
            if (_dispatcherTimer == null)
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Interval = TimeSpan.FromSeconds(seconds);
                _dispatcherTimer.Tick += Timer_Tick;
                //启动定时器
                _dispatcherTimer.Start();
            }
        }

        /// <summary>
        /// 定时器回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void  Timer_Tick(object sender, EventArgs e)
        {
            //调用定时
            OnTimer();
        }

        /// <summary>
        /// 定时消息
        /// </summary>
        protected virtual void OnTimer()
        {  
        }

        /// <summary>
        /// 程序加载完成
        /// </summary>
        protected virtual void OnStartup()
        {
        }


        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _ownerWindow.DragMove();
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        private void Minmize()
        {
            _ownerWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最小化
        /// </summary>
        private void Maxmize()
        {
            if (_ownerWindow.WindowState == WindowState.Normal)
            {
                _ownerWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                _ownerWindow.WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        private void CloseWindow()
        {
            OnClose();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        protected virtual void OnClose()
        {
            _ownerWindow.Close();
        }

        /// <summary>
        /// 关闭命令
        /// </summary>
        public RelayCommand MaxmizeCommand { private set; get; }

        /// <summary>
        /// 关闭命令
        /// </summary>
        public RelayCommand MinmizeCommand { private set; get; }

        /// <summary>
        /// 关闭命令
        /// </summary>
        public RelayCommand CloseWindowCommand { private set; get; }

        /// <summary>
        /// 拥有者窗口
        /// </summary>
        private Window _ownerWindow;

        /// <summary>
        /// 线程定时器
        /// </summary>
        private DispatcherTimer _dispatcherTimer;
    }
}
