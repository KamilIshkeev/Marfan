using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        private MainWindow mainWindow;
        Runner u;
        private User user;

        // Основной конструктор, который работает с данными
        public EditUser(User user)
        {
            InitializeComponent();
            this.user = user;
            LoadUserData();
        }

        // Второй конструктор, используемый при переходе через NavigationService
        public EditUser(MainWindow mainWindow, User user) : this(user)
        {
            this.mainWindow = mainWindow;
        }

        private void LoadUserData()
        {
            var runner = db.Runner.FirstOrDefault(x => x.Email == user.Email);
            if (runner == null)
            {
                MessageBox.Show("Бегун не найден.");
                return;
            }

            u = runner;
            var reg = db.Registration.FirstOrDefault(x => x.RunnerId == u.RunnerId);

            if (reg == null)
            {
                MessageBox.Show("Регистрация не найдена.");
                return;
            }

            Uri tickIcon = new Uri($"pack://application:,,,/WpfMarathon;component/Resources/tick-icon.png");
            Uri crossIcon = new Uri($"pack://application:,,,/WpfMarathon;component/Resources/cross-icon.png");

            imgReg.Source = new BitmapImage(tickIcon); // Зарегистрирован всегда true?

            if (reg.SponsorshipTarget > 0)
            {
                imgPayment.Source = new BitmapImage(tickIcon);
            }
            else
            {
                imgPayment.Source = new BitmapImage(crossIcon);
            }

            imgInv.Source = new BitmapImage(crossIcon); // Пример
            imgStart.Source = new BitmapImage(crossIcon); // Пример

            txbEmail.Text = user.Email;
            txbName.Text = user.FirstName;
            txbSurName.Text = user.LastName;
            txbGender.Text = u.Gender;
            txbDateOf.Text = u.DateOfBirth?.ToString("yyyy-MM-dd");
            txbCountry.Text = u.CountryCode;
            txbFund.Text = reg.Charity?.CharityName ?? "Не выбрано";
            txbMoney.Text = "$" + (reg.SponsorshipTarget ?? 0).ToString();

            var marathonNames = db.RegistrationEvent
                .Where(re => re.RegistrationId == reg.RegistrationId && re.Event != null)
                .Select(re => re.Event.Marathon.MarathonName)
                .Distinct()
                .ToList();

            txbDistance.Text = string.Join(", ", marathonNames);
        }

        private void btnShowSerf_Click(object sender, RoutedEventArgs e)
        {
            // Реализация показа сертификата
        }

        private void btnPrintB_Click(object sender, RoutedEventArgs e)
        {
            // Реализация печати бейджа
        }

        private void btnEditProf_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AdminEditUser(_mainWindow, user));
        }
    }
}
