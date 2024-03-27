using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Controls.Primitives;
using PZ1.Model;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for AddOrEditingWindow.xaml
    /// </summary>
    public partial class AddOrEditingMovieWindow : System.Windows.Window, INotifyPropertyChanged
    {
        //values for the new movie or the one we editing
        private Uri imageSource = null;
        private string imagePath="";

        //values for some methods
        private DispatcherTimer dispatcherTimer;
        public event PropertyChangedEventHandler PropertyChanged;

        //values for the window view
        private int numOfWord;

        //values which we got from CMSWindow
        public ObservableCollection<Movie> Movies;
        public Movie movie;

        //we'll here save the return value (success/fail)
        public BooleanWrapper booleanWrapper;

        //flag values
        private bool isSetUp = true;
        private bool editOrAdd = false;

        //default values for textEditor
        private Brush fontColor;
        private int defaultFontSize = 16;
        private FontFamily defaultFontFamily = new FontFamily("Calibri");
        private string defaultFontColor = "White";

        public AddOrEditingMovieWindow(ObservableCollection<Movie> movies,BooleanWrapper booleanWrapper)
        {
            this.booleanWrapper = booleanWrapper;

            isSetUp = false;
            Movies = movies;

            InitializeComponent();

            DataContext = this;

            StartUp();
        }

        public AddOrEditingMovieWindow(Movie movie, BooleanWrapper booleanWrapper) 
        {
            this.booleanWrapper = booleanWrapper;
            this.movie = movie;

            InitializeComponent();

            DataContext = this;

            StartUp();
        }


         private void StartUp()
         {

            FontFamilyComboBox.ItemsSource = System.Windows.Media.Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontFamilyComboBox.SelectedValue = defaultFontFamily;

            var values = typeof(Brushes).GetProperties().Select(p => p.Name).ToArray();
            FontColorComboBox.ItemsSource = values;

            FontColorComboBox.SelectedValue = defaultFontColor;
            fontColor = Brushes.White;
            RichTextBox.Foreground = fontColor;

            FontSizeTextBox.Text = defaultFontSize.ToString();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 6);

            if (isSetUp == true)
            {
                this.WindowTitle.Content = "Editing a Movie";

                editOrAdd = true;
                this.movie.IsOpened = true;

                this.TitleTextBox.Text = movie.Title;

                this.RatingTextBox.Text = movie.Rating.ToString();

                this.imagePath = movie.ImagePath;
                this.ImageSource = movie.ImageUri;
                this.PreviewImage.Visibility = Visibility.Visible;
                this.PreviewImageDropArea.Visibility = Visibility.Collapsed;

                this.AddButton.Content = "Edit";
                this.AddButton.ToolTip = "Edit a Movie";
                this.CancelButton.ToolTip = "Cancel the editing";

                FileStream streamFromRtfFile = new FileStream(@movie.DescriptionPath, System.IO.FileMode.Open);
                this.RichTextBox.Selection.Load(streamFromRtfFile, System.Windows.DataFormats.Rtf);
                streamFromRtfFile.Close();

                isSetUp = false;
            }
        }

        public int NumOfWord
        {
            get
            {
                return numOfWord;
            }

            set
            {
                numOfWord = value;
                OnPropertyChanged();
            }
        }

        public Brush FontColor
        {
            get
            {
                return fontColor;
            }

            set
            {
                fontColor = value;
                OnPropertyChanged();
            }
        }

        public Uri ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
                OnPropertyChanged();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.TitleErrorLabel.Foreground = Brushes.Black;
            this.ImageErrorLabel.Foreground = Brushes.Black;
            this.RatingErrorTextBlock.Foreground = Brushes.Black;

            dispatcherTimer.IsEnabled = false;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PreviewImage_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);

                string extension = System.IO.Path.GetExtension(files[0]);

                if (extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".gif") || extension.Equals(".jpeg"))
                {
                    imagePath = files[0];
                    ImageSource = new Uri("pack://" + files[0], UriKind.Absolute);

                    this.ImageErrorLabel.Foreground = Brushes.Black;
                    this.PreviewImage.Visibility = Visibility.Visible;
                    this.PreviewImageDropArea.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.ImageErrorLabel.Content = "Invalid File Format";
                    this.ImageErrorLabel.Foreground = Brushes.Red;
                    dispatcherTimer.Stop();
                    dispatcherTimer.Start();
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this.FontSizeTextBox), null);
            Keyboard.ClearFocus();
            DragMove();
        }

        private void ImageSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".jpg";
            dialog.Filter = "Images|*.jpg;*.jpeg;*.png;*.gif;";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                imagePath = dialog.FileName;
                ImageSource = new Uri("pack://" + dialog.FileName, UriKind.Absolute);

                this.ImageErrorLabel.Foreground = Brushes.Black;
                this.PreviewImage.Visibility = Visibility.Visible;
                this.PreviewImageDropArea.Visibility = Visibility.Collapsed;
            }
        }

        private void EditorToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ToolBar toolBar = sender as System.Windows.Controls.ToolBar;

            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;

            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;

            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox.Redo();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            RichTextBox.Undo();
        }

        private void RichTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key.Equals(Key.Space))
            {
                System.Windows.Controls.RichTextBox rtb = sender as System.Windows.Controls.RichTextBox;
                TextPointer tp = rtb.CaretPosition;

                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(rtb), null);
                Keyboard.ClearFocus();

                rtb.Focus();
                Keyboard.Focus(rtb);

                RichTextBox.CaretPosition = tp;
            }
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange textRange = new TextRange(RichTextBox.Document.ContentStart, RichTextBox.Document.ContentEnd);

            string[] splittedLines = textRange.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            int numofWord = 0;

            foreach (string s in splittedLines)
            {
                string[] stringParts = s.Split(' ');

                if(stringParts.ElementAt(stringParts.Length-1).Equals(""))
                {

                    numofWord += stringParts.Count()-1;

                    continue;
                }

                numofWord += stringParts.Count();
            }

            NumOfWord = numofWord;
        }

        private void FontSizeTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this.FontSizeTextBox), null);
        }

        private void FontColorComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this.FontColorComboBox), null);
        }

        private void FontFamilyComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this.FontFamilyComboBox), null);
        }

        private void FontSizeTextBox_TextChanged(object sender, RoutedEventArgs e)
        {

            if (isSetUp == false)
            {
                int value;

                if (int.TryParse(FontSizeTextBox.Text, out value))
                {
                    if (value > 0 && value < 1200)
                    {
                        double fontValue = value;

                        if (FontSizeTextBox.Text != null && !RichTextBox.Selection.IsEmpty)
                        {
                            RichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontValue);

                            if (!RichTextBox.Selection.Text.ElementAt(RichTextBox.Selection.Text.Length - 1).Equals(' '))
                            {
                                TextRange textRange = new TextRange(RichTextBox.Selection.End, RichTextBox.Selection.End);
                                textRange.Text = " ";
                                textRange.ApplyPropertyValue(Inline.FontSizeProperty, double.Parse(defaultFontSize.ToString()));
                            }
                        }
                        else
                        {
                            FontSize = fontValue;
                            defaultFontSize = int.Parse(fontValue.ToString());
                            RichTextBox.FontSize = fontValue;
                            FontSizeTextBox.Text = FontSize.ToString();
                        }
                    }
                    else
                    {
                        FontSizeTextBox.Text = defaultFontSize.ToString();
                    }
                }
                else
                {
                    FontSizeTextBox.Text = defaultFontSize.ToString();
                }
            }
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSetUp == false)
            {
                if (FontFamilyComboBox.SelectedItem != null && !RichTextBox.Selection.IsEmpty)
                {
                    RichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyComboBox.SelectedItem);

                    if (!RichTextBox.Selection.Text.ElementAt(RichTextBox.Selection.Text.Length - 1).Equals(' '))
                    {
                        TextRange textRange = new TextRange(RichTextBox.Selection.End, RichTextBox.Selection.End);
                        textRange.Text = " ";
                        textRange.ApplyPropertyValue(Inline.FontFamilyProperty, defaultFontFamily);
                    }
                }

                if (FontFamilyComboBox.SelectedItem != null && RichTextBox.Selection.IsEmpty)
                {
                    defaultFontFamily = FontFamilyComboBox.SelectedItem as FontFamily;
                    RichTextBox.FontFamily = defaultFontFamily;
                }
            }
        }


        private void FontColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSetUp == false)
            {
                FontColor = (Brush)new BrushConverter().ConvertFromString(FontColorComboBox.SelectedItem.ToString());

                if (FontColorComboBox.SelectedItem != null && !RichTextBox.Selection.IsEmpty)
                {
                    RichTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, FontColor);

                    if (!RichTextBox.Selection.Text.ElementAt(RichTextBox.Selection.Text.Length - 1).Equals(' '))
                    {
                        TextRange textRange = new TextRange(RichTextBox.Selection.End, RichTextBox.Selection.End);
                        textRange.Text = " ";
                        textRange.ApplyPropertyValue(Inline.ForegroundProperty, (Brush)new BrushConverter().ConvertFromString(defaultFontColor));
                    }
                }
                else if (FontColorComboBox.SelectedItem != null && RichTextBox.Selection.IsEmpty)
                {
                    defaultFontColor = FontColorComboBox.SelectedItem.ToString();
                    RichTextBox.Foreground = FontColor;
                }
            }
        }

        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (isSetUp == false)
            {
                if (RichTextBox.Selection.IsEmpty)
                {
                    FontColorComboBox.SelectedItem = defaultFontColor;
                    FontSizeTextBox.Text = defaultFontSize.ToString();
                    FontFamilyComboBox.SelectedItem = defaultFontFamily;
                }

                TextRange textRange;

                try
                {
                    textRange = new TextRange(RichTextBox.CaretPosition, RichTextBox.CaretPosition.GetNextContextPosition(LogicalDirection.Backward));
                }
                catch (Exception ) 
                { 
                    return; 
                }

                FontWeight fontWeight = (FontWeight)textRange.GetPropertyValue(FontWeightProperty);
                FontStyle fontStyle=(FontStyle)textRange.GetPropertyValue(FontStyleProperty);

                if (fontWeight.ToString().Equals("Bold"))
                {
                    BoldToggleButton.IsChecked = true;
                }
                else
                {
                    BoldToggleButton.IsChecked = false;
                }

                if (fontStyle.ToString().Equals("Italic"))
                {
                    ItalicToggleButton.IsChecked = true;
                }
                else
                {
                    ItalicToggleButton.IsChecked = false;
                }

                if (textRange.GetPropertyValue(Underline.TextDecorationsProperty).Equals(TextDecorations.Underline))
                {
                    UnderLineToggleButton.IsChecked = true;
                }
                else
                {
                    UnderLineToggleButton.IsChecked = false;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            bool error=false;
            double rating=0;

            if (imagePath == "")
            {
                error= true;
                this.ImageErrorLabel.Content = "Select an Image";
                this.ImageErrorLabel.Foreground = Brushes.Red;
            }
            else
            {
                this.ImageErrorLabel.Foreground = Brushes.Black;
            }

            if (this.TitleTextBox.Text.Trim().Equals(string.Empty))
            {
                error=true;
                this.TitleErrorLabel.Foreground = Brushes.Red;
            }
            else
            {
                this.TitleErrorLabel.Foreground = Brushes.Black;
            }

            if (this.RatingTextBox.Text.Trim().Equals(string.Empty))
            {
                error = true;
                this.RatingErrorTextBlock.Foreground = Brushes.Red;
            }
            else
            {
                string ratingValue=this.RatingTextBox.Text;

                ratingValue=ratingValue.Replace(',', '.');

                if (!double.TryParse(ratingValue, out rating))
                {

                    if(rating<0 || rating > 10)
                    {
                        error = true;
                        this.RatingErrorTextBlock.Foreground = Brushes.Red;
                    }
                    else
                    {
                        this.RatingErrorTextBlock.Foreground = Brushes.Black;
                    }
                }
                else
                {
                    this.RatingErrorTextBlock.Foreground = Brushes.Red;
                }
            }

            if (error)
            {
                dispatcherTimer.Stop();
                dispatcherTimer.Start();
                return;
            }

            TextRange range;
            FileStream fStream;
            range = new TextRange(RichTextBox.Document.ContentStart, RichTextBox.Document.ContentEnd);

            if (editOrAdd == false)
            {
                var invalidChars = System.IO.Path.GetInvalidFileNameChars();

                string invalidCharsRemoved = new string(this.TitleTextBox.Text
                      .Where(x => !invalidChars.Contains(x))
                      .ToArray());

                string rtfPath = Directory.GetCurrentDirectory()+"\\Movies\\Description\\"+ invalidCharsRemoved+".rtf";

                using (fStream = new FileStream(rtfPath, FileMode.Create))
                {
                    range.Save(fStream, System.Windows.DataFormats.Rtf);
                    fStream.Close();
                }

                DateTime dateTime = DateTime.Now;

                Movie movie = new Movie(rating, this.TitleTextBox.Text, imagePath, rtfPath, dateTime.ToString("yyyy-MM-dd"));

                Movies.Add(movie);

                booleanWrapper.Value = true;

                this.Close();
            }
            else
            {
                movie.Title = this.TitleTextBox.Text.Trim();
                movie.Rating = rating;
                movie.ImagePath = imagePath;

                using (fStream = new FileStream(movie.DescriptionPath, FileMode.OpenOrCreate))
                {
                    range.Save(fStream, System.Windows.DataFormats.Rtf);
                    fStream.Close();
                }

                movie.IsOpened = false;

                booleanWrapper.Value = true;

                this.Close();
            }
        }
    }
}

