namespace CS.View.Common
{
    /// <summary>
    /// Tab页信息
    /// </summary>
    internal class TabPageInfo
    {
        /// <summary>
        /// 关闭按钮是否可见
        /// </summary>
        private bool _closeButtonVisible;

        public bool CloseButtonVisible
        {
            set { _closeButtonVisible = value; }
            get { return _closeButtonVisible; }
        }

        /// <summary>
        /// 页标题文字
        /// </summary>
        private string _pageText;

        public string PageText
        {
            set { _pageText = value; }
            get { return _pageText; }
        }

        private object _pageBody;

        /// <summary>
        ///
        /// </summary>
        public object PageBody
        {
            set { _pageBody = value; }
            get { return _pageBody; }
        }
    }
}