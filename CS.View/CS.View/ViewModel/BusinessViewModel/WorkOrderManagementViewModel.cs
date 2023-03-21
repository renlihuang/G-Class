using CS.IBLL.Business;
using CS.Model.Basic.Entiry;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class WorkOrderManagementViewModel : DataProcess<WorkOrderManagementEntity>
    {


        /// <summary>
        /// 下拉框选中值
        /// </summary>
        private KeyAndValue _selectedItem;

        public KeyAndValue SelectedItem
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
        /// 角色选择下拉框
        /// </summary>
        public ObservableCollection<KeyAndValue> WorkOrderStatusCombBoxItems { get; private set; } = new ObservableCollection<KeyAndValue>();

        /// <summary>
        /// 查询数据接口
        /// </summary>
        readonly IWorkOrderManagementService _workorderManagement;

        public WorkOrderManagementViewModel(IWorkOrderManagementService ModuleStackService)
        {
            _workorderManagement = ModuleStackService;
            this.Init();
        }

        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Cancel()
        {
            base.Cancel();
        }
        protected override void Init()
        {
            base.Init();
            WorkOrderStatusCombBoxItems.Add(new KeyAndValue() { Key = 1, Value = "未下发" });
            WorkOrderStatusCombBoxItems.Add(new KeyAndValue() { Key = 2, Value = "已下发" });
            WorkOrderStatusCombBoxItems.Add(new KeyAndValue() { Key = 3, Value = "生产中" });
            WorkOrderStatusCombBoxItems.Add(new KeyAndValue() { Key = 4, Value = "暂停中/已完成" });
        }
        protected override void Edit(WorkOrderManagementEntity model)
        {
            //设置下拉框编辑的值
            SelectedItem = WorkOrderStatusCombBoxItems.FirstOrDefault(x => x.Key == model.WorkOrderStatus);
            base.Edit(model);
        }

        /// <summary>
        /// 设置工具栏按钮
        /// </summary>
        //protected override void SetDefaultToolBarButtons()
        //{
        //    base.SetDefaultToolBarButtons();
        //    //不需要添加按钮
        //    //ToolBarButtons.Clear();
        //    //DetailButtons.Clear();
        //}

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            var result = await _workorderManagement.GetWorkOrderManagementAync(pageInex, PageSize, new WorkOrderManagementCondition() { WorkOrderNo = WorkOrderNo });

            TotailCount = result.Total;
            DataGridDatas.Clear();

            if (result.Data != null)
            {
                result.Data.ForEach((item) =>
                {
                    KeyAndValue entityKeyValue = WorkOrderStatusCombBoxItems.Where(x => x.Key == item.WorkOrderStatus).FirstOrDefault();
                    item.WorkOrderStatusName = entityKeyValue.Value;
                    DataGridDatas.Add(item);
                });
            }
        }

        /// <summary>
        /// 套号
        /// </summary>
        private string _workOrderNo;
        public string WorkOrderNo
        {
            set { _workOrderNo = value; RaisePropertyChanged(); }
            get { return _workOrderNo; }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="model"></param>
        protected override async void Delete(WorkOrderManagementEntity model)
        {
            bool dialogResult = await SkinMessageBox.Question("确认删除该用户信息");

            if (dialogResult)
            {
                //删除数据
                bool result = await _workorderManagement.DeleteAsync(model);

                if (result)
                {
                    //删除表格数据
                    base.Delete(model);
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected override async void Save()
        {
            bool result = false;

            if (string.IsNullOrEmpty(Model.ModuleName))
            {
                SkinMessageBox.Error("模组名称不能为空");
                return;
            }

            if (string.IsNullOrEmpty(Model.ModuleNumber))
            {
                SkinMessageBox.Error("模组编号不能为空");
                return;
            }
            if (Model.ProductionQuantity <= 0)
            {
                SkinMessageBox.Error("生产数量不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Model.WorkOrderNo))
            {
                SkinMessageBox.Error("工单编号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Model.WorkOrderDescription))
            {
                SkinMessageBox.Error("工单说明不能为空");
                return;
            }

            if (SelectedItem.Key == 0)
            {
                SkinMessageBox.Error("请选择工单状态");
                return;
            }
            Model.WorkOrderStatus = SelectedItem.Key;
            if (Mode == ActionMode.Add)
            {
                //添加数据
                result = await _workorderManagement.InsertAsync(Model);

                if (result)
                {
                    base.Save();
                }
            }
            else if (Mode == ActionMode.Edit)
            {
                //更新数据
                result = await _workorderManagement.UpdateAsync(Model);

                if (result)
                {
                    base.Save();
                }
            }

        }
    }
}
