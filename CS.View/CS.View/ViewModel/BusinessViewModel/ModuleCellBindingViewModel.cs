using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;

namespace CS.View.ViewModel.BusinessViewModel
{
    internal class ModuleCellBindingViewModel: DataProcess<ModuleCellBindingEntity>
    {
        readonly IModuleCellBindingService _moduleCellBindingService;
        public ModuleCellBindingViewModel(IModuleCellBindingService moduleCellBindingService)
        {
            _moduleCellBindingService = moduleCellBindingService;
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
            var queryResult = await _moduleCellBindingService.GetModuleCellBindingListAsync(pageInex, PageSize, new ModuleCellBindingCondition() { });

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
    }
}
