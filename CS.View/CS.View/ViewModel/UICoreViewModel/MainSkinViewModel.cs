using CS.View.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.View.ViewModel
{
    /// <summary>
    /// 皮肤管理窗口
    /// </summary>
    class MainSkinViewModel : ViewModelBase
    {
        /// <summary>
        /// 皮肤列表
        /// </summary>
        public IEnumerable<Swatch> Swatches { set; get;}


        bool _isChecked;

        Swatch _oldswatch = null;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            set { _isChecked = true; RaisePropertyChanged();}
            get { return _isChecked; }
        }

        /// <summary>
        /// 设置皮肤
        /// </summary>
        public RelayCommand<Swatch> ApplyCommand { get; set; }


        public MainSkinViewModel()
        {
            SwatchesProvider swatchesProvider = new SwatchesProvider();
            //获取皮肤列表
            Swatches = swatchesProvider.Swatches;
            //设置命令
            ApplyCommand = new RelayCommand<Swatch>(Apply);
        }


        private void Apply(Swatch swatch)
        {
            if (_oldswatch != swatch)
            {
                _oldswatch = swatch;
                //设置皮肤
                ServiceFiguration.SetSkin(swatch);
                ServiceFiguration.SetSkinName(swatch.Name);
            }
        
        }
    }
}
