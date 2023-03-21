using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.View.Dlg;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    /// <summary>
    /// 参数名管理
    /// </summary>
    class ParamNameViewModel : DataProcess<ParamNameEntiry>
    {
        /// <summary>
        /// 参数管理接口
        /// </summary>
        IParamNameService _paramNameService;

        public ParamNameViewModel(IParamNameService paramNameService)
        {
            _paramNameService = paramNameService;

            ParamDetailCommand = new RelayCommand<ParamNameEntiry>(ParamDetail);

            this.Init();
        }


        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Reset()
        {
            QueryText = string.Empty;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected async override void Save()
        {
            bool result = false;

            if (Mode == ActionMode.Add)
            {
                result = await _paramNameService.AddParamNameAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _paramNameService.UpdateParamNameAsync(Model.Id, Model);
            }

            if (result)
            {
                base.Save();
            }
        }


        protected async override void Delete(ParamNameEntiry model)
        {
            bool dialogResult = await SkinMessageBox.Question("确认删除该参数名吗?");

            if (dialogResult)
            {
                bool result = await _paramNameService.DeteleParamNameAsync(Model.Id);

                if (result)
                {
                    base.Delete(model);
                }
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            var result = await _paramNameService.GetParamNamesAsync(pageInex,PageSize, new ParamNameCondition() { Name = QueryText });
            //已经显示
            DataGridDatas.Clear();
            //设置页数
            TotailCount = result.Total;

            if (result.Data != null)
            {
                foreach (var item in result.Data)
                { 
                    DataGridDatas.Add(item);
                }
            }
        }

        /// <summary>
        /// 设置参数明细
        /// </summary>
        /// <param name="model"></param>
        public async void ParamDetail(ParamNameEntiry model)
        {
            BaseMsgDialog baseMsgDialog = new BaseMsgDialog();
            //绑定viewModel
            baseMsgDialog.BindDataContex(new ParamValueDialog(), new ParamValueViewModel(model));
            //显示对话框
            await baseMsgDialog.ShowDialog();
        }


        /// <summary>
        /// 参数明细命令
        /// </summary>
        public RelayCommand<ParamNameEntiry> ParamDetailCommand { get; private set; }

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
    }
}
