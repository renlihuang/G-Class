using CS.View.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CS.View.ViewModel
{
    class MainToolBarViewModel
    {
        /// <summary>
        /// 是否是最大化
        /// </summary>
        bool _isMax = true;

        /// <summary>
        /// 返回工具栏集合
        /// </summary>
        public ObservableCollection<ToolBarModel> ToolBarModels
        {
            get { return _toolBarModels; }
        }


        ObservableCollection<ToolBarModel> _toolBarModels = new ObservableCollection<ToolBarModel>();


        RelayCommand _minCommand;
        /// <summary>
        /// 最小化命令
        /// </summary>
        RelayCommand MinCommand
        {
            get 
            {
                if (_minCommand == null)
                {
                    _minCommand = new RelayCommand(Min);
                }
                return _minCommand;
            }
        }

        RelayCommand _maxCommand;
        /// <summary>
        /// 最小化命令
        /// </summary>
        RelayCommand MaxCommand
        {
            get
            {
                if (_maxCommand == null)
                {
                    _maxCommand = new RelayCommand(Max);
                }
           
                return _maxCommand;
            }
        }


        RelayCommand _ExitCommand;
        /// <summary>
        /// 最小化命令
        /// </summary>
        RelayCommand ExitCommand
        {
            get
            {
                if (_ExitCommand == null)
                {
                    _ExitCommand = new RelayCommand(Exit);
                }

                return _ExitCommand;
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        private void Min()
        {
            Messenger.Default.Send("最小化", "minWindow");
        }

        /// <summary>
        /// 最大化
        /// </summary>
        private void Max()
        {
            Messenger.Default.Send(_isMax, "maxWindow");
        }

        /// <summary>
        /// 退出窗口
        /// </summary>
        private async void Exit()
        {
            bool isQuit = await SkinMessageBox.Question("确认退出系统?");
            if (isQuit)
            {
                Messenger.Default.Send("退出系统", "exitWindow");
            }
        }

        /// <summary>
        /// 初始化工具栏
        /// </summary>
        public MainToolBarViewModel()
        {
            _toolBarModels.Add(new ToolBarModel() { IconName = "Minus",NotifyCommand = MinCommand });
            _toolBarModels.Add(new ToolBarModel() { IconName = "WindowMaximize", NotifyCommand = MaxCommand });
            _toolBarModels.Add(new ToolBarModel() { IconName = "Close", NotifyCommand = ExitCommand });
        }
    }


    /// <summary>
    /// 
    /// </summary>
    class ToolBarModel: ViewModelBase
    {
        string _iconName;

        /// <summary>
        ///按钮图标
        /// </summary>
        public string IconName
        {
            get { return _iconName;}
            set { _iconName = value; RaisePropertyChanged();}
        }

        /// <summary>
        /// 关联命令
        /// </summary>
        public RelayCommand NotifyCommand { set; get;}
    }
}
