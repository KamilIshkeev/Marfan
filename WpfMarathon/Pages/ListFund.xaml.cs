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
    /// Логика взаимодействия для ListFund.xaml
    /// </summary>
    public partial class ListFund : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        public ListFund(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            var query = db.Charity
                .ToList()
                .Select(hl => new CharityItem
                {
                    CharityLogo = $"C:/Users/222209/source/repos/Marfan/WpfMarathon/Images/{hl.CharityLogo}",
                    CharityName = hl.CharityName,
                    CharityDescription = hl.CharityDescription
                })
                .ToList();
            FundDb.ItemsSource = query;
        }
    }
    public class CharityItem
    {
        public string CharityLogo { get; set; }
        public string CharityName { get; set; }
        public string CharityDescription { get; set; }
    }
}
