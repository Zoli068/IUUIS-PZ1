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
using Notification;
using Notification.Wpf;
using FontAwesome5;
using PZ1.Model;
using PZ1.Helpers;
using FontAwesome5;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;


namespace PZ1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CMSWindow : Window
    {
        private ObservableCollection<Movie> movies = null;
        public ObservableCollection<Movie> Movies { get { return movies; } set { movies = value; } }

        private DataIO serializer=new DataIO();
        public User LoggedInUser {  get; set; }
        public CMSWindow(User loggedInUser)
        {
            CMSWindowStartUp(loggedInUser);

            InitializeComponent();

            DenyPermission(loggedInUser);
        }

        private void CMSWindowStartUp(User loggedInUser)
        {
            serializer.SerializeObject<ObservableCollection<Movie>>(Movies, "MovieCollection.xml");

            movies = serializer.DeSerializeObject<ObservableCollection<Movie>>("MovieCollection.xml");

            if (movies.Equals(null))
            {
                movies = new ObservableCollection<Movie>();
            }

            LoggedInUser = loggedInUser;

            DataContext = this;
        }

        private void DenyPermission(User loggedInUser)
        {
            if (loggedInUser.Role.Equals(UserRole.Visitor)){

                this.DeleteMovieButton.Visibility = Visibility.Hidden;
                this.NewMovieButton.Visibility = Visibility.Hidden;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

            //saving the movies back to the xml
            //checking are other windows are still openned


            Close();
        }

        private void CheckBoxDeleteSelection_Click(object sender, RoutedEventArgs e)
        {
            Movie movie = (sender as FrameworkElement).DataContext as Movie;


            if ( movie.IsChecked == false)
            {
                movie.IsChecked = true;
            }
            else
            {
                movie.IsChecked = false;
            }

        }

        private void DeleteMovieButton_Click(object sender, RoutedEventArgs e)
        {

            for(int i=Movies.Count-1; i>=0; i--) 
            {
                if(Movies.ElementAt(i).IsChecked == true)
                {
                    Movies.RemoveAt(i);
                }
            }

        }


    }
}
