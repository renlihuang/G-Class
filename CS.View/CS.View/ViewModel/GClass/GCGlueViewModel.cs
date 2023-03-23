using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.GClass
{
    internal class GCGlueViewModel : DataProcess<GCGlueEntity>
    {
        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        private readonly IGCGlueService _petService;

        /// <summary>
        /// 参数明细
        /// </summary>
        private readonly GCGlueEntity petEntity;

        public GCGlueViewModel(IGCGlueService petService)
        {
            _petService = petService;

            this.Init();
        }
        /// <summary>
        /// 参数明细命令
        /// </summary>
        public RelayCommand<GCGlueEntity> ParamDetailCommand { get; private set; }

        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Reset()
        {
            QueryCode = string.Empty;
            GetPageData(1);
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
        protected override async void GetPageData(int pageInex)
        {
            //查询数据
            var result = await _petService.GetGCGlueAsync(pageInex, PageSize, new GCGlueCondition()
            {
                ModuleCode = QueryCode,
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
        protected override async void Save()
        {
            bool result = false;

            if (Mode == ActionMode.Add)
            {
                Model.Id = petEntity.Id;
                //添加数据
                result = await _petService.AddGCGlueAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _petService.UpdateGCGlueAsync(Model.Id, Model);
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
        protected override async void Delete(GCGlueEntity model)
        {
            bool result = await _petService.DeteleGCGlueAsync(model.Id);

            if (result)
            {
                base.Delete(model);
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        private string _queryText;

        public string QueryCode
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
        private string _queryText1;

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
        private string _queryText2;

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
        private string _queryText3;

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
