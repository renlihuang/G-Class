using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DCS.TASK.NET.ViewModel.ViewDlgViewModel
{
    internal class AddGroupNameDlgViewModel : BaseDIalogViewModel
    {
        public AddGroupNameDlgViewModel(Window ownerWindow) : base(ownerWindow)
        {
            this.Title = "周期任务组";
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnConfrim()
        {
            if (string.IsNullOrEmpty(GroupName))
            {
                HintText = "任务组名称不能为空";
                return;
            }

            //添加成功
            this.DialogResult = true;

            base.OnConfrim();
        }


        /// <summary>
        /// 任务组名称
        /// </summary>
        private string _groupName;

        public string GroupName
        {
            set { _groupName = value; }
            get { return _groupName; }
        }
    }
}
