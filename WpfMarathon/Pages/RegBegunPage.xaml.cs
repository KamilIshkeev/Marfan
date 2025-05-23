using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using WpfMarathon;

namespace WpfMarathon.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegBegunPage.xaml
    /// </summary>
    public partial class RegBegunPage : Page
    {

        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        public RegBegunPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            var gender = new List<string>();
            gender = db.Gender.Select(g=> g.Gender1).ToList();
            var country = new List<string>();
            country = db.Country.Select(c=> c.CountryCode).ToList();
            cmb_gender.ItemsSource = gender;
            cmbCountry.ItemsSource = country;
        }

        private void btn_source_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.FilterIndex = 2;
            open.Filter = "jpg|*.jpg| png|*.png";

            if (open.ShowDialog() == true)
            {
                BitmapImage source = new BitmapImage();
                source.BeginInit(); // начало считывания фото
                source.UriSource = new Uri(@"" + open.FileName, UriKind.Relative);
                source.CacheOption = BitmapCacheOption.OnLoad; //Задержка
                source.EndInit();

                imgAvatar.Source = source;
                imgAvatar.Stretch = Stretch.Uniform;
            }
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void btn_Reg_Click(object sender, RoutedEventArgs e)
        {
            if (txb_email.Text != "" && txb_name.Text != "" && txb_pass.Text != "" && txb_repeatpass.Text != "" && txb_surname.Text != "" && txb_email.Text != "Email" && txb_surname.Text != "Фамилия")
            {
                var check = (from b in db.User
                             where b.Email == txb_email.Text
                             select b).SingleOrDefault();
                if (check == null)
                {
                    if (txb_pass.Text == txb_repeatpass.Text)
                    {
                        if (Validate.IsValidEmail(txb_email.Text))
                        {
                            if (Validate.IsString(txb_name.Text))
                            {
                                if (Validate.IsString(txb_surname.Text))
                                {
                                    try
                                    {
                                        User user = new User
                                        {
                                            Email = txb_email.Text,
                                            Password = txb_pass.Text,
                                            FirstName = txb_name.Text,
                                            LastName = txb_surname.Text,
                                            RoleId = "R",
                                        };

                                        db.User.Add(user);
                                        db.SaveChanges();
                                        Runner runner = new Runner
                                        {
                                            Email = user.Email,
                                            Gender = cmb_gender.SelectedItem.ToString(),
                                            CountryCode = cmbCountry.Text,
                                            DateOfBirth = dateBirth.SelectedDate,
                                        };

                                        db.Runner.Add(runner);
                                        db.SaveChanges();
                                        _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Не удалось зарегистрирвоать пользователя", "Ошибка");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Нарушена валидация фамилии", "Ошибка");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Нарушена валидация имени", "Ошибка");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Нарушена валидация e-mail", "Ошибка");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пароль не совпадает");
                    }
                }
                else
                {
                    MessageBox.Show("Такой e-mail уже существует", "Ошибка");
                }

            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.NavigationService.Navigate(new AuthRegRunner(_mainWindow));
        }

        private void txb_email_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_email.Text = "";
        }

        private void txb_pass_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_pass.Text = "";
        }

        private void txb_repeatpass_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_repeatpass.Text = "";
        }

        private void txb_name_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_name.Text = "";
        }

        private void txb_surname_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_surname.Text = "";
        }

        private void txb_pathphoto_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_pathphoto.Text = "";
        }

    }
}
