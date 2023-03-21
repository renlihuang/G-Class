using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CS.View.ViewModel
{
    /// <summary>
    ///
    /// </summary>
    internal class ImgaeInfo : ViewModelBase
    {
        private string imageName = string.Empty;

        public string ImageName
        {
            set { imageName = value; RaisePropertyChanged(); }
            get { return imageName; }
        }
    }

    /// <summary>
    ///
    /// </summary>
    internal class SelectImageViewModel : ViewModelBase
    {
        private List<ImgaeInfo> _imageNameList;

        /// <summary>
        ///
        /// </summary>
        private string[] _names;

        /// <summary>
        /// 图片名列表
        /// </summary>
        private List<ImgaeInfo> _imageNames;

        public List<ImgaeInfo> ImageNames
        {
            set { _imageNames = value; RaisePropertyChanged(); }
            get { return _imageNames; }
        }

        /// <summary>
        /// 选择图标命令
        /// </summary>
        public RelayCommand<ImgaeInfo> ApplyCommand { get; private set; }

        /// <summary>
        /// 刷新图片命令
        /// </summary>
        public RelayCommand RefreshImageCommand { get; private set; }

        /// <summary>
        /// 当前选择的图片信息
        /// </summary>
        private string _selectedImageName = string.Empty;

        public string SelectedImageName
        {
            set { _selectedImageName = value; RaisePropertyChanged(); }
            get { return _selectedImageName; }
        }

        /// <summary>
        ///
        /// </summary>
        public SelectImageViewModel()
        {
            ///加载图标
            LoadImage();

            ApplyCommand = new RelayCommand<ImgaeInfo>(Apply);

            RefreshImageCommand = new RelayCommand(Refresh);
        }

        /// <summary>
        /// 选择图片命令
        /// </summary>
        /// <param name="imgaeInfo"></param>
        public void Apply(ImgaeInfo imgaeInfo)
        {
            //设置图标
            SelectedImageName = imgaeInfo.ImageName;
        }

        /// <summary>
        /// 刷新图片
        /// </summary>
        public void Refresh()
        {
            RefreshImagae();
        }

        /// <summary>
        /// 刷新图片
        /// </summary>
        public void RefreshImagae()
        {
            //生成随机数
            Random random = new Random();

            if (_imageNameList == null)
            {
                _imageNameList = new List<ImgaeInfo>();

                for (int i = 0; i < 200; i++)
                {
                    _imageNameList.Add(new ImgaeInfo());
                }
            }

            //只加载前两百个图标
            for (int i = 0; i < 200; i++)
            {
                int index = random.Next(0, _names.Length - 1);
                _imageNameList[i].ImageName = _names[index];
            }
        }

        /// <summary>
        /// 加载图标
        /// </summary>
        private async void LoadImage()
        {
            if (_names == null)
            {
                await Task.Run(() =>
                {
                    _names = Enum.GetNames(typeof(PackIconKind));
                });
            }

            //刷新图标
            RefreshImagae();

            //设置图片
            ImageNames = _imageNameList;
        }
    }
}