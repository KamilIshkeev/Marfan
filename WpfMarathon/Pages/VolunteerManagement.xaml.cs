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
    /// Логика взаимодействия для VolunteerManagement.xaml
    /// </summary>
    public partial class VolunteerManagement : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        List<string> sort = new List<string>()
        {
            "Имени",
            "Фамилии",
            "Стране",
            "Полу",
        };
        public VolunteerManagement(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            VolunteerDG.ItemsSource = db.Volunteer.ToList();
            cmbSort.ItemsSource = sort;
        }


        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnAddVol_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new ImportVolunteers(_mainWindow));
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSort.SelectedItem != null)
            {
                List<Volunteer> bdList = db.Volunteer.ToList();
                if (cmbSort.SelectedItem == "Имени")
                {
                    var sortByName = from b in bdList

                                     orderby b.FirstName
                                     select b;
                    VolunteerDG.ItemsSource = sortByName;
                }
                else if (cmbSort.SelectedItem == "Фамилии")
                {
                    var sortBySurName = from b in bdList
                                        orderby b.LastName
                                        select b;
                    VolunteerDG.ItemsSource = sortBySurName;
                }
                else if (cmbSort.SelectedItem == "Стране")
                {
                    var sortByCountry = from b in bdList
                                        orderby b.CountryCode
                                        select b;
                    VolunteerDG.ItemsSource = sortByCountry;
                }
                else if (cmbSort.SelectedItem == "Полу")
                {
                    var sortByGender = from b in bdList
                                       orderby b.Gender
                                       select b;
                    VolunteerDG.ItemsSource = sortByGender;
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }
    }
}
