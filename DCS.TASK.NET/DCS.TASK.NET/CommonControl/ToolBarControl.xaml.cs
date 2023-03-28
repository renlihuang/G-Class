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
    /// ToolBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBarControl : UserControl
    {
        /// <summary>
        /// 注册依赖属性
        /// </summary>
        public static readonly DependencyProperty PopupMenusDependencyProperty  = DependencyProperty.Register("PopupMenus", typeof(ObservableCollection<PopupBoxViewModel>),typeof(ToolBarControl));
 
        public   ObservableCollection<PopupBoxViewModel> PopupMenus
        {
            set { SetValue(PopupMenusDependencyProperty, value); }
            get { return GetValue(PopupMenusDependencyProperty) as ObservableCollection<PopupBoxViewModel>; }
        }

        public ToolBarControl()
        {
            InitializeComponent();
        }
    }
}
