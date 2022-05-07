using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

using WordGame.Utilities;

namespace WordGame
{
    /// <summary>
    /// An enum to store the various page states of our application
    /// Unfortunately we are out of time to implement the other two planned pages.
    /// </summary>
    public enum PageState : uint
    {
        MAIN_MENU = 0,
        PAGE_NEW_ROUND = 1,
        PAGE_END_ROUND = 2, 
        PAGE_SETTINGS = 3,
        PAGE_LOGIN = 4
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Serializable]
    public partial class MainWindow : Window
    {
        // Frame Pages
        //private Settings    _settings;
        //private Login       _login;
        private PageNewRound  _page_new_round;
        private PageEndRound  _page_end_round;

        public MainWindow()
        {
            // Initialization of internal WPF controls. Always make sure this is called first.
            InitializeComponent();

            // Initialize references to lifetime game objects.
            _page_new_round = new PageNewRound();
            _page_end_round = new PageEndRound();

            Trace.Write("System Width: " + SystemParameters.PrimaryScreenWidth + "\n" + "System Height: " + SystemParameters.PrimaryScreenHeight + "\n");
        }

        public void ChangePage(object sender, EventArgs e)
        {
            var args = e as ChangePageEventArgs;

            Trace.WriteLine($"Main Window-> ChangePage Method. Argument CurrentPage is {args.CurrentPage}");
            switch (args.CurrentPage)
            {
                case PageState.MAIN_MENU:
                    break;
                //case PageState.PAGE_LOGIN:
                    //Frame.Content = _login;
                    //break;
                //case PageState.PAGE_SETTINGS:
                    //Frame.Content = _settings;
                    //break;
                case PageState.PAGE_NEW_ROUND:
                    Trace.WriteLine("Main Window-> Navigating to New Round.");

                    //Current_Page = PageState.PAGE_NEW_ROUND;
                    this.GameFrame.Navigate(_page_new_round);
                    break;
                case PageState.PAGE_END_ROUND:
                    Trace.WriteLine("Main Window-> Navigating to End Round.");
                    //Current_Page = PageState.PAGE_END_ROUND;
                    this.GameFrame.Navigate(_page_end_round);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This is a Button Click event bound to our New Game button on the Main Window. We need to 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            title_box.Visibility = Visibility.Hidden;
            instruction_box.Visibility = Visibility.Hidden;
            newGame_button.Visibility = Visibility.Hidden;

            var btn = sender as Button;
            btn.Command.Execute(btn.CommandParameter);
        }

        // The following are navigation events for the Frame element (and it's pages).
        private void Frame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender, null);
        }
        private void Frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender, e);
        }

        // Here we are manually setting the DataContext of the frames to be our GameLogic instance (so that it's shared).
        // In WPF Frame child pages do not automatically inherit the DataContext of the parent control.
        private void UpdateFrameDataContext(object sender, NavigationEventArgs e)
        {
            var content = GameFrame.Content as FrameworkElement;
            if (content == null)
            {
                Trace.WriteLine("GameFrame.Content is null!");
                return;
            }
            else
            {
                Trace.WriteLine($"Setting Frame Content DataContext to {content.DataContext}");
                content.DataContext = GameFrame.DataContext;
            }
            
        }

        // This is commented out, but I am leaving it here to show what might be done to change the style of the window at run time.
        // Uncomment this to see the window without a border and title bar.
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Trace.WriteLine("Window Loaded");
        //    IntPtr window_handle = new WindowInteropHelper(this).Handle;
        //    int current_style = InteropHelper.GetWindowLongPtr(window_handle, GWL.GWL_STYLE).ToInt32();
        //    InteropHelper.SetWindowLongPtr(window_handle, GWL.GWL_STYLE, new IntPtr(current_style & (~(WS_STYLE.WS_BORDER | WS_STYLE.WS_SYSMENU) | WS_STYLE.WS_POPUP)));
        //}
    }
}
