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
    internal class BatteryCellScanCodeViewModel: DataProcess<BatteryCellScanCodeEntity>
    {
        /// <summary>
        /// 数据调用接口
        /// </summary>
        readonly IBatteryCellSanCodeService _batteryCellSanCodeService;

        public BatteryCellScanCodeViewModel(IBatteryCellSanCodeService batteryCellSanCodeService)
        {
            _batteryCellSanCodeService = batteryCellSanCodeService;
            //初始化
            this.Init();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            var result = await _batteryCellSanCodeService.GetBatteryCellSanCodeAync(pageInex, PageSize, new BatteryCellSanCodeQueryCondition());
            //设置页数
            TotailCount = result.Total;
            DataGridDatas.Clear();

            if (result.Data != null)
            {
                foreach (var dataItem in result.Data)
                {
                    DataGridDatas.Add(dataItem);
                }
            }
            else
            {
                SkinMessageBox.Error("调用接口失败,请检查网络");
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
                result = await _batteryCellSanCodeService.AddBatteryCellSanCodeAsync(Model);
            }

            if (result)
            {
                base.Save();
            }

        }


    }
}
