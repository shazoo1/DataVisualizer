using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DataVisualizer.Persistence;

namespace DataVisualizer.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // A workaround for repeating 'random' values
        public static Random RANDOM = new Random();
    }
}
