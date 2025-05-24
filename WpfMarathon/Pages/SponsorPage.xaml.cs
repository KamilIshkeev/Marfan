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
    /// Логика взаимодействия для SponsorPage.xaml
    /// </summary>
    public partial class SponsorPage : Page
    {
        static MainWindow _mainWindow1;
        public static MarafonEntities db = new MarafonEntities();
        public SponsorPage(MainWindow _mainWindow)
        {
            InitializeComponent();
            _mainWindow1 = _mainWindow;
            cmbRunner.ItemsSource = db.User.Where(x => x.RoleId == "R").Select(x=> x.Email).ToList();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow1.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow1));
        }

        private void btn_pay_Click(object sender, RoutedEventArgs e)
        {
            if (txb_cardholder.Text != null && txb_cardnum.Text != null && txb_month.Text != null && txb_name.Text != null && txb_year.Text != null && cmbRunner.SelectedIndex != -1)
            {
                string sponsor_name = txb_name.Text;
                var check = (from b in db.Sponsorship
                             where b.SponsorName == sponsor_name
                             select b).SingleOrDefault();
                decimal count = Convert.ToDecimal(txb_price_num.Text);
                var run = cmbRunner.SelectedItem;
                var ryn = db.Runner.FirstOrDefault(x => x.Email == run);
                var user = db.Registration.FirstOrDefault(x => x.RunnerId == ryn.RunnerId).RegistrationId;
                List<Sponsorship> fund = db.Sponsorship.ToList();
                List<Sponsorship> sponsor = db.Sponsorship.ToList();
                int id_sponsor = sponsor.Where(x => x.SponsorName == sponsor_name).Select(x => x.SponsorshipId).SingleOrDefault();
                decimal money = count;
                int money_int = Convert.ToInt32(money);
                if (check == null)
                {
                    db.Sponsorship.Add(new Sponsorship
                    {
                        SponsorName = sponsor_name,
                        RegistrationId = user,
                        Amount = money,
                    });
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Такой пользователь уже есть", "Ошибка");
                }

                try
                {
                    decimal countref = (decimal)fund.Where(x => x.RegistrationId == user).Select(x => x.Amount).SingleOrDefault();
                    money = count + countref;
                    db.Database.ExecuteSqlCommand($"UPDATE Sponsorship SET Amount = {money_int} WHERE SponsorshipId = {id_sponsor} And RegistrationId = {user}");
                    db.SaveChanges();
                    ThanksPay thxForPay = new ThanksPay(_mainWindow1,txb_price_num.Text, txt_fund.Text, Convert.ToString(cmbRunner.SelectedValue));
                    NavigationService.Navigate(thxForPay);
                }
                catch
                {
                    MessageBox.Show("Не удалось выполнить операцию", "Ошибка");
                }

            }
            else
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка");
            }

        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btn_pricesum_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt64(Convert.ToInt64(txt_price.Text) + Convert.ToInt64(txb_price_num.Text)) <= 10000)
            {
                txb_price_num.Text = Convert.ToString(Convert.ToInt32(txt_price.Text) + Convert.ToInt32(txb_price_num.Text));
            }
        }

        private void btn_pricemin_Click1(object sender, RoutedEventArgs e)
        {
            txb_price_num.Text = Convert.ToString(Convert.ToInt32(txb_price_num.Text) - Convert.ToInt32(txt_price.Text));
            if (Convert.ToInt32(Convert.ToInt32(txt_price.Text) - Convert.ToInt32(txb_price_num.Text)) > 0)
            {
                txb_price_num.Text = "0";
            }
        }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);

        }


        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);

        }

        private void cmbRunner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var run = cmbRunner.SelectedItem;
            var ryn = db.Runner.FirstOrDefault(x => x.Email == run);
            var user = db.Registration.FirstOrDefault(x => x.RunnerId == ryn.RunnerId).CharityId;
            List<Charity> fund = db.Charity.ToList();
            txt_fund.Text = fund.Where(x => x.CharityId == user).Select(x => x.CharityName).SingleOrDefault();
        }
        private void txb_cardnum_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_cardnum.Text = "";
        }

        private void txb_name_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_name.Text = "";
        }

        private void txb_cardholder_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_cardholder.Text = "";
        }

        private void txb_month_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_month.Text = "";
        }

        private void txb_year_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_year.Text = "";
        }

        private void txb_year_Copy_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_year_copy.Text = "";
        }

    }
}
