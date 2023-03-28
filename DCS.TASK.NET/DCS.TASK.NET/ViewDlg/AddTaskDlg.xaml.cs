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
using System.Windows.Shapes;

namespace DCS.TASK.NET.ViewDlg
{
    /// <summary>
    /// AddTaskDlg.xaml 的交互逻辑
    /// </summary>
    public partial class AddTaskDlg : Window
    {
        /// <summary>
        /// 创建正则表达式
        /// </summary>
        Regex _regex = new Regex("[^0-9.-]+");

        public AddTaskDlg()
        {
            InitializeComponent();
        }

        //键盘消息
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //不是字符不让输入
            e.Handled = _regex.IsMatch(e.Text);
        }
    }
}
