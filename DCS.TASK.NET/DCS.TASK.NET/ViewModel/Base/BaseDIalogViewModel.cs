using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DCS.TASK.NET.ViewModel.Base
{
    internal class BaseDIalogViewModel: ViewModelBase
    {
        public BaseDIalogViewModel(Window ownerWindow)
        {
            _ownerWindow = ownerWindow;

            //_ownerWindow.MouseMove += OwnerWindow_MouseMove;
            _ownerWindow.Loaded += Window_Loaded;
            //关联命令
            CloseCommand = new RelayCommand(WindowClose);
            ConfirmCommand = new RelayCommand(Confrim);
            CancelCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OnStartup();
        }

        /// <summary>
        /// 窗体加载完成
        /// </summary>
        protected virtual void OnStartup()
        { 
           
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OwnerWindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
               // _ownerWindow.DragMove();
            }
        }

        /// <summary>
        /// 关闭命令
        /// </summary>
        private void WindowClose()
        {
            if (OnClose())
            {
                CloseWindow();
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnClose()
        {
            return true;
        }

        /// <summary>
        /// 确认命令
        /// </summary>
        private void Confrim()
        {
            OnConfrim();
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        protected virtual void OnConfrim()
        {
            CloseWindow();
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {
            OnCancel();
        }


        /// <summary>
        /// 取消命令
        /// </summary>
        protected virtual void OnCancel()
        {
            CloseWindow();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        protected void CloseWindow()
        {
            _ownerWindow.Close();
        }

        /// <summary>
        ///和当前viewModel
        /// </summary>
        private Window _ownerWindow;

        /// <summary>
        /// 标题
        /// </summary>
        private string _title;

        public string Title
        {
            set { _title = value;RaisePropertyChanged();}
            get { return _title; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _hintText;

        public string HintText
        {
            set 
            {
                if (_hintText != value)
                {
                    _hintText = value;
                    RaisePropertyChanged();
                }
            }
            get { return _hintText;}
        }

        /// <summary>
        /// 对话框
        /// </summary>
        public bool DialogResult { protected set; get;}

        /// <summary>
        /// 关闭命令
        /// </summary>
        public RelayCommand CloseCommand { private set; get; }

        /// <summary>
        /// 确定命令
        /// </summary>
        public RelayCommand ConfirmCommand { private set; get; }

        /// <summary>
        /// 确定命令
        /// </summary>
        public RelayCommand CancelCommand { private set; get; }
    }
}
