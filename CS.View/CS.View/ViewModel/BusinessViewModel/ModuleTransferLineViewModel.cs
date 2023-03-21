using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    internal class ModuleTransferLineViewModel : DataProcess<ModuleTransferLineEntity>
    {
        readonly IModuleTransferLineService _moduleTransferLineService;
        public ModuleTransferLineViewModel(IModuleTransferLineService moduleTransferLineService)
        {
            _moduleTransferLineService = moduleTransferLineService;
            //初始化
            this.Init();
        }


        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            //查询数据  
            var queryResult = await _moduleTransferLineService.GetModuleTransferLineListAsync(pageInex, PageSize, new ModuleTransferLineCondition() { });

            TotailCount = queryResult.Total;
            if (queryResult.Data != null)
            {
                DataGridDatas.Clear();

                foreach (var item in queryResult.Data)
                {
                    DataGridDatas.Add(item);
                }
            }
        }

        protected override void Add(ModuleTransferLineEntity model)
        {
         
            base.Add(model);
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="model"></param>
        protected override void Edit(ModuleTransferLineEntity model)
        {
           
            base.Edit(model);
        }



        /// <summary>
        /// s's
        /// </summary>
        protected async override void Save()
        {

            bool result = false;

            if (string.IsNullOrEmpty(Model.ModuleGroupNumber))
            {
                HnitText = "请输入模组名称";
                return;
            }

            if (string.IsNullOrEmpty(Model.ModuleCode))
            {
                HnitText = "请输入模组编码";
                return;
            }

        

         



            if (Mode == ActionMode.Add)
            {
                result = await _moduleTransferLineService.AddModuleTransferLineAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _moduleTransferLineService.UpdateModuleTransferLineAsync(Model.Id, Model);
            }

            if (result)
            {
                base.Save();
            }
        }


        protected override void Cancel()
        {
            base.Cancel();
            //

        }


        protected async override void Delete(ModuleTransferLineEntity model)
        {
            bool result = await SkinMessageBox.Question("确认删除?");

            if (result)
            {
                if (await _moduleTransferLineService.DeleteModuleTransferLineAsync(model.Id))
                {
                    base.Delete(model);
                }
            }
        }
    }
}
