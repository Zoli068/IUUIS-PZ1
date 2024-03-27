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
using Notification;
using Notification.Wpf;
using FontAwesome5;
using PZ1.Model;
using PZ1.Helpers;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Reflection.Emit;
using Notification.Wpf.Constants;
using Notification.Wpf.Base;
using System.IO;
using Notification.Wpf.Controls;
using System.Media;


namespace PZ1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CMSWindow : Window
    {
        private ObservableCollection<Movie> movies = null;

        public ObservableCollection<Movie> Movies { get { return movies; } set { movies = value; } }

        private NotificationManager notificationManager = new NotificationManager();

        private DispatcherTimer dispatcherTimer;

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

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);

            string FontFamilyPath = Directory.GetCurrentDirectory();

            NotificationConstants.FontName = (FontFamilyPath + "\\#Star Jedi");


        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
           // NoneSelectedErrorTextBlock.Visibility = System.Windows.Visibility.Hidden;

            dispatcherTimer.IsEnabled = false;
        }

        private void DenyPermission(User loggedInUser)
        {
            if (loggedInUser.Role.Equals(UserRole.Visitor)){

                this.DeleteMovieButton.Visibility = Visibility.Hidden;
                this.NewMovieButton.Visibility = Visibility.Hidden;
                this.CheckBoxColumn.Visibility = Visibility.Hidden;
                this.ImageColumn.Width = 317;
                this.TitleColumn.Width = 395.5;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.MoviesDataGrid.UnselectAll();

            DragMove();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

            BooleanWrapper booleanWrapper = new BooleanWrapper();

            ApprovalWindow exitApprovalWindow = new ApprovalWindow(booleanWrapper, "Are you sure you want to logout?");

            exitApprovalWindow.ShowDialog();

            if (booleanWrapper.Value == false)
            {
                return;
            }

            serializer.SerializeObject<ObservableCollection<Movie>>(Movies, "MovieCollection.xml");

            Close();
        }

        private void CheckBoxDeleteSelection_Click(object sender, RoutedEventArgs e)
        {
            Movie movie = (sender as FrameworkElement).DataContext as Movie;


            //If we got error, but after that the user select some item
            //then we can hide the error
            //this.NoneSelectedErrorTextBlock.Visibility = Visibility.Hidden;
            //dispatcherTimer.IsEnabled = false;


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
            bool approved = false;
            int numOfSelection = 0;
            Movie movie;
            for (int i = Movies.Count - 1; i >= 0; i--)
            {
                if (Movies.ElementAt(i).IsChecked == true)
                {
                    numOfSelection++;
                   // this.NoneSelectedErrorTextBlock.Visibility = Visibility.Hidden;

                    if (numOfSelection == 1 && approved==false)
                    {

                        BooleanWrapper booleanWrapper = new BooleanWrapper();
                        ApprovalWindow movieDeleteApproval = new ApprovalWindow(booleanWrapper, "Are you sure you want to delete the selected items ?");
                        movieDeleteApproval.Owner = this;

                        SystemSounds.Beep.Play();

                        movieDeleteApproval.ShowDialog();

                        if (booleanWrapper.Value==false)
                        {
                            break;
                        }
                        else
                        {
                            approved = true;
                            var content = new NotificationContent
                             {
                                 Title = "Success",
                                 Message = "Selected items Got deleted",
                                 Type = NotificationType.Success,
                                 TrimType = NotificationTextTrimType.NoTrim,
                                 Background = new SolidColorBrush(Colors.Blue),
                                 Foreground= new SolidColorBrush(Colors.White),
                                 CloseOnClick=false,


                                 Icon = new SvgAwesome()
                                 {
                                     Icon = EFontAwesomeIcon.Solid_TrashAlt,
                                     Height = 50,
                                     Foreground = new SolidColorBrush(Colors.White)
                                 },
                             };

                             notificationManager.Show(content, "CMSWindowNotificationArea", ShowXbtn: false, expirationTime: new TimeSpan(0, 0, 5));
                        }
                    
                    }

                         movie = Movies.ElementAt(i);

                         File.Delete(@movie.DescriptionPath);

                         Movies.RemoveAt(i);
                    
                }
            }


            if (numOfSelection == 0)
            {
                //this.NoneSelectedErrorTextBlock.Visibility = Visibility.Visible;

                var content = new NotificationContent
                {
                    Title = "Error",
                    Message = "No Movies Selected For Delete",
                    Type = NotificationType.Error,
                    TrimType = NotificationTextTrimType.NoTrim,
                    Background = new SolidColorBrush(Colors.Red),
                    CloseOnClick=false,


                    Icon = new SvgAwesome()
                    {
                        Icon = EFontAwesomeIcon.Solid_Ban,
                        Height = 50,
                        Foreground = new SolidColorBrush(Colors.Black)
                    },
                };

                notificationManager.Show(content, "CMSWindowNotificationArea", ShowXbtn: false, expirationTime: new TimeSpan(0, 0, 5));
              
                //dispatcherTimer.Start();

            }


        }

        private void TitleHyperLink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            TextBlock senderTextBlock = hyperlink.Parent as TextBlock;

            Movie movie = senderTextBlock.DataContext as Movie;

            if (LoggedInUser.Role == UserRole.Admin)
            {
                if (movie.IsOpened == false)
                {
                    BooleanWrapper booleanWrapper=new BooleanWrapper();
                    AddOrEditingMovieWindow addOrEditingMovieWindow = new AddOrEditingMovieWindow(movie, booleanWrapper);
                    addOrEditingMovieWindow.Owner= this;
                    addOrEditingMovieWindow.ShowDialog();

                    if (booleanWrapper.Value)
                    {
                        var content = new NotificationContent
                        {
                            Title = "Success",
                            Message ="Movie got edited",
                            Type = NotificationType.Success,
                            TrimType = NotificationTextTrimType.NoTrim,
                            Background = new SolidColorBrush(Colors.Green),
                            Foreground = new SolidColorBrush(Colors.White),
                            CloseOnClick = false,


                            Icon = new SvgAwesome()
                            {
                                Icon = EFontAwesomeIcon.Solid_Check,
                                Height = 50,
                                Foreground = new SolidColorBrush(Colors.White)
                            },
                        };
                        notificationManager.Show(content, "CMSWindowNotificationArea", ShowXbtn: false, expirationTime: new TimeSpan(0, 0, 5));

                    }
                }
                else
                {
                    var content = new NotificationContent
                    {
                        Title = "Reminder",
                        Message = "Movie already opened for editing",
                        Type = NotificationType.Warning,
                        TrimType = NotificationTextTrimType.NoTrim,
                        Background = new SolidColorBrush(Colors.Orange),
                        Foreground = new SolidColorBrush(Colors.White),
                        CloseOnClick = false,


                        Icon = new SvgAwesome()
                        {
                            Icon = EFontAwesomeIcon.Solid_Exclamation,
                            Height = 70,
                            Foreground = new SolidColorBrush(Colors.Black)
                        },
                    };

                    notificationManager.Show(content, "CMSWindowNotificationArea", ShowXbtn: false, expirationTime: new TimeSpan(0, 0, 5));
                    //dispatcherTimer.Start();
                }

            }
            else
            {
                if (movie.IsOpened == false)
                {
                    MovieDetailsWindow movieDetailsWindow = new MovieDetailsWindow(movie);
                    movieDetailsWindow.Owner = this;
                    movieDetailsWindow.Show();
                }
                else
                {

                    var content = new NotificationContent
                    {
                        Title = "Reminder",
                        Message = "Movie already opened for detailed viewing",
                        Type = NotificationType.Warning,
                        TrimType = NotificationTextTrimType.NoTrim,
                        Background = new SolidColorBrush(Colors.Orange),
                        Foreground = new SolidColorBrush(Colors.White),
                        CloseOnClick = false,


                        Icon = new SvgAwesome()
                        {
                            Icon = EFontAwesomeIcon.Solid_Exclamation,
                            Height = 70,
                            Foreground = new SolidColorBrush(Colors.Black)
                        },
                    };

                    notificationManager.Show(content, "CMSWindowNotificationArea", ShowXbtn: false, expirationTime: new TimeSpan(0, 0, 5));
                   // dispatcherTimer.Start();
                }
            }

        }

        private void NewMovieButton_Click(object sender, RoutedEventArgs e)
        {

            BooleanWrapper booleanWrapper = new BooleanWrapper();

            AddOrEditingMovieWindow addOrEditingMovieWindow = new AddOrEditingMovieWindow(Movies,booleanWrapper);
            addOrEditingMovieWindow.Owner = this;

            //bcs we have to give feedback with toast, so isOpened at movies at admin its actually useless thing
            addOrEditingMovieWindow.ShowDialog(); 

            if(booleanWrapper.Value)
            {
                //success
                var content = new NotificationContent
                {
                    Title = "Success",
                    Message = "Movie got added",
                    Type = NotificationType.Success,
                    TrimType = NotificationTextTrimType.NoTrim,
                    Background = new SolidColorBrush(Colors.Green),
                    Foreground = new SolidColorBrush(Colors.White),
                    CloseOnClick = false,


                    Icon = new SvgAwesome()
                    {
                        Icon = EFontAwesomeIcon.Solid_Plus,
                        Height = 50,
                        Foreground = new SolidColorBrush(Colors.White)
                    },
                };

                notificationManager.Show(content, "CMSWindowNotificationArea", ShowXbtn: false, expirationTime: new TimeSpan(0, 0, 5));

            }

        }
    }
}
