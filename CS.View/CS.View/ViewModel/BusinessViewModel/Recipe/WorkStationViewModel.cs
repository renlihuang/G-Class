using CS.IBLL.Basic;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common.Enum;
using CS.View.View.Dlg;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel.Recipe
{
    class WorkStationViewModel : DataProcess<RepairOrderEntity>
    {
        readonly IWorkStationService _workStationService;
        readonly IProductService _productService;

        public WorkStationViewModel(IWorkStationService workStationService, IProductService productService)
        {
            _workStationService = workStationService;
            _productService = productService;
            //关联命令
            EditRecipeCommand = new RelayCommand<RepairOrderEntity>(EditRecipe);
            //执行初始化、
            this.Init();
        }

        protected async override void GetPageData(int pageInex)
        {
            var result = await _workStationService.GetRepairOrderListAync(pageInex, PageSize, new RepairOrderCondition() { WorkOrderNo = RecipeName });
            if (result.Data != null)
            {
                DataGridDatas.Clear();

                foreach (var item in result.Data)
                {
                    DataGridDatas.Add(item);
                }
            }
        }

        protected async override void Delete(RepairOrderEntity model)
        {
            if (await _workStationService.DeleteRepairOrderAync(model.Id))
            {
                base.Delete(model);
            }
        }

        protected async override void Save()
        {
            bool result = false;

            if (string.IsNullOrEmpty(Model.WorkOrderNo))
            {
                HnitText = "工单编号不能为空";
                return;
            }

            if (Mode == ActionMode.Add)
            {
                result = await _workStationService.AddRepairOrderAync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _workStationService.UpdateRepairOrderAync(Model.Id, Model);
            }

            if (result)
            {
                base.Save();
            }
        }

        /// <summary>
        /// 编辑配方
        /// </summary>
        /// <param name="entity"></param>
        private async void EditRecipe(RepairOrderEntity entity)
        {

            BaseMsgDialog dialog = new BaseMsgDialog();

            ProductofLineViewModel productofLineViewModel = new ProductofLineViewModel(_workStationService, _productService, entity.ProductType)
            {
                RecipeName = entity.WorkOrderNo
            };
            //绑定视图
            dialog.BindDataContex<ProductofLineDialog, ProductofLineViewModel>(new ProductofLineDialog(), productofLineViewModel);
            //显示对话框
            if (await dialog.ShowDialog())
            {
                entity.ProductType = productofLineViewModel.RecipeList;
                //更新到数据库
                await _workStationService.UpdateRepairOrderAync(entity.Id, entity);
            }
        }

        /// <summary>
        /// 编辑配方命令
        /// </summary>
        public RelayCommand<RepairOrderEntity> EditRecipeCommand { private set; get; }

        /// <summary>
        /// 配方名称
        /// </summary>
        string _recipeName;
        public string RecipeName
        {
            set { _recipeName = value; RaisePropertyChanged(); }
            get { return _recipeName; }
        }

        protected override void Reset()
        {
            RecipeName = String.Empty;
        }

    }
}
