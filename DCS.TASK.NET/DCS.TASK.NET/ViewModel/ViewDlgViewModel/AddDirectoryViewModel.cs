using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DCS.TASK.NET.ViewModel.ViewDlgViewModel
{
    internal class AddDirectoryViewModel : BaseDIalogViewModel
    {
        public AddDirectoryViewModel(Window ownerWindow) : base(ownerWindow)
        {
            this.Title = "任务目录";
        }

        /// <summary>
        /// 
        /// </summary>
        private string _directoryName;
        public string DirectoryName
        {
            set { _directoryName = value; }
            get { return _directoryName; }
        }
  

        /// <summary>
        /// 
        /// </summary>
        protected override void OnConfrim()
        {
            if (string.IsNullOrEmpty(DirectoryName))
            {
                HintText = "任务目录名不能为空";
                return;
            }

            DialogResult = true;

            base.OnConfrim();
        }
    }
}
