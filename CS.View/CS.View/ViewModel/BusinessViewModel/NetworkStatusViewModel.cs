using CS.IBLL.Business;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{


    class NetworkStatusViewModel : ViewModelBase
    {
        public NetworkStatusViewModel(IDeviceNetworkInfoService deviceNetworkInfoService)
        {
            _deviceNetworkInfoService = deviceNetworkInfoService;

            CheckNetworkCommand = new RelayCommand(CheckNetwork);
            NetworkStatusItems = new ObservableCollection<NetworkStatusItem>();
            //检测网络
            CheckNetwork();
        }

        /// <summary>
        /// 检查网络
        /// </summary>
        private async void CheckNetwork()
        {
            //已经在检测就不能再点了
            if (_isCheck)
                return;
            //设置检测状态
            _isCheck = true;
            //加载数据
            await LoadData();
            //检查网络状态
            NetworkStatus();
        }

        /// <summary>
        /// 检查网络状态
        /// </summary>
        private async void NetworkStatus()
        {
            //当前ping
            Ping ping = new Ping();
            //要检测的网络总数
            int totailCount = NetworkStatusItems.Count;
            //当前数量
            int currentCount = 0;
            //设置进度
            ProcessRate = 10;
            //设置进度
            ProcessText = "10";
            //循环检测所有设备
            foreach (var item in NetworkStatusItems)
            {
                //ping IP地址
                var result = await ping.SendPingAsync(item.IpAddr);
                //判断是否完成
                item.SetStatus(result.Status == IPStatus.Success);
                //设置检测时间
                item.CreateTime = DateTime.Now;
                //数量加1
                currentCount++;
                //设置进度
                ProcessRate = currentCount * 100.0 / totailCount;
                //设置文本
                ProcessText = ProcessRate.ToString();
            }

            _isCheck = false;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private async Task LoadData()
        {
            var result = await _deviceNetworkInfoService.GetListAync();
            //清除数据
            NetworkStatusItems.Clear();
            //循环添加数据
            foreach (var item in result)
            {
                NetworkStatusItem networkStatusItem = new NetworkStatusItem
                {
                    DeviceName = item.DeviceName,
                    IpAddr = item.IpAddr,
                    CreateTime = DateTime.Now
                };
                //添加数据
                NetworkStatusItems.Add(networkStatusItem);
            }
        }


        private bool _isCheck;

        /// <summary>
        /// 数据调用接口
        /// </summary>
        readonly IDeviceNetworkInfoService _deviceNetworkInfoService;

        /// <summary>
        /// 关联表格节点
        /// </summary>
        public ObservableCollection<NetworkStatusItem> NetworkStatusItems { private set; get; }

        /// <summary>
        /// 绑定命令
        /// </summary>
        public RelayCommand CheckNetworkCommand { private set; get; }

        /// <summary>
        /// 当前进度
        /// </summary>
        private double _processRate;
        public double ProcessRate
        {
            set { _processRate = value;RaisePropertyChanged();}
            get { return _processRate; }
        }

        /// <summary>
        /// 当前进度文本
        /// </summary>
        private string _processText = "等待检查网络";
        public string ProcessText
        {
            set 
            {
              _processText = $"检测进度{value}%";
              RaisePropertyChanged();
            }
            get 
            {
                return _processText; 
            }
        }
    }

    /// <summary>
    /// 网络状态
    /// </summary>
    class NetworkStatusItem: ViewModelBase
    {
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if (status)
            {
                StatusColor = "Green";
                Icon = "Check";
                StatusText = "在线";

            }
            else
            {
                StatusColor = "Red";
                Icon = "Close";
                StatusText = "离线";
            }
        }

        /// <summary>
        ///  设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        ///  IP地址
        /// </summary>
        public string IpAddr { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime _createTime;
        public DateTime CreateTime 
        {
            set { _createTime = value;RaisePropertyChanged(); }
            get { return _createTime; }
        }

        /// <summary>
        /// 颜色
        /// </summary>
        string _statusColor = "Black";

        public string StatusColor
        {
            private set { _statusColor = value; RaisePropertyChanged(); }
            get { return _statusColor; }
        }

        /// <summary>
        /// 状态文本
        /// </summary>
        string _statusText = "等待检测";

        public string StatusText
        {
            private set { _statusText = value; RaisePropertyChanged(); }
            get { return _statusText; }
        }

        /// <summary>
        /// 图标
        /// </summary>
        string _icon = "Clock";

        public string Icon
        {
            private set { _icon = value; RaisePropertyChanged(); }
            get { return _icon; }
        }

    }
}
