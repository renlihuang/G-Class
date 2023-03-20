using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DCS.TASK.NET.ViewModel.ViewDlgViewModel
{


    /// <summary>
    /// 采集任务组
    /// </summary>
    internal class AddCollectionGroupViewModel : BaseDIalogViewModel
    {
        public AddCollectionGroupViewModel(Window ownerWindow) : base(ownerWindow)
        {
            //创建下拉框
            OpcItems = new ObservableCollection<OpcItem>();
            //设置
            this.Title = "采集任务组";
        }

        /// <summary>
        /// 提交
        /// </summary>
        protected override void OnConfrim()
        {
            if (string.IsNullOrEmpty(GroupName))
            {
                HintText = "请输入任务组名称";
                return;
            }

            if (string.IsNullOrEmpty(OpcUrl))
            {
                HintText = "请输入OPC UA URL";
                return;
            }

            //当前选中的值
            NodeNS = SelectedItem.Value;
            //表示已经点击过确认按钮
            DialogResult = true;

            base.OnConfrim();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStartup()
        {
            //加载OPC下拉框类型
            OpcItems.Add(new OpcItem() { Name = "Kepware OPC服务器", Value = 2 });
            OpcItems.Add(new OpcItem() { Name = "PLC内置OPC服务器", Value = 3 });
            OpcItems.Add(new OpcItem() { Name = "1200 PLC内置OPC服务器", Value = 4 });

            SelectedItem = OpcItems[0];

            if (NodeNS > 0)
            {
                SelectedItem = OpcItems.FirstOrDefault(x => x.Value == NodeNS);
            }
        }

        /// <summary>
        /// 任务组名称
        /// </summary>
        private string _groupName;

        public string GroupName
        {
            set { _groupName = value; }
            get { return _groupName; }
        }


        /// <summary>
        /// Opc Url地址
        /// </summary>
        private string _opcUrl;

        public string OpcUrl
        {
            set { _opcUrl = value; }
            get { return _opcUrl; }
        }

        /// <summary>
        /// 对应的NS值
        /// </summary>
        public short NodeNS { set; get; }

        /// <summary>
        /// 下拉框选择值
        /// </summary>
        private OpcItem _selectedItem;

        public OpcItem SelectedItem
        { 
           set 
           {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged();
                }
            
           }
           get { return _selectedItem; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OpcItem> OpcItems { private set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    class OpcItem
    {
        /// <summary>
        /// OPC类型名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 对应的OPC类型值
        /// </summary>
        public short Value { set; get; }
    }
}
