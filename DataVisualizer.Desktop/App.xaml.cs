using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using CommonServiceLocator;
using DataVisualizer.Persistence;
using Prism.Ioc;
using Prism.Unity;

namespace DataVisualizer.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        // A workaround for repeating 'random' values
        public static Random RANDOM = new Random();

    }
}
