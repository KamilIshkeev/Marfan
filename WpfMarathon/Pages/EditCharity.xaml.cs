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
using Path = System.IO.Path;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditCharity.xaml
    /// </summary>
    public partial class EditCharity : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        int id = 0;
        private MainWindow mainWindow;

        public EditCharity(MainWindow mainWindow, Charity fund)
        {
            InitializeComponent();
            txbName.Text = fund.CharityName;
            txbDisc.Text = fund.CharityDescription;
            id = fund.CharityId;
            if (fund.CharityLogo != null)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(fund.CharityLogo));
                imgLogo.Source = bitmap;
                //using (var stream = new MemoryStream(fund.Logo))
                //{
                //    var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                //    imgLogo.Source = decoder.Frames[0];
                //}
            }
        }

        public EditCharity(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }

        private void txbName_GotFocus(object sender, RoutedEventArgs e)
        {
            txbName.Text = "";
            txbName.Foreground = Brushes.Black;
        }
        private void txbDisc_GotFocus(object sender, RoutedEventArgs e)
        {
            txbDisc.Text = "";
            txbDisc.Foreground = Brushes.Black;
        }

        private void btn_Reg_Click(object sender, RoutedEventArgs e)
        {
            if (txbName.Text != "" && txbDisc.Text != "")
            {
                foreach (Charity fun in db.Charity)
                {
                    if (fun.CharityId == id)
                    {
                        fun.CharityName = txbName.Text;
                        fun.CharityDescription = txbDisc.Text;
                        if (imgLogo.Source != null)
                        {
                            fun.CharityLogo = txbFile.Text;
                        }
                    }
                }

                db.SaveChanges();
                this.NavigationService.Navigate(new Uri("Admin/ManageCharities.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Поля не заполнены", "Ошибка");
            }
        }
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new ManageCharities(_mainWindow));
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog open = new OpenFileDialog();
            //open.FilterIndex = 2;
            //open.Filter = "jpg|*.jpg|  png|*.png";

            //if (open.ShowDialog() == true)
            //{
            //    BitmapImage source = new BitmapImage();
            //    source.BeginInit(); // начало считывания фото
            //    source.UriSource = new Uri(@"" + open.FileName, UriKind.Relative);
            //    source.CacheOption = BitmapCacheOption.OnLoad; //Задержка
            //    source.EndInit();

            //    txbFile.Text = source.ToString();
            //    imgLogo.Source = source;
            //}

            // Открытие диалога для выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.PNG;*.JPG;*.JPEG;*.BMP)|*.PNG;*.JPG;*.JPEG;*.BMP|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourceFilePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(sourceFilePath);
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string targetDirectory = Path.Combine(projectDirectory, "Images");
                string targetFilePath = Path.Combine(targetDirectory, fileName);

                try
                {
                    // Проверяем и создаём папку Images, если её нет
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }

                    // Копируем файл
                    File.Copy(sourceFilePath, targetFilePath, overwrite: true);

                    // Обновляем TextBox
                    txbFile.Text = fileName;

                    // Загружаем изображение в Image элемент
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(targetFilePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imgLogo.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
