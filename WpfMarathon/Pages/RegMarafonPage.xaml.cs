using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для RegMarafonPage.xaml
    /// </summary>
    public partial class RegMarafonPage : Page
    {
        int price;
        int fundMoney;
        int id = 0;
        public static MarafonEntities db = new MarafonEntities();
        public RegMarafonPage(int _id)
        {
            InitializeComponent();
            id = _id;

            List<Sponsorship> fun = db.Sponsorship.ToList();
            List<Registration> us = db.Registration.ToList();

            foreach (var f in fun)
            {
                foreach (var u in us)
                {
                    if (f.RegistrationId == u.RegistrationId && u.RegistrationId == id)
                    {
                        txb_price.Text = f.Amount.ToString();
                        fundMoney = (int)f.Amount;
                    }
                }
            }
            var re = from f in db.Sponsorship.ToList()
                     from u in db.Registration.ToList()
                     where f.RegistrationId == u.RegistrationId && u.RegistrationId == id
                     select f.SponsorName;

            cmb_fund.ItemsSource = re;

            foreach (var b in db.RaceKitOption)
            {
                if (b.RaceKitOptionId == "A")
                {
                    check_full.Content = $"{b.RaceKitOptionId}km {b.RaceKitOption1} (${b.Cost})";
                }
                if (b.RaceKitOptionId == "B")
                {
                    check_half.Content = $"{b.RaceKitOptionId}km {b.RaceKitOption1} (${b.Cost})";
                }
                if (b.RaceKitOptionId == "C")
                {
                    check_min.Content = $"{b.RaceKitOptionId}km {b.RaceKitOption1} (${b.Cost})";
                }
            }
            txt_price.Text = "";
        }

        private void btn_reg_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_fund.SelectedItem != null)
            {
                if (fundMoney > 0)
                {
                    List<Registration> us = new List<Registration>();
                    us = db.Registration.ToList();

                    foreach (var u in us)
                    {
                        if (u.RegistrationId == id)
                        {
                            try
                            {
                                Registration stm = new Registration();
                                stm.RunnerId = id;
                                RegistrationEvent regev = new RegistrationEvent();
                                regev.RegistrationId = stm.RegistrationId;
                                Event ev = new Event();
                                ev.EventId = regev.EventId;
                                if (check_full.IsChecked == true)
                                {
                                    ev.EventTypeId = "FM";
                                    db.Event.Add(ev);
                                    db.SaveChanges();
                                }
                                if (check_half.IsChecked == true)
                                {
                                    ev.EventTypeId = "FR"; 
                                    db.Event.Add(ev);
                                    db.SaveChanges();
                                }
                                if (check_min.IsChecked == true)
                                {
                                    ev.EventTypeId = "HM"; 
                                    db.Event.Add(ev);
                                    db.SaveChanges();
                                }

                                List<Sponsorship> fun = new List<Sponsorship>();
                                fun = db.Sponsorship.ToList();

                                foreach (var f in fun)
                                {

                                    //Донат
                                    if (f.RegistrationId == u.RegistrationId)
                                    {
                                        f.Amount -= price;
                                        db.SaveChanges();
                                    }
                                }

                                if (radio_a.IsChecked == true)
                                {
                                    u.RaceKitOptionId = "A";
                                    db.SaveChanges();
                                }
                                else if (radio_b.IsChecked == true)
                                {
                                    u.RaceKitOptionId = "B";
                                    db.SaveChanges();
                                }
                                else if (radio_c.IsChecked == true)
                                {
                                    u.RaceKitOptionId = "C";
                                    db.SaveChanges();
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Не удалось выполнить операцию", "Ошибка");
                            }
                        }
                    }

                    this.NavigationService.Navigate(new Uri("Runner/RegConfirm.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("У спонсора недостаточно средств", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Не выбран спонсор", "Ошибка");
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void check_min_Checked(object sender, RoutedEventArgs e)
        {
            price += 20;
            txt_price.Text = Convert.ToString(price);
        }

        private void check_half_Checked(object sender, RoutedEventArgs e)
        {
            price += 75;
            txt_price.Text = Convert.ToString(price);

        }

        private void check_full_Checked(object sender, RoutedEventArgs e)
        {
            price += 145;
            txt_price.Text = Convert.ToString(price);
        }

        private void radio_a_Checked(object sender, RoutedEventArgs e)
        {
            price += 0;
            txt_price.Text = Convert.ToString(price);
        }

        private void radio_b_Checked(object sender, RoutedEventArgs e)
        {
            price += 20;
            txt_price.Text = Convert.ToString(price);
        }

        private void radio_c_Checked(object sender, RoutedEventArgs e)
        {
            price += 45;
            txt_price.Text = Convert.ToString(price);
        }

        private void check_full_Unchecked(object sender, RoutedEventArgs e)
        {
            price -= 145;
            txt_price.Text = Convert.ToString(price);
        }

        private void check_half_Unchecked(object sender, RoutedEventArgs e)
        {
            price -= 75;
            txt_price.Text = Convert.ToString(price);
        }

        private void check_min_Unchecked(object sender, RoutedEventArgs e)
        {
            price -= 20;
            txt_price.Text = Convert.ToString(price);
        }

        private void radio_a_Unchecked(object sender, RoutedEventArgs e)
        {
            price -= 0;
            txt_price.Text = Convert.ToString(price);
        }

        private void radio_b_Unchecked(object sender, RoutedEventArgs e)
        {
            price -= 20;
            txt_price.Text = Convert.ToString(price);
        }

        private void radio_c_Unchecked(object sender, RoutedEventArgs e)
        {
            price -= 45;
            txt_price.Text = Convert.ToString(price);
        }
    }
}
