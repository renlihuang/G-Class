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
    class ParamValueViewModel : DataProcess<ParamValueEntiry>
    {
        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        readonly IParamValueService _paramValueService;

        /// <summary>
        /// 参数明细
        /// </summary>
        readonly ParamNameEntiry _paramNameEntiry;

        public ParamValueViewModel(ParamNameEntiry paramNameEntiry)
        {
            //参数名信息
            _paramNameEntiry = paramNameEntiry;
            //获取参数管理接口
            _paramValueService = ServiceProviderInstacnce.GetService<IParamValueService>();
            //初始化
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
        /// 查询数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            //查询数据
            var result = await _paramValueService.GetParamValuesAsync(pageInex, PageSize, new ParamValueCondition()
            { 
                Name = QueryText,
                ParentID = _paramNameEntiry.Id
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
                Model.ParentID = _paramNameEntiry.Id;
                //添加数据
                result = await _paramValueService.AddParamValueAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _paramValueService.UpdateParamValueAsync(Model.Id, Model);
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
        protected async override void Delete(ParamValueEntiry model)
        {
            bool result = await _paramValueService.DeteleParamValueAsync(model.Id);

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
    }
}
