using CS.View.Common;
using CS.View.FrameControl;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace CS.View.ViewModel
{
    internal class MainPopupBoxViewModel : ViewModelBase
    {
        public ObservableCollection<PopupBoxModel> PopupBoxModels { set; get; }

        /// <summary>
        /// 退出登录
        /// </summary>
        public RelayCommand logoutCommand { set; get; }

        /// <summary>
        ///设置
        /// </summary>
        public RelayCommand SetCommand { set; get; }

        /// <summary>
        ///更换界面
        /// </summary>
        public RelayCommand SkinCommand { set; get; }

        public MainPopupBoxViewModel()
        {
            PopupBoxModels = new ObservableCollection<PopupBoxModel>();
            //关联命令
            logoutCommand = new RelayCommand(Logout);
            SetCommand = new RelayCommand(Set);
            SkinCommand = new RelayCommand(Skin);

            LoadPopupBoxModels();
        }

        /// <summary>
        /// 加载
        /// </summary>
        private void LoadPopupBoxModels()
        {
            PopupBoxModels.Add(new PopupBoxModel() { IconName = "Logout", Text = "退出登录", ExcuteCommand = logoutCommand });
            PopupBoxModels.Add(new PopupBoxModel() { IconName = "Set", Text = "系统设置", ExcuteCommand = SetCommand });
            PopupBoxModels.Add(new PopupBoxModel() { IconName = "Palette", Text = "主题设置", ExcuteCommand = SkinCommand });
        }

        /// <summary>
        ///
        /// </summary>
        private async void Logout()
        {
            bool isLogout = false;

            isLogout = await SkinMessageBox.Question("确认注销并退出登录?");

            if (isLogout)
            {
                Messenger.Default.Send("logout", "exitWindow");
            }
        }

        /// <summary>
        ///系统设置
        /// </summary>
        private void Set()
        {
        }

        /// <summary>
        ///
        /// </summary>
        private async void Skin()
        {
            BaseMsgDialog baseMsgDialog = new BaseMsgDialog();
            baseMsgDialog.BindDataContex<MainSkinWindow, MainSkinViewModel>(new MainSkinWindow(), new MainSkinViewModel());
            bool isShow = await baseMsgDialog.ShowDialog();
        }
    }

    internal class PopupBoxModel : ViewModelBase
    {
        private string _iconName;

        /// <summary>
        /// 图片名称
        /// </summary>
        public string IconName
        {
            set { _iconName = value; RaisePropertyChanged(); }
            get { return _iconName; }
        }

        private string _text;

        /// <summary>
        /// 标题名称
        /// </summary>
        public string Text
        {
            set { _text = value; RaisePropertyChanged(); }
            get { return _text; }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public RelayCommand ExcuteCommand { set; get; }
    }
}