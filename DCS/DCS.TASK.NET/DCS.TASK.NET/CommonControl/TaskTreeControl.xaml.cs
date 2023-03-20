using DCS.TASK.NET.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DCS.TASK.NET.CommonControl
{
    /// <summary>
    /// TaskTreeControl.xaml 的交互逻辑
    /// </summary>
    public partial class TaskTreeControl : UserControl
    {
        //注册依赖属性
        public static readonly DependencyProperty TreeItemSourcedependencyProperty = DependencyProperty.Register("TreeItemSource", typeof(ObservableCollection<BaseTreeViewModel>), typeof(TaskTreeControl));
        /// <summary>
        ///数据源
        /// </summary>
        public ObservableCollection<BaseTreeViewModel> TreeItemSource
        {
            set { SetValue(TreeItemSourcedependencyProperty, value); }
            get { return GetValue(TreeItemSourcedependencyProperty) as ObservableCollection<BaseTreeViewModel>; }  
        }

        public TaskTreeControl()
        {
            InitializeComponent();
        }
    }
}
