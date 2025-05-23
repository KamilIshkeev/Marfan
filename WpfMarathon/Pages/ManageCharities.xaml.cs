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
using WpfMarathon.Base;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManageCharities.xaml
    /// </summary>
    public partial class ManageCharities : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        public ManageCharities(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            var query = db.Charity
                .ToList()
                .Select(hl => new Charity
                {
                    CharityLogo = $"C:/Users/222209/source/repos/Marfan/WpfMarathon/Images/{hl.CharityLogo}",
                    CharityName = hl.CharityName.ToString(),
                    CharityDescription = hl.CharityDescription.ToString()
                })
                .ToList();
            FundDb.ItemsSource = query.ToList();
        }

        private void btnAddFund_Click(object sender, RoutedEventArgs e)
        {

            _mainWindow.MainFrame.NavigationService.Navigate(new AddCharity(_mainWindow));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Charity fun = FundDb.SelectedValue as Charity;

            _mainWindow.MainFrame.NavigationService.Navigate(new EditCharity(_mainWindow, fun));

        }
        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
