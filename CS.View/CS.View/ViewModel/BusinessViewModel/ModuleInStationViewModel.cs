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
    /// 模组进站
    /// </summary>
    class ModuleInStationViewModel: DataProcess<ModuleInStationEntity>
    {
        /// <summary>
        /// 模组进站接口
        /// </summary>
        readonly IModuleInStationService _moduleInStationService;


        public ModuleInStationViewModel(IModuleInStationService moduleInStationService)
        {
            _moduleInStationService = moduleInStationService;
            //初始化
            this.Init();
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

        /// <summary>
        /// 清空模组条码
        /// </summary>
        protected override void Reset()
        {
            ModuleCode = string.Empty;
        }


        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            //调用API查询进站记录
            var result = await _moduleInStationService.GetModuleInStationsAync(pageInex, PageSize, new ModuleInStationQueryCondtion(){ });
            //设置页数
            TotailCount = result.Total;
            //填充查询数据
            DataGridDatas.Clear();

            if (result.Data != null)
            {
                //循环填充数据
                foreach (var item in result.Data)
                {
                    DataGridDatas.Add(item);
                }
            }
            else
            {
                SkinMessageBox.Error("调用接口失败,请检查网络");
            }
        }

        /// <summary>
        /// 查询模组条码
        /// </summary>
        string _moduleCode;

        public string ModuleCode
        {
            set { _moduleCode = value; RaisePropertyChanged(); }
            get { return _moduleCode; }
        }
    }
}
