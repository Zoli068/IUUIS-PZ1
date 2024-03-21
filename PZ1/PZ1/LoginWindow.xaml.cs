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
using System.Windows.Shapes;
using FontAwesome5;
using PZ1.Model;
using PZ1.Helpers;
using System.Threading;

//Admin - admin123
//Visitor - visitor123

namespace PZ1
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private DataIO serializer = new DataIO();

        private List<User> allUserCredentials;

        public User LoggedInUser { get; set; }

        public List<User> AllUserCredentials { get { return allUserCredentials; } }

        public LoginWindow()
        {
            allUserCredentials = serializer.DeSerializeObject<List<User>>("UserCredentials.xml");

            if (allUserCredentials==null)
            {

                MessageBox.Show("Missing application files!","Error",MessageBoxButton.OK);

                throw new Exception("CorruptedUserFiles");

            }


            InitializeComponent();

            //dev purpose
            this.UserNameTextBox.Text = "Admin";
            this.PasswordTextBox.Password = "admin123";
            LoginButton_Click(null, null);
            //
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            string userName = this.UserNameTextBox.Text;
            string password = this.PasswordTextBox.Password.ToString();

            if(LoginAttempt(userName, password))
            {
                Hide();             //Hide the login window at successfull atempt

                CMSWindow CMSWindow = new CMSWindow(LoggedInUser);
                CMSWindow.ShowDialog();

                RemoveLoginValues();

                Show();             //When we return from the CMS then we start showing the login windows again
            }

        }

        private void RemoveLoginValues()
        {
            LoggedInUser = null;
            this.PasswordTextBox.Password = string.Empty;
            this.UserNameTextBox.Text = string.Empty;
        }

        private void RemoveErrorMessages()
        {
            this.UserNameTextBox.BorderBrush = Brushes.White;
            this.PasswordTextBox.BorderBrush = Brushes.White;
            this.InvalidLoginAttemptLabel.Foreground = Brushes.White;
        }

        private bool LoginAttempt(string userName, string password)
        {
            if(userName.Trim().Equals(string.Empty) && password.Length.Equals(0))
            {
                RemoveErrorMessages();
                this.UserNameTextBox.BorderBrush = Brushes.Red;
                this.PasswordTextBox.BorderBrush = Brushes.Red;
                this.InvalidLoginAttemptLabel.Content = "Username and Password can not be blank";
                this.InvalidLoginAttemptLabel.Foreground = Brushes.Red;

                return false;

            }
            else if(!userName.Trim().Equals(string.Empty) && password.Length.Equals(0))
            {
                RemoveErrorMessages();
                this.PasswordTextBox.BorderBrush = Brushes.Red;
                this.InvalidLoginAttemptLabel.Content = "Password can not be blank";
                this.InvalidLoginAttemptLabel.Foreground = Brushes.Red;

                return false;
            }
            else if(userName.Trim().Equals(string.Empty) && !password.Length.Equals(0))
            {
                RemoveErrorMessages();
                this.UserNameTextBox.BorderBrush = Brushes.Red;
                this.InvalidLoginAttemptLabel.Content = "Username can not be blank";
                this.InvalidLoginAttemptLabel.Foreground = Brushes.Red;

                return false;
            }
            else
            {

                foreach(User user in allUserCredentials)
                {
                    if (user.UserName.Equals(userName))
                    {
                        if (!user.Password.Equals(password))
                        {
                            break;                  //Found the username but password didn't match
                        }
                        else
                        {
                            LoggedInUser = user;
                            return true;            //Successfully logged in
                        }
                    }
                }

                RemoveErrorMessages();
                this.UserNameTextBox.BorderBrush = Brushes.Red;
                this.PasswordTextBox.BorderBrush = Brushes.Red;
                this.InvalidLoginAttemptLabel.Content = "Invalid Username or Password ";
                this.InvalidLoginAttemptLabel.Foreground = Brushes.Red;

                return false;                       //couldn't find an user with that username
            }

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(LoginButton), null);
            Keyboard.ClearFocus();
            this.DragMove();
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(null, null);
            }
        }

        private void UserNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.PasswordTextBox.Focus();
            }

        }
    }
}