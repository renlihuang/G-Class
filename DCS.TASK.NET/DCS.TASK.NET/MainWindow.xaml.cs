using DCS.TASK.NET.ViewModel.MainView;
using MESwebservice.Mesini;
using System;
using System.Collections.Generic;
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

namespace DCS.TASK.NET
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //绑定viewModel
            this.DataContext = new MainViewModel(this);

            InitializeComponent();
            MesiniHelper.InitAllmesset();
        }
    }
}
