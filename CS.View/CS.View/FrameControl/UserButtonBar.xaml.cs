using System.Windows;
using System.Windows.Controls;

namespace CS.View.FrameControl
{
    /// <summary>
    /// UserButtonBar.xaml 的交互逻辑
    /// </summary>
    public partial class UserButtonBar : UserControl
    {
        public object ItemListSource
        {
            set { SetValue(ItemListDP, value); }
            get { return GetValue(ItemListDP); }
        }

        public static readonly DependencyProperty ItemListDP = DependencyProperty.Register("ItemListSource", typeof(object), typeof(UserButtonBar));

        public UserButtonBar()
        {
            InitializeComponent();
        }
    }
}