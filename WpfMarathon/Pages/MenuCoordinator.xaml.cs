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

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuCoordinator.xaml
    /// </summary>
    public partial class MenuCoordinator : Page
    {
        static MainWindow _mainWindow;
        public MenuCoordinator(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void btnFund_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("Coordinator/ShowFund.xaml", UriKind.Relative));
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("Coordinator/ManagRunner.xaml", UriKind.Relative));
        }
    }
}
