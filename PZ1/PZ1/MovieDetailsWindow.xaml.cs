using PZ1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for MovieDetailsWindow.xaml
    /// </summary>
    public partial class MovieDetailsWindow : Window
    {
        public Movie MovieDetail { get; set; }

        public MovieDetailsWindow(Movie movie)
        {
            movie.IsOpened = true;
            MovieDetail = movie;

            InitializeComponent();

            WindowStartup();

            DataContext = this;
        }

        private void WindowStartup()
        {
            FileStream streamFromRtfFile = new FileStream(@MovieDetail.DescriptionPath, System.IO.FileMode.Open);
            this.RichTextBoxDescription.Selection.Load(streamFromRtfFile, DataFormats.Rtf);
            streamFromRtfFile.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MovieDetail.IsOpened = false;
            Close();
        }
    }
}
