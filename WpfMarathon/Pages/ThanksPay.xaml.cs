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
    /// Логика взаимодействия для ThanksPay.xaml
    /// </summary>
    public partial class ThanksPay : Page
    {
        static MainWindow _mainWindow;
        public string S { get; }
        public ThanksPay(MainWindow mainWindow, string s, string fund, string user)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            S = s;
            txt_price.Text = S;
            txt_fund.Text = fund;
            txbUser.Text = user;
        }

        
        
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
