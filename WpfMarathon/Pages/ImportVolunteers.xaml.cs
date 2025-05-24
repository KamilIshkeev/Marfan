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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }

        private void btnAddInBd_Click(object sender, RoutedEventArgs e)
        {
            List<string> errorMessages = new List<string>();

            foreach (var vol in vol2)
            {
                // Проверка длины CountryCode
                if (vol.CountryCode.Length != 3)
                {
                    errorMessages.Add($"Некорректный код страны: {vol.CountryCode} для волонтёра {vol.FirstName} {vol.LastName}. Код должен быть длиной 2 символа.");
                    continue;
                }

                // Проверка обязательных полей
                if (string.IsNullOrWhiteSpace(vol.FirstName) ||
                    string.IsNullOrWhiteSpace(vol.LastName) ||
                    string.IsNullOrWhiteSpace(vol.CountryCode) ||
                    string.IsNullOrWhiteSpace(vol.Gender))
                {
                    errorMessages.Add($"Обязательные поля недопустимы для волонтёра {vol.FirstName} {vol.LastName}.");
                    continue;
                }

                // Проверка корректности пола
                if (!new[] { "Male", "Female" }.Contains(vol.Gender))
                {
                    errorMessages.Add($"Некорректный пол: {vol.Gender} для волонтёра {vol.FirstName} {vol.LastName}. Пол должен быть 'Male' или 'Female'.");
                    continue;
                }

                // Проверка на дубликаты
                if (db.Volunteer.Any(v => v.CountryCode == vol.CountryCode &&
                                          v.FirstName == vol.FirstName &&
                                          v.LastName == vol.LastName &&
                                          v.Gender == vol.Gender))
                {
                    errorMessages.Add($"Запись уже существует: {vol.FirstName} {vol.LastName}, CountryCode: {vol.CountryCode}");
                    continue;
                }

                // Создание объекта Volunteer и добавление в БД
                Volunteer volbd = new Volunteer()
                {
                    CountryCode = vol.CountryCode,
                    FirstName = vol.FirstName,
                    LastName = vol.LastName,
                    Gender = vol.Gender
                };

                db.Volunteer.Add(volbd);
            }

            try
            {
                db.SaveChanges();

                if (errorMessages.Count > 0)
                {
                    string errors = string.Join(Environment.NewLine, errorMessages);
                    MessageBox.Show($"Импорт завершён частично. Не все данные были корректны:" + Environment.NewLine + errors);
                }
                else
                {
                    MessageBox.Show("Все данные успешно импортированы!");
                }

                _mainWindow.MainFrame.NavigationService.Navigate(new VolunteerManagement(_mainWindow));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}" +
                                Environment.NewLine +
                                "Внутреннее исключение:" +
                                Environment.NewLine +
                                $"{ex.InnerException?.Message ?? "Отсутствует"}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.FilterIndex = 1;
            open.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";

            if (open.ShowDialog() == true)
            {
                try
                {
                    var lines = File.ReadAllLines(open.FileName);

                    var data = from l in lines.Skip(1) // Пропускаем заголовок
                               let split = l.Split(',')
                               where split.Length >= 5
                               select new VolunteerClas
                               {
                                   FirstName = split[1],
                                   LastName = split[2],
                                   CountryCode = split[3],
                                   Gender = NormalizeGender(split[4]) // Нормализуем значение Gender
                               };

                    vol2 = data.ToList();

                    txbFilePath.Text = open.FileName;

                    MessageBox.Show($"Успешно загружено {vol2.Count} записей.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла: " + ex.Message);
                }
            }
        }

        private string NormalizeGender(string genderInput)
        {
            switch (genderInput.Trim().ToUpper())
            {
                case "M":
                case "MALE":
                    return "Male";

                case "F":
                case "FEMALE":
                    return "Female";

                default:
                    return genderInput; // Оставляем как есть, если не распознали
            }
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

    }

    




}
