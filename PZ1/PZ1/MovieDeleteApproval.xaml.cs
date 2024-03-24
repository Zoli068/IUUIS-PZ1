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
    public partial class MovieDeleteApproval : Window
    {
        private BooleanWrapper confirmation;

        public MovieDeleteApproval(BooleanWrapper booleanWrapper)
        {
            confirmation = booleanWrapper;
            InitializeComponent();
        }

        private void CancelDelete_Click(object sender, RoutedEventArgs e)
        {
            confirmation.Value = false;
            this.Close();
        }

        private void ApproveDelete_Click(object sender, RoutedEventArgs e)
        {
            confirmation.Value = true;
            this.Close();
        }
    }
}
