namespace DCS.TASK.NET.ViewModel.MesShow
{
    internal class MesTagItem
    {
        private string _interfacename;
        private string _interfacecode;
        private string _interfacemsg;
        private string _updateTime;

        /// <summary>
        /// 接口调用时间
        /// </summary>
        public string UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }

        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string Interfacemsg
        {
            get { return _interfacemsg; }
            set { _interfacemsg = value; }
        }

        /// <summary>
        /// 接口返回code
        /// </summary>
        public string Interfacecode
        {
            get { return _interfacecode; }
            set { _interfacecode = value; }
        }

        /// <summary>
        /// 接口名称
        /// </summary>

        public string Interfacename
        {
            get { return _interfacename; }
            set { _interfacename = value; }
        }
    }
}