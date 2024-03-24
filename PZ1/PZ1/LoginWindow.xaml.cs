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
using System.Windows.Shapes;
using FontAwesome5;
using PZ1.Model;
using PZ1.Helpers;
using System.Threading;
using Notification.Wpf.Constants;
using System.IO;

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

        private double leftPosition=double.NaN, topPosition;

        public LoginWindow()
        {
            allUserCredentials = serializer.DeSerializeObject<List<User>>("UserCredentials.xml");

            if (allUserCredentials==null)
            {

                MessageBox.Show("Missing application files!","Error",MessageBoxButton.OK);

                throw new Exception("CorruptedUserFiles");

            }


            InitializeComponent();

            this.UserNameTextBox.Text = "Admin";
            this.PasswordTextBox.Password = "admin123";
            LoginButton_Click(null, null);


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
                Hide();            

                CMSWindow CMSWindow = new CMSWindow(LoggedInUser);
                CMSWindow.Owner = this;
                CMSWindow.ShowDialog();

                RemoveLoginValues();
                
                Show();

                this.Left = leftPosition; 
                this.Top=topPosition;
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
            this.InvalidLoginAttemptLabel.Foreground = Brushes.Black;
        }

        private bool LoginAttempt(string userName, string password)
        {

            if (userName.Trim().Equals(string.Empty) && password.Length.Equals(0))
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
                            break;                 
                        }
                        else
                        {
                            LoggedInUser = user;
                            return true;           
                        }
                    }
                }

                RemoveErrorMessages();
                this.UserNameTextBox.BorderBrush = Brushes.Red;
                this.PasswordTextBox.BorderBrush = Brushes.Red;
                this.InvalidLoginAttemptLabel.Content = "Invalid Username or Password ";
                this.InvalidLoginAttemptLabel.Foreground = Brushes.Red;

                return false;                    
            }

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(LoginButton), null);
            Keyboard.ClearFocus();
            if (leftPosition.Equals(double.NaN))
            {
                leftPosition = this.Left;
                topPosition = this.Top;
            }

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
