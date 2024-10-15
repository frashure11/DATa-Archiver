using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DATa_Archiver
{
        /// <summary>
        /// Interaction logic for App.xaml
        /// </summary>
        public partial class App : Application
        {
                // Add a public static bool variable for DebugFlag
                public static bool DebugFlag = false;
                // Override the OnStartup method to handle command-line arguments
                protected override void OnStartup(StartupEventArgs e)
                {
                        base.OnStartup(e);
                        string[] args = Environment.GetCommandLineArgs();
                        foreach (string arg in args)
                        {
                                if (arg.Equals("-d", StringComparison.OrdinalIgnoreCase))
                                {
                                        DebugFlag = true;
                                        break;
                                }
                        }
                        //if (DebugFlag)
                        //{
                        //    MessageBox.Show("Debug Mode Enabled", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
                        //}
                }
        }
}
