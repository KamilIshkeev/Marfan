using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для RegConfirm.xaml
    /// </summary>
    public partial class RegConfirm : Page
    {
        static MainWindow _mainWindow;

        public static MarafonEntities db = new MarafonEntities();
        string Email;
        public RegConfirm(MainWindow mainWindow, int id)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            var run = db.Runner.FirstOrDefaultAsync(r => r.RunnerId == id);
            Email = run.Result.Email;
        }


        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new RunnerMenu(_mainWindow, Email));
        }
        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

    }
}
