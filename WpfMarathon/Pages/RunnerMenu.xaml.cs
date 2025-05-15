using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfMarathon.Base;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для RunnerMenu.xaml
    /// </summary>
    public partial class RunnerMenu : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();

        string email;
        int _id;
        public RunnerMenu(MainWindow mainWindow, string email1)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            this.email = email1;
            Runner runner = new Runner();
            runner.Email = email;
            _id = runner.RunnerId;
        }


        private void btn_regmarathon_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new RegMarafonPage(_mainWindow, _id));
        }

        private void btn_myresult_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new MyResults(_mainWindow, _id));
        }

        private void btn_editprofile_Click(object sender, RoutedEventArgs e)
        {
            //List<User> user = db.User.ToList();

            //EditProfile ep = new EditProfile(user.Where(x => x.ID == id).ToList());
            //this.NavigationService.Navigate(ep);
        }

        private void btn_sponsor_Click(object sender, RoutedEventArgs e)
        {
            //MySponsor rg = new MySponsor(_id);
            //NavigationService.Navigate(rg);
        }

        private void btn_contact_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new ContactCoordinator(_mainWindow));
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
