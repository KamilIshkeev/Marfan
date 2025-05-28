//using Microsoft.EntityFrameworkCore.Internal;
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
using Microsoft.Office.Interop.Excel;
using Page = System.Windows.Controls.Page;
using Path = System.IO.Path;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManagRunner.xaml
    /// </summary>
    public partial class ManagRunner : Page
    {
        static MainWindow _mainWindow;
        class SortUser : IEqualityComparer<User>
        {
            public bool Equals(User x, User y)
            {
                return x.FirstName == y.FirstName && x.LastName == y.LastName && x.Email == y.Email;
            }
            public int GetHashCode(User x)
            {
                return (x.FirstName + "_" + x.LastName + "_" + x.Email).GetHashCode();
            }
        }
        
        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
        }

        public static MarafonEntities db = new MarafonEntities();
        List<string> pay = db.RegistrationStatus.Select(x=> x.RegistrationStatus1).ToList();
        List<string> sort = new List<string>()
        {
            "Имени",
            "Фамилии",
            "Email",
        };

        public partial class UserRunnerRegistrationView
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string RegistrationStatus { get; set; }
            public string Distance { get; set; }
        }
        public ManagRunner(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            var us =new User();
            var rn =new Runner();
            var reg = new Registration();
            var query = from u in db.User
                        join r in db.Runner on u.Email equals r.Email
                        join g in db.Registration on r.RunnerId equals g.RunnerId
                        where u.RoleId == "R"
                        select new UserRunnerRegistrationView
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            RegistrationStatus = g.RegistrationStatus.RegistrationStatus1
                        };
            UserInCoord.ItemsSource = query.ToList();
            cmbPayment.ItemsSource = pay;
            cmbSortBy.ItemsSource = sort;
            cmbDistance.ItemsSource = db.EventType.Select(x=> x.EventTypeName).ToList();
        }
        //private void btnUserUpdate_Click(object sender, RoutedEventArgs e)
        //{
        //    int pay = 0;

        //    List<User> bdList = db.User.ToList();
        //    List<Runner> rn = db.Runner.ToList();
        //    List<Registration> stm = db.Registration.ToList();
        //    List<Marathon> sm = db.Marathon.ToList();

        //    if (cmbPayment.SelectedItem != null)
        //    {
        //        if (cmbPayment.SelectedItem == "Payment Confirmed")
        //        {
        //            pay = 1;
        //        }

        //        if (cmbDistance.SelectedItem != null)
        //        {
        //            if (cmbSortBy.SelectedItem == "Имени")
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 orderby n.User.FirstName
        //                                 select n;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Фамилии")
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 orderby n.User.LastName
        //                                 select n;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Email")
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 orderby n.User.Email
        //                                 select n;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else if (cmbSortBy.SelectedItem == null)
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 select n.User;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct(st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else
        //            {
        //                UserInCoord.ItemsSource = db.User.ToList();
        //            }
        //        }
        //        else
        //        {
        //            if (cmbSortBy.SelectedItem == "Имени")
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where pay == b.Cost 
        //                                 orderby n.User.FirstName
        //                                 select b;
        //                UserInCoord.ItemsSource = sortByName;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Фамилии")
        //            {
        //                var sortBySurName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where pay == b.Cost 
        //                                 orderby n.User.LastName
        //                                 select b;
        //                UserInCoord.ItemsSource = sortBySurName;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Email")
        //            {
        //                var sortByEmail = from n in rn
        //                                    from b in stm
        //                                    from m in bdList
        //                                    where pay == b.Cost 
        //                                    orderby n.User.Email
        //                                  select b;
        //                UserInCoord.ItemsSource = sortByEmail;
        //            }
        //            else if (cmbSortBy.SelectedItem == null)
        //            {
        //                var sortByNot = from n in rn
        //                                  from b in stm
        //                                  from m in bdList
        //                                  where pay == b.Cost
        //                                  select b;
        //                UserInCoord.ItemsSource = sortByNot;
        //            }
        //            else
        //            {
        //                UserInCoord.ItemsSource = db.User.ToList();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (cmbDistance.SelectedItem != null)
        //        {

        //            if (cmbSortBy.SelectedItem == "Имени")
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 orderby n.User.FirstName
        //                                 select n;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Фамилии")
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 orderby n.User.LastName
        //                                 select n;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Email")
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 orderby n.User.Email
        //                                 select n;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else if (cmbSortBy.SelectedItem == null)
        //            {
        //                var sortByName = from n in rn
        //                                 from b in stm
        //                                 from m in bdList
        //                                 where n.RunnerId == b.RunnerId && m.Email == n.Email
        //                                 select n.User;
        //                SortUser st = new SortUser();
        //                var distinctsort = sortByName.Distinct(st);
        //                UserInCoord.ItemsSource = distinctsort;
        //            }
        //            else
        //            {
        //                UserInCoord.ItemsSource = db.User.ToList();
        //            }
        //        }
        //        else
        //        {
        //            if (cmbSortBy.SelectedItem == "Имени")
        //            {
        //                var sortByName = from b in bdList
        //                                 orderby b.FirstName
        //                                 select b;
        //                UserInCoord.ItemsSource = sortByName;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Фамилии")
        //            {
        //                var sortBySurName = from b in bdList
        //                                    orderby b.LastName
        //                                    select b;
        //                UserInCoord.ItemsSource = sortBySurName;
        //            }
        //            else if (cmbSortBy.SelectedItem == "Email")
        //            {
        //                var sortByEmail = from b in bdList
        //                                  orderby b.Email
        //                                  select b;
        //                UserInCoord.ItemsSource = sortByEmail;
        //            }
        //            else if (cmbSortBy.SelectedItem == null)
        //            {
        //                var sortByNot = from b in bdList
        //                                select b;
        //                UserInCoord.ItemsSource = sortByNot;

        //            }
        //            else
        //            {
        //                UserInCoord.ItemsSource = db.User.ToList();
        //            }
        //        }
        //    }

        //}




        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbPayment.SelectedIndex = -1;
            cmbDistance.SelectedIndex = -1;
            cmbSortBy.SelectedIndex = -1;

            var originalData = from u in db.User
                               join r in db.Runner on u.Email equals r.Email
                               join g in db.Registration on r.RunnerId equals g.RunnerId
                               join ev in db.Event on g.RegistrationEvent.Select(x => x.EventId).SingleOrDefault() equals ev.EventId
                               where u.RoleId == "R"
                               select new UserRunnerRegistrationView
                               {
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Email = u.Email,
                                   RegistrationStatus = g.RegistrationStatus.RegistrationStatus1,
                                   Distance = ev.EventType.EventTypeName
                               };

            UserInCoord.ItemsSource = originalData.ToList();
            txbCountUser.Text = originalData.Count().ToString();
        }

        //private void btnClear_Click(object sender, RoutedEventArgs e)
        //{
        //    UserInCoord.ItemsSource = db.User.ToList();
        //    cmbDistance.SelectedIndex = -1;
        //    cmbPayment.SelectedIndex = -1;
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = UserInCoord.SelectedValue as User;
            //EditUser aed = new EditUser(user);
            //this.NavigationService.Navigate(aed);
        }
        private void btnUserUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Загружаем данные из базы один раз
            var query = from u in db.User
                        join r in db.Runner on u.Email equals r.Email
                        join g in db.Registration on r.RunnerId equals g.RunnerId
                        let eventId = g.RegistrationEvent
                            .Select(re => re.EventId)
                            .FirstOrDefault()
                        join ev in db.Event on eventId equals ev.EventId into evGroup
                        from ev in evGroup.DefaultIfEmpty() // Left Join
                        select new UserRunnerRegistrationView
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            RegistrationStatus = g.RegistrationStatus.RegistrationStatus1,
                            Distance = ev.EventType != null ? ev.EventType.EventTypeName : "Неизвестно"
                        };

            var filteredList = query.ToList();

            // Фильтр по статусу оплаты
            if (cmbPayment.SelectedItem is string selectedPayment && !string.IsNullOrEmpty(selectedPayment))
            {
                filteredList = filteredList
                    .Where(x => x.RegistrationStatus == selectedPayment)
                    .ToList();
            }

            // Фильтр по дистанции
            if (cmbDistance.SelectedItem is string selectedDistance && !string.IsNullOrEmpty(selectedDistance))
            {
                filteredList = filteredList
                    .Where(x => x.Distance == selectedDistance)
                    .ToList();
            }

            // Сортировка
            if (cmbSortBy.SelectedItem is string selectedSort)
            {
                switch (selectedSort)
                {
                    case "Имени":
                        filteredList = filteredList.OrderBy(x => x.FirstName).ToList();
                        break;
                    case "Фамилии":
                        filteredList = filteredList.OrderBy(x => x.LastName).ToList();
                        break;
                    case "Email":
                        filteredList = filteredList.OrderBy(x => x.Email).ToList();
                        break;
                }
            }

            // Обновляем DataGrid
            UserInCoord.ItemsSource = filteredList;

            // Обновляем счётчик
            txbCountUser.Text = filteredList.Count.ToString();
        }

        private void btnInExecel_Click(object sender, RoutedEventArgs e)
        {
            if (UserInCoord.Items.Count == 0)
            {
                MessageBox.Show("Нет данных для выгрузки.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Создаем приложение Excel
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = true;
                excelApp.DisplayAlerts = false;

                // Создаем рабочую книгу и лист
                Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
                Worksheet worksheet = (Worksheet)workbook.Sheets[1];
                worksheet.Name = "Список бегунов";

                // Заголовки таблицы
                worksheet.Cells[1, 1] = "Имя";
                worksheet.Cells[1, 2] = "Фамилия";
                worksheet.Cells[1, 3] = "Email";
                worksheet.Cells[1, 4] = "Статус оплаты";
                worksheet.Cells[1, 5] = "Дистанция";

                // Автоширина заголовков
                worksheet.Range["A1:E1"].Font.Bold = true;
                worksheet.Range["A1:E1"].Interior.Color = XlRgbColor.rgbLightBlue;
                worksheet.Range["A1:E1"].Borders.LineStyle = XlLineStyle.xlContinuous;

                int row = 2;

                foreach (var item in UserInCoord.Items)
                {
                    // Получаем объект через ItemsSource
                    var obj = item.GetType().GetProperty("FirstName")?.GetValue(item)?.ToString() ?? "";
                    var lastName = item.GetType().GetProperty("LastName")?.GetValue(item)?.ToString() ?? "";
                    var email = item.GetType().GetProperty("Email")?.GetValue(item)?.ToString() ?? "";
                    var status = item.GetType().GetProperty("RegistrationStatus")?.GetValue(item)?.ToString() ?? "";

                    // Если нужна дистанция — добавляем её
                    var distance = "";
                    var distProp = item.GetType().GetProperty("Distance");
                    if (distProp != null)
                        distance = distProp.GetValue(item)?.ToString() ?? "";

                    worksheet.Cells[row, 1] = obj;
                    worksheet.Cells[row, 2] = lastName;
                    worksheet.Cells[row, 3] = email;
                    worksheet.Cells[row, 4] = status;
                    worksheet.Cells[row, 5] = distance;

                    row++;
                }

                // Автоподбор ширины столбцов
                worksheet.Columns.AutoFit();

                // Сохраняем файл
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Список_бегунов.xlsx");

                workbook.SaveAs(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);

                workbook.Close(false, Type.Missing, Type.Missing);
                excelApp.Quit();

                MessageBox.Show($"Файл успешно сохранён:\n{filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании Excel-файла:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            string writePath = @"emails.txt";

            if (UserInCoord.ItemsSource is List<UserRunnerRegistrationView> users)
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    foreach (var user in users)
                    {
                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            sw.WriteLine(user.Email);
                        }
                    }
                }
            }

            MessageBox.Show("Файл с email-ами успешно сохранён в директории:\n" + Path.GetFullPath(writePath), "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
