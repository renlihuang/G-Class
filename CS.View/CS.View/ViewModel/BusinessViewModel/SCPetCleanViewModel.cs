using CS.BLL.Business;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    internal class SCPetCleanViewModel : DataProcess<SCPetCleanEntity>
    {
        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        readonly ISCPetCleanService _petService;

        /// <summary>
        /// 参数明细
        /// </summary>
        readonly SCPetCleanEntity petEntity;

        public SCPetCleanViewModel(ISCPetCleanService petService)
        {
            _petService = petService;
            QueryStartTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00");
            QueryEndTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

            this.Init();
        }

        /// <summary>
        /// 参数明细命令
        /// </summary>
        public RelayCommand<SCPetCleanEntity> ParamDetailCommand { get; private set; }
        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Reset()
        {
            QueryTrayCode = string.Empty;
            QueryProductType = string.Empty;
            QueryStartTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00");
            QueryEndTime = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
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
            var result = await _petService.GetSCPetCleanAsync(pageInex, PageSize, new SCPetCleanCondition()
            {
                TrayCode = QueryTrayCode,
                ProductType = QueryProductType,
                CreateTimeStart = DateTime.Parse(QueryStartTime).ToString("yyyy-MM-dd 00:00:00"),
                CreateTimeEnd = DateTime.Parse(QueryEndTime).ToString("yyyy-MM-dd 23:59:59")
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
                Model.ID = petEntity.ID;
                //添加数据
                result = await _petService.AddSCPetCleanAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _petService.UpdateSCPetCleanAsync(Model.ID, Model);
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
        protected async override void Delete(SCPetCleanEntity model)
        {
            bool result = await _petService.DeteleSCPetCleanAsync(model.ID);

            if (result)
            {
                base.Delete(model);
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _queryText;
        public string QueryTrayCode
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
        string _queryText1;
        public string QueryStartTime
        {
            set
            {
                if (_queryText1 != value)
                {
                    _queryText1 = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _queryText1;
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _queryText2;
        public string QueryEndTime
        {
            set
            {
                if (_queryText2 != value)
                {
                    _queryText2 = value;
                    RaisePropertyChanged();
                }
            }
            get
            {
                return _queryText2;
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _queryText3;
        public string QueryProductType
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


    }
}
