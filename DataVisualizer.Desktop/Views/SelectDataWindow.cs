using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataVisualizer.Desktop.Views
{
    public abstract class SelectDataWindow : Window
    {
        public SelectDataWindow()
        {

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
