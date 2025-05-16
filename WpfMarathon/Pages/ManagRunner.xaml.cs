using Microsoft.EntityFrameworkCore.Internal;
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

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManagRunner.xaml
    /// </summary>
    public partial class ManagRunner : Page
    {
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

        public static MarafonEntities db = new MarafonEntities();
        List<string> pay = new List<string>() {
            "Оплата подтвержена",
            "Оплата не подтвержена",
        };
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
            public byte RegistrationStatusId { get; set; }
        }

        public ManagRunner()
        {
            InitializeComponent();
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
                            RegistrationStatusId = g.RegistrationStatusId
                        };
            UserInCoord.ItemsSource = query.ToList();
            cmbPayment.ItemsSource = pay;
            cmbSortBy.ItemsSource = sort;
            cmbDistance.ItemsSource = db.EventType.ToList();
        }
        private void btnUserUpdate_Click(object sender, RoutedEventArgs e)
        {
            int pay = 0;

            List<User> bdList = db.User.ToList();
            List<Runner> rn = db.Runner.ToList();
            List<Registration> stm = db.Registration.ToList();
            List<Marathon> sm = db.Marathon.ToList();

            if (cmbPayment.SelectedItem != null)
            {
                if (cmbPayment.SelectedItem == "Оплата подтвержена")
                {
                    pay = 1;
                }

                if (cmbDistance.SelectedItem != null)
                {
                    if (cmbSortBy.SelectedItem == "Имени")
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
                                         orderby n.User.FirstName
                                         select n;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else if (cmbSortBy.SelectedItem == "Фамилии")
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
                                         orderby n.User.LastName
                                         select n;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else if (cmbSortBy.SelectedItem == "Email")
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
                                         orderby n.User.Email
                                         select n;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else if (cmbSortBy.SelectedItem == null)
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where pay == b.Cost && n.RunnerId == b.RunnerId && m.Email == n.Email
                                         select n.User;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct(st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else
                    {
                        UserInCoord.ItemsSource = db.User.ToList();
                    }
                }
                else
                {
                    if (cmbSortBy.SelectedItem == "Имени")
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where pay == b.Cost 
                                         orderby n.User.FirstName
                                         select b;
                        UserInCoord.ItemsSource = sortByName;
                    }
                    else if (cmbSortBy.SelectedItem == "Фамилии")
                    {
                        var sortBySurName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where pay == b.Cost 
                                         orderby n.User.LastName
                                         select b;
                        UserInCoord.ItemsSource = sortBySurName;
                    }
                    else if (cmbSortBy.SelectedItem == "Email")
                    {
                        var sortByEmail = from n in rn
                                            from b in stm
                                            from m in bdList
                                            where pay == b.Cost 
                                            orderby n.User.Email
                                          select b;
                        UserInCoord.ItemsSource = sortByEmail;
                    }
                    else if (cmbSortBy.SelectedItem == null)
                    {
                        var sortByNot = from n in rn
                                          from b in stm
                                          from m in bdList
                                          where pay == b.Cost
                                          select b;
                        UserInCoord.ItemsSource = sortByNot;
                    }
                    else
                    {
                        UserInCoord.ItemsSource = db.User.ToList();
                    }
                }
            }
            else
            {
                if (cmbDistance.SelectedItem != null)
                {

                    if (cmbSortBy.SelectedItem == "Имени")
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where n.RunnerId == b.RunnerId && m.Email == n.Email
                                         orderby n.User.FirstName
                                         select n;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else if (cmbSortBy.SelectedItem == "Фамилии")
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where n.RunnerId == b.RunnerId && m.Email == n.Email
                                         orderby n.User.LastName
                                         select n;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else if (cmbSortBy.SelectedItem == "Email")
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where n.RunnerId == b.RunnerId && m.Email == n.Email
                                         orderby n.User.Email
                                         select n;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct((IEqualityComparer<Runner>)st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else if (cmbSortBy.SelectedItem == null)
                    {
                        var sortByName = from n in rn
                                         from b in stm
                                         from m in bdList
                                         where n.RunnerId == b.RunnerId && m.Email == n.Email
                                         select n.User;
                        SortUser st = new SortUser();
                        var distinctsort = sortByName.Distinct(st);
                        UserInCoord.ItemsSource = distinctsort;
                    }
                    else
                    {
                        UserInCoord.ItemsSource = db.User.ToList();
                    }
                }
                else
                {
                    if (cmbSortBy.SelectedItem == "Имени")
                    {
                        var sortByName = from b in bdList
                                         orderby b.FirstName
                                         select b;
                        UserInCoord.ItemsSource = sortByName;
                    }
                    else if (cmbSortBy.SelectedItem == "Фамилии")
                    {
                        var sortBySurName = from b in bdList
                                            orderby b.LastName
                                            select b;
                        UserInCoord.ItemsSource = sortBySurName;
                    }
                    else if (cmbSortBy.SelectedItem == "Email")
                    {
                        var sortByEmail = from b in bdList
                                          orderby b.Email
                                          select b;
                        UserInCoord.ItemsSource = sortByEmail;
                    }
                    else if (cmbSortBy.SelectedItem == null)
                    {
                        var sortByNot = from b in bdList
                                        select b;
                        UserInCoord.ItemsSource = sortByNot;

                    }
                    else
                    {
                        UserInCoord.ItemsSource = db.User.ToList();
                    }
                }
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            UserInCoord.ItemsSource = db.User.ToList();
            cmbDistance.SelectedIndex = -1;
            cmbPayment.SelectedIndex = -1;
            cmbPayment.SelectedIndex = -1;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = UserInCoord.SelectedValue as User;
            //EditUser aed = new EditUser(user);
            //this.NavigationService.Navigate(aed);
        }
        private void btnInExecel_Click(object sender, RoutedEventArgs e)
        {
            //Объявляем приложение
            Microsoft.Office.Interop.Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
            //Количество листов в рабочей книге
            ex.SheetsInNewWorkbook = 2;
            ex.Visible = true;
            //Добавить рабочую книгу
            Microsoft.Office.Interop.Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
            //Отключить отображение окон с сообщениями
            ex.DisplayAlerts = false;
            //Получаем первый лист документа (счет начинается с 1)
            Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)ex.Worksheets.get_Item(1);
            //Название листа (вкладки снизу)
            sheet.Name = $"Отчет";
            //Пример заполнения ячеек
            sheet.Cells[1, 1] = String.Format("Имя");
            sheet.Cells[1, 2] = String.Format("Фамилия");
            sheet.Cells[1, 3] = String.Format("E-mail");
            sheet.Cells[1, 4] = String.Format("Статус");
            int i = 2;
            foreach (User b in UserInCoord.Items)
            {
                var rn = db.Runner.FirstOrDefault(x=> x.Email == b.Email).RunnerId;
                var stm = db.Registration.FirstOrDefault(x => x.RunnerId == rn).Cost;
                sheet.Cells[i, 1] = String.Format($"{b.FirstName}");
                sheet.Cells[i, 2] = String.Format($"{b.LastName}");
                sheet.Cells[i, 3] = String.Format($"{b.Email}");
                if (stm == 0)
                {
                    sheet.Cells[i, 4] = String.Format($"Оплата не подтверждена");
                }
                else
                {
                    sheet.Cells[i, 4] = String.Format($"Оплата подтверждена");
                }
                i++;
            }

            ex.Application.ActiveWorkbook.SaveAs("otchet.csv");
            //Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
            //Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            string writePath = @"emails.txt";
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                foreach (User b in UserInCoord.Items)
                {
                    sw.WriteLine(b.Email);
                }
            }

            MessageBox.Show("Фаил в директории\nMarfan/WpfMarathon/bin/emails", "Успешно");
        }
    }
}
