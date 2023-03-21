using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CS.View.View.BusinessView
{
    /// <summary>
    /// ModuleTypeView.xaml 的交互逻辑
    /// </summary>
    public partial class ModuleTypeView : UserControl
    {
        Regex _regex = new Regex("[^0-9.-]+");

        public ModuleTypeView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
