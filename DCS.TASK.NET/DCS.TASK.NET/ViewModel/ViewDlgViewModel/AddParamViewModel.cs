using DCS.TASK.NET.ViewModel.Base;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DCS.TASK.NET.ViewModel.ViewDlgViewModel
{


    internal class AddParamViewModel : BaseDIalogViewModel
    {
        public AddParamViewModel(Window ownerWindow) : base(ownerWindow)
        {
            this.Title = "参数编辑";
            //关联命令
            AddParamCommand = new RelayCommand(AddParam);
            EditParamCommand = new RelayCommand<object>(EditParam);
            DeleteParamCommand = new RelayCommand<object>(DeleteParam);
            //要显示的参数列表
            DataGridItems = new ObservableCollection<ParamItem>();
        }

        /// <summary>
        /// 系统加载完成
        /// </summary>
        protected override void OnStartup()
        {
            foreach (var item in _paramItems)
            {
                DataGridItems.Add(new ParamItem()
                {
                    Name = item.Key,
                    Value = item.Value
                });
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="items"></param>
        public void SetParams(IDictionary<string, string> items)
        {
            _paramItems = items;
        }

        /// <summary>
        /// 提交修改
        /// </summary>
        protected override void OnConfrim()
        {
            if (_operatorType == OperatorType.add ||
                _operatorType == OperatorType.edit)
            {
                if (string.IsNullOrEmpty(ParamModel.Name))
                {
                    HintText = "参数名不能空";
                    return;
                }

                if (string.IsNullOrEmpty(ParamModel.Value))
                {
                    HintText = "参数值不能空";
                    return;
                }

                var paramItem = DataGridItems.FirstOrDefault(x => x.Name == ParamModel.Name);

                //添加模式
                if (_operatorType == OperatorType.add)
                {
                    if (paramItem != null)
                    {
                        HintText = "无法添加相同的参数名";
                        return;
                    }

                    DataGridItems.Add(new ParamItem
                    {
                        Name = ParamModel.Name,
                        Value = ParamModel.Value
                    });
                }
                
                else
                {
                    //更新参数
                    paramItem.Value = ParamModel.Value;
                }
                //清空提示文本
                HintText = String.Empty;

                _operatorType = OperatorType.none;

                //回到主页
                TabPageIndex = 0;
            }
            else
            {
                _paramItems.Clear();
                //循环添加数据
                foreach (var item in DataGridItems)
                {
                    _paramItems[item.Name] = item.Value;
                }
                //关闭对话框
                base.OnConfrim();
            }
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        protected override void OnCancel()
        {
            if (TabPageIndex == 1)
            {
                if (!string.IsNullOrEmpty(HintText))
                {
                    HintText = string.Empty;
                }
                //回到主页
                TabPageIndex = 0;
                //设置为无操作
                _operatorType = OperatorType.none;
            }
            else
            {
                base.OnCancel();
            }
        }




        /// <summary>
        /// 添加参数
        /// </summary>
        private void AddParam()
        {
            _operatorType = OperatorType.add;
            //创建模型
            ParamModel = new ParamEntiry();
            //跳转到编辑页
            TabPageIndex = 1;
            //允许编辑
            ParamEditIsReadOnly = false;
        }



        /// <summary>
        /// 编辑参数
        /// </summary>
        private void EditParam(object item)
        {
            _operatorType = OperatorType.edit;
            //参数
            var paramItem = (ParamItem)item;

            //创建模型
            ParamModel = new ParamEntiry()
            {
                Name = paramItem.Name,
                Value = paramItem.Value
            };

            //允许编辑
            ParamEditIsReadOnly = true;
            //切换到页
            TabPageIndex = 1;
        }

        /// <summary>
        /// 删除参数
        /// </summary>
        private void DeleteParam(object item)
        {
            //参数
            var paramItem = (ParamItem)item;
            //移除数据
            if (DataGridItems.Contains(paramItem))
            {
                DataGridItems.Remove(paramItem);
            }
        }

        /// <summary>
        /// 是否只都
        /// </summary>
        private bool _paramEditIsReadOnly;
        public bool ParamEditIsReadOnly
        {
            set
            {
                if (_paramEditIsReadOnly != value)
                {
                    _paramEditIsReadOnly = value;
                    RaisePropertyChanged();
                }
            }

            get
            { 
               return _paramEditIsReadOnly;
            }
        }

        /// <summary>
        /// tab页切换
        /// </summary>
        private int _tabPageIndex;

        public int TabPageIndex
        {
            set 
            {
                if (_tabPageIndex != value)
                {
                    _tabPageIndex = value;
                    RaisePropertyChanged();
                }
            }
            get { return _tabPageIndex; }   
        }

        /// <summary>
        /// 参数字典接口
        /// </summary>
        IDictionary<string, string> _paramItems;

        /// <summary>
        /// 操作模式
        /// </summary>
        private OperatorType _operatorType;

        /// <summary>
        /// 编辑参数
        /// </summary>
        ParamEntiry _paramModel;

        public ParamEntiry ParamModel
        {
            set { _paramModel = value;RaisePropertyChanged();}
            get { return _paramModel; }
        }

        /// <summary>
        /// 添加参数命令
        /// </summary>
        public RelayCommand AddParamCommand { private set; get; }

        /// <summary>
        /// 编辑参数
        /// </summary>
        public RelayCommand<object> EditParamCommand { private set; get; }

        /// <summary>
        /// 删除参数
        /// </summary>
        public RelayCommand<object> DeleteParamCommand { private set; get; }

        /// <summary>
        /// 列表数据
        /// </summary>
        public ObservableCollection<ParamItem> DataGridItems { private set; get; }
    }

    /// <summary>
    ///操作模式
    /// </summary>
    enum OperatorType
    { 
        none,
        add,
        edit
    }


    class ParamEntiry
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    class ParamItem : ViewModelBase
    {
        /// <summary>
        /// 参数名
        /// </summary>
        private string _name;
        public string Name
        {
            set { _name = value; RaisePropertyChanged(); }
            get { return _name; }
        }
        /// <summary>
        /// 参数值
        /// </summary>
        private string _value;
        public string Value
        {
            set { _value = value; RaisePropertyChanged(); }
            get { return _value; }
        }
    }
}
