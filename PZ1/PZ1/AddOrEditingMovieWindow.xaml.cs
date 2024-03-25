﻿using System;
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

namespace PZ1
{
    /// <summary>
    /// Interaction logic for AddOrEditingWindow.xaml
    /// </summary>
    public partial class AddOrEditingMovieWindow : System.Windows.Window,INotifyPropertyChanged
    {
        private Uri imageSource = null;
        private DispatcherTimer dispatcherTimer;
        public event PropertyChangedEventHandler PropertyChanged;

        //Values for the new movie
        private string imagePath;
        private int fontSize = 12;

        private Brush fontColor;

        public AddOrEditingMovieWindow()
        {
            InitializeComponent();
            DataContext = this;

            StartUp();
        }

        private void StartUp()
        {

            FontFamilyComboBox.ItemsSource = System.Windows.Media.Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontFamilyComboBox.SelectedIndex = 2;

            var values = typeof(Brushes).GetProperties().Select(p =>  p.Name).ToArray();
            FontColorComboBox.ItemsSource = values;

            FontColorComboBox.Text = "White";
            FontSizeTextBox.Text = fontSize.ToString();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 6);

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
            this.InvalidExtensionErrorLabel.Foreground = Brushes.Black;

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
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);

                string extension = System.IO.Path.GetExtension(files[0]);

               if(extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".gif") ||extension.Equals(".jpeg"))
                {
                    imagePath = files[0];
                    ImageSource = new Uri("pack://"+files[0],UriKind.Absolute);
                    this.PreviewImage.Visibility= Visibility.Visible;
                    this.PreviewImageDropArea.Visibility= Visibility.Collapsed;
                    this.InvalidExtensionErrorLabel.Foreground = Brushes.Black;
                }
                else
                {
                    this.InvalidExtensionErrorLabel.Foreground = Brushes.Red;
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

            bool? result=dialog.ShowDialog();

            if (result == true)
            {
                imagePath = dialog.FileName;
                ImageSource = new Uri("pack://" + dialog.FileName, UriKind.Absolute);
                this.PreviewImage.Visibility = Visibility.Visible;
                this.PreviewImageDropArea.Visibility = Visibility.Collapsed;
                this.InvalidExtensionErrorLabel.Foreground = Brushes.Black;
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

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            this.RichTextBox.FontFamily= FontFamilyComboBox.SelectedValue as FontFamily;
        }

        private void FontColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

                FontColor = (Brush)new BrushConverter().ConvertFromString(FontColorComboBox.SelectedValue.ToString());

                this.RichTextBox.Foreground = FontColor;

        }

        private void FontSizeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            int value;

           if(int.TryParse(FontSizeTextBox.Text, out value))
            {
            
                if(value>0 && value < 1200)
                {
                fontSize = value;
                RichTextBox.FontSize = fontSize;

                }
                else
                {
                    FontSizeTextBox.Text = fontSize.ToString();
                }
            
            }
            else
            {
                FontSizeTextBox.Text = fontSize.ToString();
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

        private void FontSizeTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this.FontSizeTextBox), null);
        }
    }
}
