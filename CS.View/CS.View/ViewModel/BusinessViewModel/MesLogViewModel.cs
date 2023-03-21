using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common;
using CS.View.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    /// <summary>
    /// 查询MESLOG
    /// </summary>
    class MesLogViewModel: DataProcess<MesLogEntity>
    {
        readonly IMesLogService _mesLogService;
        public MesLogViewModel(IMesLogService mesLogService)
        {
            _mesLogService = mesLogService;
            this.Init();
        }

        // <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Cancel()
        {
            ModuleCode = string.Empty;
        }


        /// <summary>
        /// 设置工具栏按钮
        /// </summary>
        protected override void SetDefaultToolBarButtons()
        {
            base.SetDefaultToolBarButtons();
            //不需要添加按钮
            ToolBarButtons.Clear();
            DetailButtons.Clear();
        }

        protected async override void GetPageData(int pageInex)
        {
            var result = await _mesLogService.GetMesLogsAync(pageInex, PageSize, new MesLogQueryCondtion() { ModuleCode = ModuleCode });

            TotailCount = result.Total;
            DataGridDatas.Clear();
            //
            if (result.Data != null)
            {
                result.Data.ForEach((item) =>
                {
                    DataGridDatas.Add(item);
                });
            }
            else
            {
                SkinMessageBox.Error("调用接口失败,请检查网络");
            }
        }

        /// <summary>
        /// 模组条码
        /// </summary>
        private string _moduleCode;
        public string ModuleCode
        {
            set { _moduleCode = value; RaisePropertyChanged(); }
            get { return _moduleCode; }
        }
    }
}
