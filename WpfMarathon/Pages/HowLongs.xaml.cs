using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для HowLongs.xaml
    /// </summary>
    public partial class HowLongs : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        public HowLongs(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            // Для gridDistance
            var query = db.HowLong
                .Where(hl => hl.Speed == null)
                .ToList()
                .Select(hl => new DistanceItem
                {
                    ID = hl.ID,
                    Image = $"C:/Users/222209/source/repos/Marfan/WpfMarathon/Images/{hl.Image}",
                    Length = hl.Length?.ToString() + " м"
                })
                .ToList();

            gridDistance.ItemsSource = query;

            // Для gridSpeed
            var query1 = db.HowLong
                .Where(hl => hl.Length == null)
                .ToList()
                .Select(hl => new SpeedItem
                {
                    ID = hl.ID,
                    Image = $"C:/Users/222209/source/repos/Marfan/WpfMarathon/Images/{hl.Image}",
                    Speed = hl.Speed?.ToString() + " км/ч"
                })
                .ToList();

            gridSpeed.ItemsSource = query1;
        }

        private void gridDistance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = gridDistance.SelectedItem as DistanceItem;
            if (item != null)
            {
                // Получаем оригинальную запись из БД по ID, если нужно
                var element = db.HowLong.FirstOrDefault(hl => hl.ID == item.ID);
                if (element != null)
                {
                    txtInfo.Text = $"{element.Length} м";
                    txtName.Text = element.Name;
                    BitmapImage bitmapImage = new BitmapImage(new Uri(item.Image));
                    imgInfo.Source = bitmapImage;
                }
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали", "Ошибка");
            }
        }

        private void gridSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = gridSpeed.SelectedItem as SpeedItem;
            if (item != null)
            {
                // Получаем оригинальную запись из БД по ID, если нужно
                var element = db.HowLong.FirstOrDefault(hl => hl.ID == item.ID);
                if (element != null)
                {
                    txtInfo.Text = $"{element.Speed} км/ч";
                    txtName.Text = element.Name;
                    BitmapImage bitmapImage = new BitmapImage(new Uri(item.Image));
                    imgInfo.Source = bitmapImage;
                }
            }
            else
            {
                MessageBox.Show("Вы ничего не выбрали", "Ошибка");
            }
        }
    }

    public class DistanceItem
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Length { get; set; }
    }

    public class SpeedItem
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Speed { get; set; }
    }

}
