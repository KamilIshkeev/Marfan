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
using WpfMarathon.Pages;

namespace WpfMarathon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            MainFrame.NavigationService.Navigate(new MainPage(this));
            //DateTime dateTime = DateTime.Now;
            //DateTime dateMarafon = new DateTime(2025, 5, 30, 10, 0, 0, 0);
            //var dateTimeStart = dateMarafon - dateTime;
            //TimeStart.Text = $"{dateTimeStart.Days}-дней {dateTimeStart.Minutes}-минут {dateTimeStart.Seconds}-секунд до старта марафона!";
            countdown = new MarathonCountdown(UpdateCountdownText, marathonDate);
        }
        private MarathonCountdown countdown;
        private DateTime marathonDate = new DateTime(2025, 10, 20);
        private void UpdateCountdownText(string text)
        {
            TimeStart.Text = text;
        }

    }
}
