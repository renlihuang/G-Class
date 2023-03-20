using DCS.TASK.NET.Common;
using DCS.TASK.NET.ViewModel.Base;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DCS.TASK.NET.ViewModel.ViewDlgViewModel
{
    class ComboxItemViewModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }


    internal class AddTaskViewModel : BaseDIalogViewModel
    {
        public AddTaskViewModel(Window ownerWindow) : base(ownerWindow)
        {
            ComboxItems = new ObservableCollection<ComboxItemViewModel>();
            //添加默认选项
            ComboxItems.Add(new ComboxItemViewModel() { Name = "请选择实现类" });
            //添加
            var classNames = Global.GlobalAssemblyManage.ClassNames;
            //循环添加实现类
            foreach (var className in classNames)
            {
                ComboxItems.Add(new ComboxItemViewModel() { Name = className });
            }
            //得到运行周期
            TaskCycle = Interval.ToString();
        }

        /// <summary>
        /// 窗体加载完成
        /// </summary>
        protected override void OnStartup()
        {
            //设置周期
            TaskCycle = Interval.ToString();
            //默认选中第一个
            SelectItem = ComboxItems[0];
            //设置对应实现类
            if (!string.IsNullOrEmpty(ClassName))
            {
                //查找类名对应的选中项
                var item = ComboxItems.FirstOrDefault(x => x.Name == ClassName);
                //
                if (item != null)
                {
                    SelectItem = item;
                }
            }
        }

        /// <summary>
        /// 提交按钮事件
        /// </summary>
        protected override void OnConfrim()
        {
            if (string.IsNullOrEmpty(TaskName))
            {
                HintText = "任务名不能空";
                return;
            }


            if (string.IsNullOrEmpty(TaskCycle))
            {
                HintText = "请输入任务运行周期";
                return;
            }
            //选中第一个
            if (SelectItem == ComboxItems[0])
            {
                HintText = "请选择任务对应的实现类";
                return;
            }

            //转换为毫秒
            Interval = Convert.ToInt32(TaskCycle);

            //关联实现类名
            ClassName = SelectItem.Name;
            //对话框返回值
            DialogResult = true;

            base.OnConfrim();
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        private string _taskName;
        public string TaskName
        {
            set { _taskName = value; }
            get { return _taskName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Interval { set; get; } = 200;

        /// <summary>
        /// 周期
        /// </summary>
        private string _taskCycle;
        public string TaskCycle
        {
            set { _taskCycle = value; }
            get { return _taskCycle; }
        }

        /// <summary>
        /// 下拉框当前选择值
        /// </summary>
        private ComboxItemViewModel _selectItem;

        public ComboxItemViewModel SelectItem
        {
            set 
            {
                if (_selectItem != value)
                {
                    _selectItem = value;
                    RaisePropertyChanged();
                }
          
            }
            get { return _selectItem; }
        }


        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { set; get;}

        /// <summary>
        /// 下拉框数据
        /// </summary>
        public ObservableCollection<ComboxItemViewModel> ComboxItems { private set; get;}
    }
}
