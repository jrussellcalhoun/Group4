using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

using Word_Game.Utilities;

namespace Word_Game
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class WordGameApp : Application
    {
        public Game_Logic Logic { get; private set; }
        public MainWindow WordGameMainWindow { get; private set; }
        //public static double SystemWidthDPIAware { get; private set; }
        //public static double SystemHeightDPIAware { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConsoleHelper.CreateDebugConsole();

            Logic = new Game_Logic();
            WordGameMainWindow = new MainWindow { DataContext = Logic };
            Logic.ChangePageEvent += WordGameMainWindow.ChangePage;
            WordGameMainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                ConsoleHelper.ShutdownDebugConsole();
                //Shutdown(0);
            }
            finally
            {
                base.OnExit(e);
            }
        }

        private void CacheScreenSizeDPI()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            var dpiY = (int)dpiYProperty.GetValue(null, null);

            //SystemWidthDPIAware = SystemParameters.PrimaryScreenWidth * dpiX / 96.0;
            //SystemHeightDPIAware = SystemParameters.PrimaryScreenHeight * dpiY / 96.0;
            Trace.Write("DpiX: " + dpiX + " DpiY: " + dpiY + "\n");
        }
    }
}
