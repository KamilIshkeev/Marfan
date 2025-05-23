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
        static MainWindow _mainWindow;
        public AuthRegRunner(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }


        private void btn_oldrunner_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void btn_newrunner_Click(object sender, RoutedEventArgs e)
        {

            _mainWindow.MainFrame.NavigationService.Navigate(new RegBegunPage(_mainWindow));
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }

    }
}
