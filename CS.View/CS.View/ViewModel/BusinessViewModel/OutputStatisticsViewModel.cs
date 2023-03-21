using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.Model.Entiry;
using CS.View.Common.Enum;
using CS.View.FrameControl;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace CS.View.ViewModel.BusinessViewModel
{
    internal class OutputStatisticsViewModel : DataProcess<ProductOverStopEntity>
    {
        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        readonly IProductOverStopService _productOverStopService;

        /// <summary>
        /// 工艺路线接口
        /// </summary>
        private readonly IProductService _productService;

        /// <summary>
        /// 参数明细
        /// </summary>
        readonly ProductOverStopEntity _productOverStopEntity;

        /// <summary>
        /// 设置菜单命令
        /// </summary>
        public RelayCommand<RoleEntiry> SetRoleMenuCommand { get; private set; }

        /// <summary>
        ///下拉框数据
        /// </summary>
        public ObservableCollection<CombBoxItem> RoleCombBoxItems { get; private set; }

        /// <summary>
        /// 过站站点下拉框数据
        /// </summary>
        public ObservableCollection<CombBoxItem> OverStopItems { get; private set; }

        public OutputStatisticsViewModel(IProductOverStopService productOverStopService, IProductService productService)
        {
            _productOverStopService = productOverStopService; 
            _productService = productService;
            //ParamDetailCommand = new RelayCommand<BatteryCoreOcvTestEntity>(ParamDetail);

            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Init()
        {

            // 过站站点下拉框
            OverStopItems = new ObservableCollection<CombBoxItem>();
            // 获取过站站点数据
            var productDatas = _productService.GetProductListAsync(1, 20, new ProductCondition() { });
            for (int i = 0;i<productDatas.Result.Total;i++)
            {
                OverStopItems.Add(new CombBoxItem() { QueryText3 = productDatas.Result.Data[i].ProductName, Value = i });
            }
            // SelectedItem.QueryText3 = OverStopItems[0].QueryText3;
            SelectedItem = OverStopItems[0];

            //过站结果下拉框
            RoleCombBoxItems = new ObservableCollection<CombBoxItem>();
            RoleCombBoxItems.Add(new CombBoxItem() { Text = "ok", Value = 0 });
            RoleCombBoxItems.Add(new CombBoxItem() { Text = "ng", Value = 1 });
            SelectedItem1 = RoleCombBoxItems[0];
            // 关联命令
            SetRoleMenuCommand = new RelayCommand<RoleEntiry>(SetRoleMenu);
            base.Init();
        }

        /// <summary>
        /// 设置菜单树命令
        /// </summary>
        /// <param name="model"></param>
        private async void SetRoleMenu(RoleEntiry model)
        {
            BaseMsgDialog baseMsgDialog = new BaseMsgDialog();

            //绑定viewModel
            baseMsgDialog.BindDataContex<SelectMenuWindow, SelectMenuViewModel>(new SelectMenuWindow(), new SelectMenuViewModel(model));
            //显示对话框
            await baseMsgDialog.ShowDialog();
        }

        /// <summary>
        /// 参数明细命令
        /// </summary>
        public RelayCommand<ProductOverStopEntity> ParamDetailCommand { get; private set; }
        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Reset()
        {
            QueryText = string.Empty;
            StartTime = string.Empty;
            EndTime = string.Empty;
            QueryText3 = string.Empty;
            SelectedItem = null;
            SelectedItem1 = null;
            StartTime = null;
            EndTime = null;
            // ModuleCode = string.Empty;
        }

        protected override void SetDefaultToolBarButtons()
        {
            base.SetDefaultToolBarButtons();

            ToolBarButtons.Clear();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            //查询数据
            var result = await _productOverStopService.GetProductOverStopAllAsync(pageInex, PageSize, new ProductOverStopCondition()
            {
                ProductType = QueryText, // 产品类型
                OverStop = SelectedItem1 == null ? null : SelectedItem1.Text, // 过站结果
                // StandInfo = SelectedItem == null ? null : SelectedItem.QueryText3, // 过站信息
                CreateTimeStart = StartTime,
                CreateTimeEnd = EndTime
                //ID = _batteryCoreOcvTestEntity.Id
            });

            //清除显示数据
            DataGridDatas.Clear();
            TotailCount = result.Total;
            //填充数据
            if (result.Data != null)
            {
                //foreach (var item in result.Data)
                //{
                //    DataGridDatas.Add(item);
                //}
                // DataGridDatas.Add(new ProductOverStopEntity { StandInfo = SelectedItem == null ? "所有站点" : SelectedItem.QueryText3, Count = TotailCount });
                DataGridDatas.Add(new ProductOverStopEntity { ProductType = string.IsNullOrWhiteSpace(QueryText) ? "所有产品类型": QueryText, Count = TotailCount });
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected async override void Save()
        {
            bool result = false;

            if (Mode == ActionMode.Add)
            {
                Model.Id = _productOverStopEntity.Id;
                //添加数据
                result = await _productOverStopService.AddProductOverStopAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _productOverStopService.UpdateProductOverStopAsync(Model.Id, Model);
            }

            if (result)
            {
                base.Save();
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        protected async override void Delete(ProductOverStopEntity model)
        {
            bool result = await _productOverStopService.DeteleProductOverStopAsync(model.Id);

            if (result)
            {
                base.Delete(model);
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _queryText;
        public string QueryText
        {
            set
            {
                if (_queryText != value)
                {
                    _queryText = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _queryText;
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _startTime;
        public string StartTime
        {
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _startTime;
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _endTime;
        public string EndTime
        {
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _endTime;
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _queryText3;
        public string QueryText3
        {
            set
            {
                if (_queryText3 != value)
                {
                    _queryText3 = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _queryText3;
            }
        }

        /// <summary>
        /// 下拉框选中的值
        /// </summary>
        private CombBoxItem _selectedItem;

        public CombBoxItem SelectedItem
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
        /// 下拉框选中的值
        /// </summary>
        private CombBoxItem _selectedItem1;

        public CombBoxItem SelectedItem1
        {
            set
            {
                if (_selectedItem1 != value)
                {
                    _selectedItem1 = value;
                    RaisePropertyChanged();
                }
            }
            get { return _selectedItem1; }
        }
    }
}
