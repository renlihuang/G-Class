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
    class ModuleWeightViewModel:DataProcess<ModuleWeightEntity>
    {
        /// <summary>
        /// 业务接口
        /// </summary>
        readonly IModuleWeightService _moduleWeightService;

        public ModuleWeightViewModel(IModuleWeightService moduleWeightService)
        {
            _moduleWeightService = moduleWeightService;
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
        /// 查询数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
           var result = await _moduleWeightService.GetModuleWeightsAync(pageInex, PageSize, new ModuleWeightQueryCondtion() { ModuleCode = ModuleCode });
            //清空数据
            DataGridDatas.Clear();
            //设置页数
            TotailCount = result.Total;
            //循环填充数据
            if (result.Data != null)
            {
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
        /// 清除模组条码
        /// </summary>
        protected override void Reset()
        {
            ModuleCode = string.Empty;
        }

        /// <summary>
        /// 查询模组条码
        /// </summary>
        string _moduleCode;

        public string ModuleCode
        {
            set
            {
                if (_moduleCode != value)
                {
                    _moduleCode = value;
                    RaisePropertyChanged();
                }
            }
            get { return _moduleCode; }
        }
    }
}
