using System.Diagnostics;
using System.Reflection;
using System.Windows;

using WordGame.Utilities;

namespace WordGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class WordGameApp : Application
    {
        // The Logic Model and MainWindow ViewModel are owned instances by the Application
        public GameLogic Logic { get; private set; }
        public MainWindow WordGameMainWindow { get; private set; }

        // Overriden OnStartup method we instantiate the instances of our Window view model and Logic.
        // Then we subscribe the WordGameMainWindow instance to the ChangePageEvent of the logic class.
        // We also utilize our ConsoleHelper class here if we are in debug mode. Otherwise we don't call
        // the CreateDebugConsole() method.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConsoleHelper.CreateDebugConsole();

            Logic = new GameLogic();
            WordGameMainWindow = new MainWindow { DataContext = Logic };
            Logic.ChangePageEvent += WordGameMainWindow.ChangePage;
            WordGameMainWindow.Show();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                ConsoleHelper.ShutdownDebugConsole();
            }
            finally
            {
                base.OnExit(e);
            }
        }

    }
}
