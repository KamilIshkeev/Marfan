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
            txt_gender.Text = db.Runner.Where(x => x.RunnerId == id).Select(x => x.Gender).ToString();
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
            grid_Results.ItemsSource = db.RegistrationEvent.Where(x => x.RegistrationId == id).ToList();
        }

        private void btn_showallresults_Click(object sender, RoutedEventArgs e)
        {
            PerviousResult pr = new PerviousResult();
            NavigationService.Navigate(pr);
        }

    }
}
