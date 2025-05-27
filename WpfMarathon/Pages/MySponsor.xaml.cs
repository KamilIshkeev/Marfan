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
    /// Логика взаимодействия для MySponsor.xaml
    /// </summary>
    public partial class MySponsor : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        int id;
        private MainWindow mainWindow;

        public MySponsor(MainWindow mainWindow, int _id)
        {
            id = _id;
            InitializeComponent();
            _mainWindow = mainWindow;
            var user = db.Runner.Where(x => x.RunnerId == id).SingleOrDefault();
            var reg = db.Registration.Where(x=> x.RunnerId == id).SingleOrDefault();
            var logo = db.Charity.Where(x => x.CharityId == reg.CharityId).Select(x => x.CharityLogo).SingleOrDefault();
            if (logo != null)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri($"C:/Users/222209/source/repos/Marfan/WpfMarathon/Images/{logo}"));
                Logo.Source = bitmapImage;
                txt_fund.Text = db.Charity.Where(x => x.CharityId == reg.CharityId).Select(x => x.CharityName).SingleOrDefault();
                txt_fund_Copy.Text = "";
                var a1 = db.Registration.Where(x => x.CharityId == reg.CharityId).Select(x => x.SponsorshipTarget).ToList();
                int sum = 0;
                foreach (var a in a1)
                {
                    sum += (int)a;
                }
                txt_fund_Copy.Text = Convert.ToString(sum);
                if (reg.RegistrationId == 0)
                {
                    MessageBox.Show("Нет спонсора", "Ошибка сервера");
                }
                else
                {
                    grid_sponsor.ItemsSource = db.Sponsorship.Where(x => x.RegistrationId == reg.RegistrationId).Select(x => new { SponsorName = x.SponsorName, SponsorshipTarget = x.Registration.SponsorshipTarget }).ToList();
                }

            }
            else
            {
                MessageBox.Show("У вас нет спонсора", "Ошибка");
            }

        }

        public MySponsor(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }
    }
}
