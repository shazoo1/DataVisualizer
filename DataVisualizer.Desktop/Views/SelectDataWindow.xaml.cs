using DataVisualizer.Desktop.ViewModel;
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

namespace DataVisualizer.Desktop.Views
{
    /// <summary>
    /// Interaction logic for SelectDataWindow.xaml
    /// </summary>
    public partial class SelectDataWindow : Window
    {
        public SelectDataWindow()
        {
            InitializeComponent();
        }

        public void OnOkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
