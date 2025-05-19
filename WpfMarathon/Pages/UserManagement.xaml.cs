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
    /// Логика взаимодействия для UserManagement.xaml
    /// </summary>
        public partial class UserManagement : Page
        {
            public static MarafonEntities db = new MarafonEntities();
            List<string> sort = new List<string>()
        {
            "Имени",
            "Фамилии",
            "Email",
        };
            List<string> role = new List<string>()
        {
            "R",
            "A",
            "C",
        };
            public UserManagement()
            {
                InitializeComponent();
                UserInAdmin.ItemsSource = db.User.ToList();
                cmbRole.ItemsSource = role;
                cmbSortBy.ItemsSource = sort;
            }

            private void btnAddUser_Click(object sender, RoutedEventArgs e)
            {
                this.NavigationService.Navigate(new Uri("AdminController/AddNewUser.xaml", UriKind.Relative));
            }

            private void btnUser_Copy_Click(object sender, RoutedEventArgs e)
            {
                if (txbFind.Text == "")
                {
                    List<User> bdList = db.User.ToList();

                    if (cmbRole.SelectedItem != null)
                    {
                        if (cmbSortBy.SelectedItem == "Имени")
                        {
                            var sortByName = from b in bdList
                                             where b.RoleId == cmbRole.SelectedItem.ToString()
                                             orderby b.FirstName
                                             select b;
                            UserInAdmin.ItemsSource = sortByName;
                        }
                        else if (cmbSortBy.SelectedItem == "Фамилии")
                        {
                            var sortBySurName = from b in bdList
                                                where b.RoleId == cmbRole.SelectedItem.ToString()
                                                orderby b.LastName
                                                select b;
                            UserInAdmin.ItemsSource = sortBySurName;
                        }
                        else if (cmbSortBy.SelectedItem == "Email")
                        {
                            var sortByEmail = from b in bdList
                                              where b.RoleId == cmbRole.SelectedItem.ToString()
                                              orderby b.Email
                                              select b;
                            UserInAdmin.ItemsSource = sortByEmail;
                        }
                        else if (cmbSortBy.SelectedItem == null)
                        {
                            UserInAdmin.ItemsSource = db.User.Where(itemF => itemF.RoleId == cmbRole.SelectedItem).ToList();
                        }
                        else
                        {
                            UserInAdmin.ItemsSource = db.User.ToList();
                        }
                    }
                    else
                    {
                        if (cmbSortBy.SelectedItem == "Имени")
                        {
                            var sortByName = from b in bdList
                                             orderby b.FirstName
                                             select b;
                            UserInAdmin.ItemsSource = sortByName;
                        }
                        else if (cmbSortBy.SelectedItem == "Фамилии")
                        {
                            var sortBySurName = from b in bdList
                                                orderby b.LastName
                                                select b;
                            UserInAdmin.ItemsSource = sortBySurName;
                        }
                        else if (cmbSortBy.SelectedItem == "Email")
                        {
                            var sortByEmail = from b in bdList
                                              orderby b.Email
                                              select b;
                            UserInAdmin.ItemsSource = sortByEmail;
                        }
                        else if (cmbSortBy.SelectedItem == null)
                        {
                            UserInAdmin.ItemsSource = db.User.Where(itemF => itemF.RoleId == cmbRole.SelectedItem).ToList();
                        }
                        else
                        {
                            UserInAdmin.ItemsSource = db.User.ToList();
                        }
                    }
                }
                else
                {
                    UserInAdmin.ItemsSource = db.User.Where(itemF => itemF.FirstName == txbFind.Text).ToList();
                }

            }

            private void btnClear_Click(object sender, RoutedEventArgs e)
            {
                UserInAdmin.ItemsSource = db.User.ToList();
                cmbRole.SelectedItem = null;
                cmbSortBy.SelectedItem = null;
            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                //User user = UserInAdmin.SelectedValue as User;
                //AdminEditUser aed = new AdminEditUser(user);
                //this.NavigationService.Navigate(aed);
            }
        }
    
}
