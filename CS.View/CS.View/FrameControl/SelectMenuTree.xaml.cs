using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;

namespace CS.View.FrameControl
{
    /// <summary>
    /// SelectMenuTree.xaml 的交互逻辑
    /// </summary>
    public partial class SelectMenuTree : UserControl
    {
        /// <summary>
        /// 菜单命令
        /// </summary>
        private static readonly DependencyProperty CommandDependencyProperty = DependencyProperty.Register("ButtonCommand", typeof(RelayCommand<object>), typeof(SelectMenuTree));

        public RelayCommand<object> ButtonCommand
        {
            set { SetValue(CommandDependencyProperty, value); }
            get { return GetValue(CommandDependencyProperty) as RelayCommand<object>; }
        }

        /// <summary>
        ///
        /// </summary>
        private static readonly DependencyProperty MenuItemDependencyProperty = DependencyProperty.Register("MenuItemSource", typeof(object), typeof(SelectMenuTree));

        /// <summary>
        /// 菜单树节点数据
        /// </summary>
        public object MenuItemSource
        {
            set { SetValue(MenuItemDependencyProperty, value); }
            get { return GetValue(MenuItemDependencyProperty); }
        }

        public SelectMenuTree()
        {
            InitializeComponent();
        }
    }
}