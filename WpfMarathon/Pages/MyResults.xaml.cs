using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для MyResults.xaml
    /// </summary>
    public partial class MyResults : Page
    {
        public static MarafonEntities db = new MarafonEntities();
        int id;
        static MainWindow _mainWindow;
        public MyResults(MainWindow mainWindow, int _id)
        {
            id = _id;
            InitializeComponent();
            _mainWindow = mainWindow;
            txt_gender.Text = db.Runner.Where(x => x.RunnerId == id).Select(x => x.Gender).SingleOrDefault().ToString();
            DateTime birth = (DateTime)db.Runner.Where(x => x.RunnerId == id).Select(x => x.DateOfBirth).SingleOrDefault();
            DateTime date = DateTime.Now;
            TimeSpan d = date - birth;
            if ((d.Days / 365) <= 18 && (d.Days / 365) > 27)
            {
                txt_age.Text = "18-27";
            }
            if ((d.Days / 365) <= 27 && (d.Days / 365) > 36)
            {
                txt_age.Text = "27-36";
            }
            if ((d.Days / 365) <= 36 && (d.Days / 365) > 49)
            {
                txt_age.Text = "36-49";
            }

            var query = db.RegistrationEvent
                    .Where(re => re.RaceTime != null); // Только финишировавшие

            // Загружаем в память с нужными полями
            var results = query
    .Select(re => new
    {
        MarathonName = re.Event.Marathon.MarathonName,
        EventTypeName = re.Event.EventType.EventTypeName,
        RaceTimeSeconds = re.RaceTime, // Получаем просто значение в секундах
        Country = re.Registration.Runner.CountryCode,
        DateOfBirth = re.Registration.Runner.DateOfBirth.Value.Year
    })
    .ToList() // Загружаем в память
    .Select(re => new
    {
        re.MarathonName,
        re.EventTypeName,
        RaceTime = re.RaceTimeSeconds.HasValue
                   ? TimeSpan.FromSeconds(re.RaceTimeSeconds.Value)
                   : (TimeSpan?)null,
        re.Country,
        DateOfBirth = DateTime.Now.Year - re.DateOfBirth
    })
    .ToList();


            grid_Results.ItemsSource = results.ToList();
        }

        private void btn_showallresults_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new PerviousResult(_mainWindow));
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
