using PZ1.Model;
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

namespace PZ1
{
    /// <summary>
    /// Interaction logic for MovieDeleteApproval.xaml
    /// </summary>
    public partial class ApprovalWindow : Window
    {
        private BooleanWrapper confirmation;

        public ApprovalWindow(BooleanWrapper booleanWrapper,string aproveText)
        {

            confirmation = booleanWrapper;

            InitializeComponent();
            this.AproveTextBlock.Text = aproveText;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            confirmation.Value = false;
            this.Close();
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            confirmation.Value = true;
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
