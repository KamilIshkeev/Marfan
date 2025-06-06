﻿

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
using static System.Net.Mime.MediaTypeNames;
using WpfMarathon.Base;
using Microsoft.Extensions.Logging;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для PerviousResult.xaml
    /// </summary>
    public partial class PerviousResult : Page
    {
        public static MarafonEntities db = new MarafonEntities();
        List<Gonka> gonka = new List<Gonka>();
        int _age;
        int cnt;
        public PerviousResult(MainWindow mainWindow)
        {
            InitializeComponent();
            //countdown = new MarathonCountdown(UpdateCountdownText, marathonDate);
            LoadResults();
            LoadFilters();

            // Подписываемся на событие изменения выбора
            //MarathonComboBox.SelectionChanged += (s, e) => LoadResults();
            //DistanceComboBox.SelectionChanged += (s, e) => LoadResults();
            //GenderComboBox.SelectionChanged += (s, e) => LoadResults();
            //CategoryComboBox.SelectionChanged += (s, e) => LoadResults();
        }
        private MarathonCountdown countdown;
        private DateTime marathonDate = new DateTime(2025, 10, 20);


        //private void UpdateCountdownText(string text)
        //{
        //    CountdownTextBlock.Text = text;
        //}

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            countdown.Stop();
        }

        private void LoadFilters()
        {
            try
            {
                // Марафоны
                MarathonComboBox.ItemsSource = db.Marathon
                    .Select(m => new { m.MarathonId, m.MarathonName, m.YearHeld })
                    .ToList();
                MarathonComboBox.DisplayMemberPath = "MarathonName";
                MarathonComboBox.SelectedValuePath = "MarathonId";

                // Дистанции
                DistanceComboBox.ItemsSource = db.EventType
                    .Select(et => new { et.EventTypeId, et.EventTypeName })
                    .ToList();
                DistanceComboBox.DisplayMemberPath = "EventTypeName";
                DistanceComboBox.SelectedValuePath = "EventTypeId";

                // Пол
                GenderComboBox.ItemsSource = db.Runner
                    .Select(r => r.Gender)
                    .Distinct()
                    .ToList();

                // Категории
                CategoryComboBox.ItemsSource = new List<string>
                {
                    "до 18", "от 18 до 29", "от 30 до 39", "от 40 до 55", "от 56 до 70", "более 70"
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке фильтров: {ex.Message}");
            }
        }

        private void LoadResults()
        {
            try
            {
                // Получаем значения из фильтров
                int? marathonId = null;
                if (MarathonComboBox.SelectedValue != null)
                {
                    marathonId = Convert.ToInt32(MarathonComboBox.SelectedValue); // Преобразуем SelectedValue в int?
                }

                var distanceId = DistanceComboBox.SelectedValue as string;
                var gender = GenderComboBox.SelectedValue as string;
                var category = CategoryComboBox.SelectedValue as string;

                // Базовый запрос: только те, кто финишировал (RaceTime != null)
                var query = db.RegistrationEvent
                    .Where(re => re.RaceTime != null);

                // Фильтруем по марафону
                if (marathonId.HasValue)
                {
                    query = query.Where(re => re.Event.MarathonId == marathonId.Value);
                }

                // Фильтруем по дистанции
                if (!string.IsNullOrEmpty(distanceId))
                {
                    query = query.Where(re => re.Event.EventTypeId == distanceId);
                }

                // Фильтруем по полу
                if (!string.IsNullOrEmpty(gender))
                {
                    query = query.Where(re => re.Registration.Runner.Gender == gender);
                }

                // Загружаем данные в память
                var results = query
                    .Select(re => new
                    {
                        RunnerId = re.Registration.RunnerId,
                        RaceTime = re.RaceTime.Value,
                        FullName = re.Registration.Runner.User.FirstName + " " + re.Registration.Runner.User.LastName,
                        Country = re.Registration.Runner.CountryCode,
                        DateOfBirth = re.Registration.Runner.DateOfBirth
                    })
                    .ToList();

                // Фильтрация по возрастной категории
                if (!string.IsNullOrEmpty(category))
                {
                    var today = DateTime.Today;
                    results = results.Where(r =>
                    {
                        if (!r.DateOfBirth.HasValue) return false;
                        var dob = r.DateOfBirth.Value;
                        var age = today.Year - dob.Year;
                        if (dob > today.AddYears(-age)) age--;
                        switch (category)
                        {
                            case "до 18": return age < 18;
                            case "от 18 до 29": return age >= 18 && age <= 29;
                            case "от 30 до 39": return age >= 30 && age <= 39;
                            case "от 40 до 55": return age >= 40 && age <= 55;
                            case "от 56 до 70": return age >= 56 && age <= 70;
                            case "более 70": return age > 70;
                            default: return true;
                        }
                    }).ToList();
                }

                // Очистка предыдущих результатов
                ResultsPanel.Children.Clear();

                // Сортировка по времени
                var resultList = results.OrderBy(r => r.RaceTime).ToList();

                // Заполнение таблицы
                int place = 1;
                foreach (var result in resultList)
                {
                    var row = new Grid
                    {
                        Height = 36,
                        Background = Brushes.White,
                        ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(200) },
                    new ColumnDefinition { Width = new GridLength(350) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(160) }
                }
                    };
                    var time = TimeSpan.FromSeconds(result.RaceTime);
                    string formattedTime = $"{(int)time.TotalHours}h {time.Minutes}m {time.Seconds}s";
                    AddTextBlockToGrid(row, place++.ToString(), 0, true); // Место
                    AddTextBlockToGrid(row, formattedTime, 1); // Время
                    AddTextBlockToGrid(row, result.FullName, 2, false, HorizontalAlignment.Left); // Имя
                    AddTextBlockToGrid(row, result.Country, 3); // Страна
                    ResultsPanel.Children.Add(row);
                }

                // Обновление статистики
                TotalRunnersText.Text = db.RegistrationEvent.Count(re => re.RaceTime != null).ToString();
                TotalFinishedText.Text = resultList.Count.ToString();
                if (resultList.Any())
                {
                    var avgTime = TimeSpan.FromSeconds(resultList.Average(r => r.RaceTime));
                    AverageTimeText.Text = $"{(int)avgTime.TotalHours}h {avgTime.Minutes}m {avgTime.Seconds}s";
                }
                else
                {
                    AverageTimeText.Text = "-";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void LoadResults()
        //{
        //    try
        //    {
        //        var marathonId = MarathonComboBox.SelectedValue as int?;
        //        var distanceId = DistanceComboBox.SelectedValue as string;
        //        var gender = GenderComboBox.SelectedValue as string;
        //        var category = CategoryComboBox.SelectedValue as string;

        //        var query = db.RegistrationEvent
        //            .Where(re => re.RaceTime != null); // Только финишировавшие

        //        if (marathonId != null)
        //            query = query.Where(re => re.Event.MarathonId == marathonId);

        //        if (!string.IsNullOrEmpty(distanceId))
        //            query = query.Where(re => re.Event.EventTypeId == distanceId);

        //        if (!string.IsNullOrEmpty(gender))
        //            query = query.Where(re => re.Registration.Runner.Gender == gender);

        //        // Загружаем в память с нужными полями
        //        var results = query
        //            .Select(re => new
        //            {
        //                RunnerId = re.Registration.RunnerId,
        //                RaceTime = re.RaceTime.Value,
        //                FullName = re.Registration.Runner.User.FirstName + " " + re.Registration.Runner.User.LastName,
        //                Country = re.Registration.Runner.CountryCode,
        //                DateOfBirth = re.Registration.Runner.DateOfBirth
        //            })
        //            .ToList();

        //        // Фильтрация по возрасту
        //        if (!string.IsNullOrEmpty(category))
        //        {
        //            var today = DateTime.Today;

        //            results = results.Where(r =>
        //            {
        //                if (!r.DateOfBirth.HasValue) return false;

        //                var dob = r.DateOfBirth.Value;
        //                var age = today.Year - dob.Year;
        //                if (dob > today.AddYears(-age)) age--;

        //                switch (category)
        //                {
        //                    case "до 18":
        //                        return age < 18;
        //                    case "от 18 до 29":
        //                        return age >= 18 && age <= 29;
        //                    case "от 30 до 39":
        //                        return age >= 30 && age <= 39;
        //                    case "от 40 до 55":
        //                        return age >= 40 && age <= 55;
        //                    case "от 56 до 70":
        //                        return age >= 56 && age <= 70;
        //                    case "более 70":
        //                        return age > 70;
        //                    default:
        //                        return true;
        //                }
        //            }).ToList();
        //        }

        //        // Очистка UI
        //        ResultsPanel.Children.Clear();

        //        // Сортировка по времени (меньшее — выше)
        //        var resultList = results.OrderBy(r => r.RaceTime).ToList();

        //        int place = 1;
        //        foreach (var result in resultList)
        //        {
        //            var row = new Grid
        //            {
        //                Height = 36,
        //                Background = new SolidColorBrush(Colors.White),
        //                ColumnDefinitions =
        //                {
        //                    new ColumnDefinition { Width = new GridLength(200)  },
        //                    new ColumnDefinition { Width = new GridLength(350) },
        //                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
        //                    new ColumnDefinition { Width = new GridLength(160) }
        //                }
        //            };

        //            var time = TimeSpan.FromSeconds(result.RaceTime);
        //            string formattedTime = $"{(int)time.TotalHours}h {time.Minutes}m {time.Seconds}s";

        //            // Место
        //            AddTextBlockToGrid(row, place.ToString(), 0, true);

        //            // Время
        //            AddTextBlockToGrid(row, formattedTime, 1);

        //            // Имя бегуна
        //            AddTextBlockToGrid(row, result.FullName, 2, false, HorizontalAlignment.Left);

        //            // Страна
        //            AddTextBlockToGrid(row, result.Country, 3);

        //            row.Tag = result.RunnerId;
        //            row.MouseLeftButtonUp += (s, e) =>
        //            {
        //                var id = (int)((Grid)s).Tag;
        //                //new RunnerPopupWindow(id).ShowDialog(); // Теперь должно работать
        //            };

        //            ResultsPanel.Children.Add(row);
        //            place++;
        //        }

        //        // Обновление статистики
        //        TotalRunnersText.Text = query.Count().ToString();
        //        TotalFinishedText.Text = resultList.Count.ToString();

        //        if (resultList.Any())
        //        {
        //            var avgTime = TimeSpan.FromSeconds(resultList.Average(r => r.RaceTime));
        //            AverageTimeText.Text = $"{(int)avgTime.TotalHours}h {avgTime.Minutes}m {avgTime.Seconds}s";
        //        }
        //        else
        //        {
        //            AverageTimeText.Text = "-";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при загрузке результатов: {ex.Message}");
        //    }
        //}

        private void AddTextBlockToGrid(Grid grid, string text, int column, bool bold = false,
                                      HorizontalAlignment alignment = HorizontalAlignment.Center)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                FontSize = 32,
                Foreground = new SolidColorBrush(Color.FromRgb(112, 112, 112)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = alignment,
                FontFamily = new FontFamily("Arial"),
                Padding = alignment == HorizontalAlignment.Left ? new Thickness(10, 0, 0, 0) : new Thickness(0)
            };

            if (bold)
            {
                textBlock.FontWeight = FontWeights.Bold;
            }

            Grid.SetColumn(textBlock, column);
            grid.Children.Add(textBlock);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadResults();
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }


    public partial class Gonka
    {
        public short Mesto { get; set; }
        public TimeSpan Time { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
