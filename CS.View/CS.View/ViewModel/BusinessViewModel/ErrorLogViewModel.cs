using CS.BLL.Business;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.Model.Entiry;
using CS.View.Common.Enum;
using CS.View.FrameControl;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;

namespace CS.View.ViewModel.BusinessViewModel
{
    internal class ErrorLogViewModel : DataProcess<ErrorLogEntity>
    {
        /// <summary>
        /// 下拉框选中的值
        /// </summary>
        private CombBoxItem _selectedItem;

        public CombBoxItem SelectedItem
        {
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged();
                }
            }
            get { return _selectedItem; }
        }

        /// <summary>
        /// 设置菜单命令
        /// </summary>
        public RelayCommand<RoleEntiry> SetRoleMenuCommand { get; private set; }

        /// <summary>
        ///下拉框数据
        /// </summary>
        public ObservableCollection<CombBoxItem> RoleCombBoxItems { get; private set; }

        /// <summary>
        /// 参数明细命令
        /// </summary>
        // public RelayCommand<ErrorLogEntity> ParamDetailCommand { get; private set; }

        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        readonly IErrorLogService _errorLogService;

        /// <summary>
        /// 参数明细
        /// </summary>
        readonly ErrorLogEntity _errorLogEntity;

        public ErrorLogViewModel(IErrorLogService errorLogService)
        {
            _errorLogService = errorLogService;
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Init()
        {
            base.Init();

            RoleCombBoxItems = new ObservableCollection<CombBoxItem>();
            //初始化下拉框
            RoleCombBoxItems.Add(new CombBoxItem() { Text = "普通用户", Value = 0 });
            RoleCombBoxItems.Add(new CombBoxItem() { Text = "系统管理员", Value = 1 });
            //关联命令
            SetRoleMenuCommand = new RelayCommand<RoleEntiry>(SetRoleMenu);
        }

        /// <summary>
        /// 设置菜单树命令
        /// </summary>
        /// <param name="model"></param>
        private async void SetRoleMenu(RoleEntiry model)
        {
            BaseMsgDialog baseMsgDialog = new BaseMsgDialog();

            //绑定viewModel
            baseMsgDialog.BindDataContex<SelectMenuWindow, SelectMenuViewModel>(new SelectMenuWindow(), new SelectMenuViewModel(model));
            //显示对话框
            await baseMsgDialog.ShowDialog();
        }

        /// <summary>
        /// 清空编辑框
        /// </summary>
        protected override void Reset()
        {
            QueryText = string.Empty;
            //QueryText1 = string.Empty;
            //QueryText2 = string.Empty;
            // ModuleCode = string.Empty;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        protected override void Add(ErrorLogEntity model)
        {
            //默认选择第一个
            SelectedItem = RoleCombBoxItems[0];
            base.Add(model);
        }

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="model"></param>
        protected override void Edit(ErrorLogEntity model)
        {
            //设置下拉框已经选择的值
            // SelectedItem = RoleCombBoxItems.FirstOrDefault(x => x.Value == model.IsManage);
            base.Edit(model);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
            //查询数据
            var result = await _errorLogService.GetErrorLogAsync(pageInex, PageSize, new ErrorLogCondition()
            {
                ErrorCode = QueryText,
                //CreateTimeStart = QueryText1,
                //CreateTimeEnd = QueryText2
                //ID = _batteryCoreOcvTestEntity.Id
            });

            //清除显示数据
            DataGridDatas.Clear();
            TotailCount = result.Total;
            //查询
            if (result.Total > 0)
            {
                //设置列表
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
                Model.Id = _errorLogEntity.Id;
                //添加数据
                result = await _errorLogService.AddErrorLogAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _errorLogService.UpdateErrorLogAsync(Model.Id.Value, Model);
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
        protected async override void Delete(ErrorLogEntity model)
        {
            bool result = await _errorLogService.DeteleErrorLogAsync(model.Id.Value);

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
