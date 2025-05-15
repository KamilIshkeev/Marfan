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
        static MainWindow _mainWindow;
        int price;
        int fundMoney;
        int id = 0;
        public static MarafonEntities db = new MarafonEntities();
        public RegMarafonPage(MainWindow mainWindow, int _id)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
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
            var re = db.Charity.Select(x=> x.CharityName);

            cmb_fund.ItemsSource = re;
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
                                var chartId = db.Charity.FirstOrDefault(x=> x.CharityName == cmb_fund.SelectedItem).CharityId;
                                Registration stm = new Registration 
                                { 
                                    RunnerId = id,
                                    RegistrationDateTime = DateTime.Now,
                                    RegistrationStatusId = 4,
                                    SponsorshipTarget = fundMoney,
                                    CharityId = chartId,
                                };
                                
                                RegistrationEvent regev = new RegistrationEvent();
                                regev.RegistrationId = stm.RegistrationId;
                                Event ev = new Event();
                                ev.EventId = regev.EventId;
                                RegistrationEvent regevent = new RegistrationEvent
                                {
                                    RegistrationId = stm.RegistrationId,

                                };
                                if (check_full.IsChecked == true)
                                {
                                    Random rn = new Random();
                                    int rno = rn.Next(1, 2200);
                                    regevent.BibNumber = Convert.ToInt16(rno);
                                    regevent.EventId = "15_5FM";
                                    db.RegistrationEvent.Add(regevent);
                                    db.SaveChanges();
                                }
                                if (check_half.IsChecked == true)
                                {
                                    Random rn = new Random();
                                    int rno = rn.Next(1, 5000);
                                    regevent.BibNumber = Convert.ToInt16(rno);
                                    regevent.EventId = "15_5FR"; 
                                    db.RegistrationEvent.Add(regevent);
                                    db.SaveChanges();
                                }
                                if (check_min.IsChecked == true)
                                {
                                    Random rn = new Random();
                                    int rno = rn.Next(1, 2500);
                                    regevent.EventId = "15_5HM";
                                    regevent.BibNumber = Convert.ToInt16(rno);
                                    db.RegistrationEvent.Add(regevent);
                                    db.SaveChanges();
                                }

                                List<Registration> fun = new List<Registration>();
                                fun = db.Registration.ToList();

                                foreach (var f in fun)
                                {

                                    //Донат
                                    if (f.RegistrationId == u.RegistrationId)
                                    {
                                        f.SponsorshipTarget -= price;
                                        db.SaveChanges();
                                    }
                                }

                                if (radio_a.IsChecked == true)
                                {
                                    stm.RaceKitOptionId = "A";
                                    stm.Cost = 0;
                                    db.SaveChanges();
                                }
                                else if (radio_b.IsChecked == true)
                                {
                                    stm.RaceKitOptionId = "B";
                                    stm.Cost = 15;
                                    db.SaveChanges();
                                }
                                else if (radio_c.IsChecked == true)
                                {
                                    stm.RaceKitOptionId = "C";
                                    stm.Cost = 20;
                                    db.SaveChanges();
                                }

                            }
                            catch
                            {
                                MessageBox.Show("Не удалось выполнить операцию", "Ошибка");
                            }
                        }
                    }

                    _mainWindow.MainFrame.NavigationService.Navigate(new RegConfirm(_mainWindow, id));
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
