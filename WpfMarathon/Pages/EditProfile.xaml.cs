using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Page
    {
        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        public Runner rn;
        public EditProfile(MainWindow mainWindow,List<User> user)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            var runner = db.Runner.FirstOrDefault(x=> x.Email == user.Select(x1 => x1.Email).SingleOrDefault());
            rn = runner;
            string urgender = runner.Gender;
            string urcountry = runner.CountryCode;
            txt_email.Text = user.Select(x => x.Email).SingleOrDefault();
            txb_name.Text = user.Select(x => x.FirstName).SingleOrDefault();
            txb_surname.Text = user.Select(x => x.LastName).SingleOrDefault();
            List<string> gender = new List<string> { "Male", "Female" };
            cmb_gender.ItemsSource = gender;
            cmb_gender.SelectedValue = gender.Find(x => x.Length == urgender.Length);
            dateBirth.SelectedDate = runner.DateOfBirth;
            var country = new List<string> { "Andora", "Argentina", "Australia", "Austria", "Belgium", "Botswana", "Brazil", "Bulgaria", "Cameroon", "Canada", "Central Africa", "Chile", "China", "Chinese Taipei", "Colombia", "Croatia", "Czech Republic", "Denmark", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Esonia", "Finland", "France", "Germany", "Greece", "Guatemala", "Guinea", "Guinea-Bissau", "Honduras", "Hong Kong", "Hungary", "India", "Indonesia", "Ireland", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kenya", "Latvia", "Liechtenstein", "Lithuania", "Luxemburg", "Macau", "Macedonia", "Madagascar", "Malaysia", "Mali", "Malta", "Mauritius", "Mexico", "Moldova", "Monaco", "Montenegro", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Norway", "Panama", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Romania", "Russia", "Saudi Arabia", "Senegal", "Singapore", "Slovakia", "South Africa", "South Korea", "Spain", "Sweden", "Switzerland", "Thailand", "Turkey", "Unitied Arab Emirates", "Inited Kingdom", "Uruguay", "USA", "Vatican", "Venezuela", "Vietnam" };
            cmbCountry.ItemsSource = country;
            cmbCountry.SelectedValue = country.Find(x => x == urcountry);
            //imgAvatar.Source = Img.ToBitmapImage(user.Select(x => x.Photo).SingleOrDefault());
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (txb_name.Text != "" && txb_pass.Password != "" && txb_repeatpass.Password != "" && txb_surname.Text != "")
            {
                if (txb_pass.Password == txb_repeatpass.Password)
                {
                    if (Validate.IsString(txb_name.Text))
                    {
                        if (Validate.IsString(txb_surname.Text))
                        {
                            User user = new User
                            {
                                Email = txb_name.Text,
                                Password = txb_pass.Password,
                                FirstName = txb_name.Text,
                                LastName = txb_surname.Text,
                            };
                            Runner runner = new Runner
                            {
                                RunnerId = rn.RunnerId,
                                Email = txb_name.Text,
                                Gender = cmb_gender.SelectedItem.ToString(),
                                CountryCode = cmbCountry.Text,
                                DateOfBirth = dateBirth.SelectedDate,
                            };
                            db.User.AddOrUpdate(user);
                            db.Runner.AddOrUpdate(runner);
                            db.SaveChanges();
                            _mainWindow.MainFrame.NavigationService.Navigate(new AuthPage(_mainWindow));
                        }
                        else
                        {
                            MessageBox.Show("Нарушена валидация e-mail", "Ошибка");
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
                MessageBox.Show("Не все поля заполнены");
            }

        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
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

        private void txb_pass_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_pass.Password = "";
        }

        private void txb_repeatpass_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_repeatpass.Password = "";
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

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
