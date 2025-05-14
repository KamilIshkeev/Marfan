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
    /// Логика взаимодействия для AuthRegRunner.xaml
    /// </summary>
    public partial class AuthRegRunner : Page
    {
        public AuthRegRunner(MainWindow _mainWindow)
        {
            InitializeComponent();
        }


        private void btn_oldrunner_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("AuthPage.xaml", UriKind.Relative));
        }

        private void btn_newrunner_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("RegBegunPage.xaml", UriKind.Relative));
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("AuthPage.xaml", UriKind.Relative));
        }

    }
}
