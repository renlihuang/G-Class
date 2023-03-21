using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.Model.Entiry;
using CS.View.Common.Enum;
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
    internal class ProductOverStopViewModel : DataProcess<ProductOverStopEntity>
    {
        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        readonly IProductOverStopService _productOverStopService;

        /// <summary>
        /// 工艺路线接口
        /// </summary>
        readonly IProductService _productService;

        /// <summary>
        /// 参数明细
        /// </summary>
        readonly ProductOverStopEntity _productOverStopEntity;

        /// <summary>
        /// 过站站点下拉框数据
        /// </summary>
        public ObservableCollection<CombBoxItem> OverStopItems { get; private set; }

        public ProductOverStopViewModel(IProductOverStopService productOverStopService, IProductService productService)
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
            for (int i = 0; i < productDatas.Result.Total; i++)
            {
                OverStopItems.Add(new CombBoxItem() { QueryText3 = productDatas.Result.Data[i].ProductName, Value = i });
            }
            base.Init();
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
            SelectedItem = null;
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
            var result = await _productOverStopService.GetProductOverStopAsync(pageInex, PageSize, new ProductOverStopCondition()
            {
                ProductCode = QueryText,
                StandInfo = SelectedItem == null ? null : SelectedItem.QueryText3, // 过站信息,
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
                foreach (var item in result.Data)
                {
                    DataGridDatas.Add(item);
                }
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
    }
}
