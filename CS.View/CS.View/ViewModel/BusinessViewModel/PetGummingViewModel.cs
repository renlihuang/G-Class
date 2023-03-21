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
    internal class PetGummingViewModel : DataProcess<PetGummingEntity>
    {
        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        readonly IPetGummingService _petGummingService;

        /// <summary>
        /// 参数明细
        /// </summary>
        readonly PetGummingEntity _petGummingEntity;

        public PetGummingViewModel(IPetGummingService petGummingService)
        {
            _petGummingService = petGummingService;

            //ParamDetailCommand = new RelayCommand<BatteryCoreOcvTestEntity>(ParamDetail);

            this.Init();
        }

        /// <summary>
        /// 参数明细命令
        /// </summary>
        public RelayCommand<PetGummingEntity> ParamDetailCommand { get; private set; }
        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Reset()
        {
            QueryText = string.Empty;
            QueryText1 = string.Empty;
            QueryText2 = string.Empty;
            // ModuleCode = string.Empty;
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
            var result = await _petGummingService.GetPetGummingAsync(pageInex, PageSize, new PetGummingCondition()
            {
                ProductCode = QueryText,
                //CreateTimeStart = QueryText1,
                //CreateTimeEnd = QueryText2
                //ID = _batteryCoreOcvTestEntity.Id
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
                Model.Id = _petGummingEntity.Id;
                //添加数据
                result = await _petGummingService.AddPetGummingAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _petGummingService.UpdatePetGummingAsync(Model.Id, Model);
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
        protected async override void Delete(PetGummingEntity model)
        {
            bool result = await _petGummingService.DetelePetGummingAsync(model.Id);

            if (result)
            {
                base.Delete(model);
            }
        }

        /// <summary>
        /// 查询文本
        /// </summary>
        string _queryText;
        public string QueryText
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
        public string QueryText1
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
        public string QueryText2
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



    }
}
