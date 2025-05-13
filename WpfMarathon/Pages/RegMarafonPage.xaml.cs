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
        string id = "";

        public static MarafonEntities db = new MarafonEntities();
        public RegMarafonPage()
        {
            InitializeComponent();
            
            txt_price.Text = "";
        }

        private void btn_reg_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_fund.SelectedItem != null)
            {
                if (fundMoney > 0)
                {
                    List<User> us = new List<User>();
                    us = db.User.ToList();

                    foreach (var u in us)
                    {
                        if (u.Email == id)
                        {
                            try
                            {
                                //Registration stm = new Registration();
                                //stm.RunnerId = id;

                                //if (check_full.IsChecked == true)
                                //{
                                //    stm.ID_Marathon = 1;
                                //    db.StatisticsMarathon.Add(stm);
                                //    db.SaveChanges();
                                //}
                                //if (check_half.IsChecked == true)
                                //{
                                //    stm.ID_Marathon = 2;
                                //    db.StatisticsMarathon.Add(stm);
                                //    db.SaveChanges();
                                //}
                                //if (check_min.IsChecked == true)
                                //{
                                //    stm.ID_Marathon = 3;
                                //    db.StatisticsMarathon.Add(stm);
                                //    db.SaveChanges();
                                //}

                                //List<Fund> fun = new List<Fund>();
                                //fun = db.Fund.ToList();

                                //foreach (var f in fun)
                                //{

                                //    //Донат
                                //    if (f.ID == u.ID_Fund)
                                //    {
                                //        f.Money -= price;
                                //        db.SaveChanges();
                                //    }
                                //}

                                //if (radio_a.IsChecked == true)
                                //{
                                //    u.ID_Inventory = 1;
                                //    db.SaveChanges();
                                //}
                                //else if (radio_b.IsChecked == true)
                                //{
                                //    u.ID_Inventory = 2;
                                //    db.SaveChanges();
                                //}
                                //else if (radio_c.IsChecked == true)
                                //{
                                //    u.ID_Inventory = 3;
                                //    db.SaveChanges();
                                //}
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
