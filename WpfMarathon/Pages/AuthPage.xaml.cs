﻿using System;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {

        static MainWindow _mainWindow;
        public static MarafonEntities db = new MarafonEntities();
        public AuthPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_Reg_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;
            if (txb_email.Text != "" && txb_pass.Password != "" && txb_email.Text != "Email")
            {
                if (Validate.IsValidEmail(txb_email.Text))
                {
                    try
                    {
                        List<User> users = db.User.ToList();
                        foreach (var us in users)
                        {
                            if (us.Email == txb_email.Text && us.Password == txb_pass.Password)
                            {

                                i = 0;
                                if (us.RoleId == "R")
                                {
                                    var runner = db.Runner.FirstOrDefault(x=> x.Email == us.Email).RunnerId;
                                    _mainWindow.MainFrame.NavigationService.Navigate(new RunnerMenu(_mainWindow, runner));
                                    j = 1;
                                }
                                if (us.RoleId == "A")
                                {
                                    _mainWindow.MainFrame.NavigationService.Navigate(new AdminMenu(_mainWindow));
                                    j = 1;
                                }
                                if (us.RoleId == "C")
                                {
                                    _mainWindow.MainFrame.NavigationService.Navigate(new MenuCoordinator(_mainWindow)); 
                                    j = 1;
                                }
                            }
                            else
                            {
                                i = 1;
                            }
                        }
                        if (i == 1)
                        {
                            if (j == 0)
                            {
                                MessageBox.Show("Нет совпадений", "Ошибка");
                            }

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось авторизироваться", "Ошибка");
                    }
                }
                else
                {
                    MessageBox.Show("Нарушена валидация e-mail", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка");
            }
        }

        private void txb_email_GotFocus(object sender, RoutedEventArgs e)
        {
            txb_email.Text = "";
        }

    }
}
