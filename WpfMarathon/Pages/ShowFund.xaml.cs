

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
    /// Логика взаимодействия для ShowFund.xaml
    /// </summary>
    public partial class ShowFund : Page
    {
        static MainWindow _mainWindow; 
        private List<CharityViewModel> _allCharityData;
        private bool _isAscending = true; // Переключатель направления сортировки
        public static MarafonEntities db = new MarafonEntities();
        public ShowFund(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            List<CharityViewModel> data = db.Charity
    .Select(c => new {
        ChartyLogoFromDB = c.CharityLogo,
        ChartyNameFromDB = c.CharityName,
        ChartySumFromDB = c.Registration
            .Where(r => r.SponsorshipTarget != null)
            .Sum(r => (decimal?)r.SponsorshipTarget) // Явно приводим к decimal?
    })
    .ToList()
    .Select(c => new CharityViewModel
    {
        ChartyLogo = $"C:/Users/222209/source/repos/Marfan/WpfMarathon/Images/{c.ChartyLogoFromDB}",
        ChartyName = c.ChartyNameFromDB,
        ChartySum = c.ChartySumFromDB ?? 0 // Можно заменить на 0, если сумма отсутствует
    })
    .ToList();

            _allCharityData = data;
            FundDb.ItemsSource = _allCharityData;

            var totalCount = db.Charity.Count();
            var totaMoney = db.Registration.Select(x => x.SponsorshipTarget).Sum();

            txbCount.Text = totalCount.ToString();
            txbMoney.Text = "$" + totaMoney.ToString();
        

        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }
        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_sort_Click(object sender, RoutedEventArgs e)
        {
            if (_allCharityData == null || !_allCharityData.Any()) return;

            // Получаем фильтр по сумме
            decimal minSum = 0;
            if (!decimal.TryParse(txt_sum.Text, out minSum))
            {
                MessageBox.Show("Введите корректное числовое значение в поле 'Итоговая сумма'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Фильтруем данные
            var filteredList = _allCharityData.Where(d => d.ChartySum >= minSum).ToList();

            // Меняем направление сортировки
            _isAscending = !_isAscending;

            // Сортируем отфильтрованный список
            var sortedList = _isAscending
                ? filteredList.OrderByDescending(d => d.ChartySum).ToList()
                : filteredList.OrderBy(d => d.ChartySum).ToList();

            // Обновляем DataGrid
            FundDb.ItemsSource = sortedList;

            // Меняем текст кнопки для наглядности
            btn_sort.Content = _isAscending ? "Сортировка ⬆" : "Сортировка ⬇";
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            txt_sum.Text = "";
            FundDb.ItemsSource = _allCharityData;
            btn_sort.Content = "Сортировка";
            _isAscending = true;
        }
    }

    public class CharityViewModel
    {
        public string ChartyLogo { get; set; }
        public string ChartyName { get; set; }
        public decimal? ChartySum { get; set; } // Поддерживает null
    }
}
