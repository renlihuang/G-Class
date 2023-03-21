using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class ModuleGroupViewModel: DataProcess<ModuleGroupEntity>
    {
        /// <summary>
        /// 查询数据接口
        /// </summary>
        readonly IModuleGroupService _moduleGroupService;

        public ModuleGroupViewModel(IModuleGroupService moduleGroupService)
        {
            _moduleGroupService = moduleGroupService;
            this.Init();
        }

        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Cancel()
        {
            GroupCode = string.Empty;
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
            var result = await _moduleGroupService.GetModuleGroupsAync(pageInex, PageSize, new ModuleGroupCondition() { GroupCode = GroupCode });

            TotailCount = result.Total;
            DataGridDatas.Clear();

            if (result.Data != null)
            {
                result.Data.ForEach((item)=>
                {
                    DataGridDatas.Add(item);
                });
            }
        }

        /// <summary>
        /// 套号
        /// </summary>
        private string _groupCode;
        public string GroupCode
        {
            set { _groupCode = value;RaisePropertyChanged();}
            get { return _groupCode; }
        }
    }
}
 