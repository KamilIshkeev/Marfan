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
using System.Globalization;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        Runner u;
        public User _user;

        // Основной конструктор, который работает с данными
        public EditUser(User user)
        {
            InitializeComponent();
            _user = user;
            LoadUserData();
        }

        // Второй конструктор, используемый при переходе через NavigationService
        public EditUser(MainWindow mainWindow, User user) : this(user)
        {
            _mainWindow = mainWindow;
        }

        private void LoadUserData()
        {
            var runner = db.Runner.FirstOrDefault(x => x.Email == _user.Email);
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

            Uri tickIcon = new Uri("C:/Users/222209/source/repos/Marfan/WpfMarathon/Pages/icons/tick-icon.png");
            Uri crossIcon = new Uri("C:/Users/222209/source/repos/Marfan/WpfMarathon/Pages/icons/cross-icon.png");

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

            txbEmail.Text = _user.Email;
            txbName.Text = _user.FirstName;
            txbSurName.Text = _user.LastName;
            txbGender.Text = u.Gender;
            txbDateOf.Text = u.DateOfBirth?.ToString("yyyy-MM-dd");
            txbCountry.Text = u.CountryCode;
            txbFund.Text = reg.Charity?.CharityName ?? "Не выбрано"; 
            txbMoney.Text = "$" + ((int?)reg.SponsorshipTarget ?? 0).ToString();

            var marathonNames = db.RegistrationEvent
                .Where(re => re.RegistrationId == reg.RegistrationId && re.Event != null)
                .Select(re => re.Event.EventType.EventTypeName)
                .Distinct()
                .ToList();

            txbDistance.Text = string.Join(", ", marathonNames);
        }

        

        private void btnEditProf_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AdminEditUser(_mainWindow, _user));
        }
    }
}
