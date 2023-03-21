using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;

namespace CS.View.FrameControl
{
    /// <summary>
    /// MenuTree.xaml 的交互逻辑
    /// </summary>
    public partial class MenuTree : UserControl
    {
        /// <summary>
        /// 菜单命令
        /// </summary>
        private static readonly DependencyProperty CommandDependencyProperty = DependencyProperty.Register("Command", typeof(RelayCommand<object>), typeof(MenuTree));

        public RelayCommand<object> Command
        {
            set { SetValue(CommandDependencyProperty, value); }
            get { return GetValue(CommandDependencyProperty) as RelayCommand<object>; }
        }

        /// <summary>
        ///
        /// </summary>
        private static readonly DependencyProperty TreeItemDependencyProperty = DependencyProperty.Register("TreeItemSource", typeof(object), typeof(MenuTree));

        /// <summary>
        /// 菜单树节点数据
        /// </summary>
        public object TreeItemSource
        {
            set { SetValue(TreeItemDependencyProperty, value); }
            get { return GetValue(TreeItemDependencyProperty); }
        }

        public MenuTree()
        {
            InitializeComponent();
        }
    }
}