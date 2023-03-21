using GalaSoft.MvvmLight;

namespace CS.View.ViewModel.Base
{
    /// <summary>
    ///下拉框值
    /// </summary>
    internal class CombBoxItem : ViewModelBase
    {
        /// <summary>
        /// 标题文本
        /// </summary>
        public string Text { get; set; }

        //文本对应的的值
        public int Value { get; set; }

        /// <summary>
        /// 标题文本
        /// </summary>
        public string QueryText3 { get; set; }
    }
}