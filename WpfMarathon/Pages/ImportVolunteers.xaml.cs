using Microsoft.Win32;
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
    /// Логика взаимодействия для ImportVolunteers.xaml
    /// </summary>
    public partial class ImportVolunteers : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        List<VolunteerClas> vol2 = new List<VolunteerClas>();
        public ImportVolunteers(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void btnAddInBd_Click(object sender, RoutedEventArgs e)
        {
            Volunteer volbd = new Volunteer();
            foreach (var vol in vol2)
            {
                volbd.CountryCode = vol.CountryCode;
                volbd.FirstName = vol.FirstName;
                volbd.LastName = vol.LastName;
                volbd.Gender = vol.Gender;
            }

            db.Volunteer.Add(volbd);
            db.SaveChanges();



            _mainWindow.MainFrame.NavigationService.Navigate(new VolunteerManagement(_mainWindow));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.FilterIndex = 1;
            open.Filter = "csv|*.csv";

            if (open.ShowDialog() == true)
            {
                var lines = File.ReadAllLines(open.FileName);

                var data = from l in lines.Skip(1)
                           let split = l.Split(',')
                           select new VolunteerClas
                           {
                               FirstName = split[0],
                               LastName = split[1],
                               CountryCode = split[2],
                               Gender = split[3]
                           };
                vol2 = data.ToList();


                txbFilePath.Text = open.FileName.ToString();
            }
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

    }

    




}
