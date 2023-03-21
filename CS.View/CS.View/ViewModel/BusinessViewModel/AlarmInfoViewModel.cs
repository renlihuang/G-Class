using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common.Enum;
using CS.View.Common;
using CS.View.ViewModel.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using GalaSoft.MvvmLight.Command;
using CS.IBLL;
using CS.Model.Entiry;
using System.Collections.ObjectModel;
using CS.View.FrameControl;
using CS.Model.QueryCondition;

namespace CS.View.ViewModel.BusinessViewModel
{
    class AlarmInfoViewModel : DataProcess<ErrorLogEntity>
    {
        /// <summary>
        /// 查询文本
        /// </summary>
        private string _queryText = string.Empty;

        public string QueryText
        {
            set { _queryText = value; RaisePropertyChanged(); }
            get { return _queryText; }
        }

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
        /// 工艺路线接口
        /// </summary>
        private readonly IErrorLogService _errorLogService;
        /// <summary>
        /// 参数明细
        /// </summary>
        readonly ErrorLogEntity _errorLogEntity;
        public AlarmInfoViewModel(IErrorLogService errorLogService)
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
            //调用API获取数据
            var queryResult = await _errorLogService.GetErrorLogAsync(pageInex, PageSize, new ErrorLogCondition() { ErrorCode = QueryText });
            //清除之前的数据
            DataGridDatas.Clear();
            //设置页数
            TotailCount = queryResult.Total;
            //查询
            if (queryResult.Total > 0)
            {
                //设置列表
                foreach (var item in queryResult.Data)
                {
                    DataGridDatas.Add(item);
                }
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="model"></param>
        protected async override void Delete(ErrorLogEntity model)
        {
            bool result = await SkinMessageBox.Question("确认删除该条数据?");
            if (result)
            {
                //调用接口删除数据
                if (await _errorLogService.DeteleErrorLogAsync(model.Id.Value))
                {
                    base.Delete(model);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        protected async override void Save()
        {
            bool result = false;

            if (Mode == ActionMode.Add)
            {
                // Model.Id = _errorLogEntity.Id;
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

        protected override void Reset()
        {
            QueryText = string.Empty;
        }
    }
}
