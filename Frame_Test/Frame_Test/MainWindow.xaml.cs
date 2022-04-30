using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;

using Word_Game.Utilities;

namespace Word_Game
{
    public enum PageState
    {
        MAIN_MENU, PAGE_NEW_ROUND, PAGE_PLAY_AGAIN, PAGE_SETTINGS, PAGE_LOGIN
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Frame Pages
        //private Settings    _settings;
        //private Login       _login;
        private Page_New_Round  _page_new_round;
        private Page_End_Round  _page_end_round;

        private PageState _current_page;

        public MainWindow()
        {
            // Initialization of internal WPF controls. Always make sure this is called first.
            InitializeComponent();

            // Initialize references to lifetime game objects.
            _current_page = PageState.MAIN_MENU;
            _page_new_round = new Page_New_Round();
            _page_end_round = new Page_End_Round();
            Trace.Write("System Width: " + SystemParameters.PrimaryScreenWidth + "\n" + "System Height: " + SystemParameters.PrimaryScreenHeight + "\n");
        }

        public void ChangePage(object sender, EventArgs e)
        {
            var args = e as ChangePageEventArgs;
            switch (args.CurrentPage)
            {
                case PageState.PAGE_LOGIN:
                    //Frame.Content = _login;
                    break;
                case PageState.PAGE_SETTINGS:
                    //Frame.Content = _settings;
                    break;
                case PageState.PAGE_NEW_ROUND:
                    Frame.Content = _page_new_round;
                    _current_page = PageState.PAGE_NEW_ROUND;
                    break;
                case PageState.PAGE_PLAY_AGAIN:
                    Frame.Content = _page_end_round;
                    _current_page = PageState.PAGE_PLAY_AGAIN;
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            title_box.Visibility = Visibility.Hidden;
            instruction_box.Visibility = Visibility.Hidden;
            newGame_button.Visibility = Visibility.Hidden;
            Frame.Content = _page_new_round;
            _current_page = PageState.PAGE_NEW_ROUND;
        }

        // The following are navigation events for the Frame element (and it's pages).
        // Here we are manually setting the DataContext of the frames to be our Game_Logic instance (so that it's shared).
        private void Frame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender, null);
        }
        private void Frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender, e);
        }
        private void UpdateFrameDataContext(object sender, NavigationEventArgs e)
        {
            var content = Frame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = Frame.DataContext;
        }

        // Haven't decided if I want to keep this yet, uncomment to remove the border and system menu on the window.
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Trace.WriteLine("Window Loaded");
        //    IntPtr window_handle = new WindowInteropHelper(this).Handle;
        //    int current_style = InteropHelper.GetWindowLongPtr(window_handle, GWL.GWL_STYLE).ToInt32();
        //    InteropHelper.SetWindowLongPtr(window_handle, GWL.GWL_STYLE, new IntPtr(current_style & (~(WS_STYLE.WS_BORDER | WS_STYLE.WS_SYSMENU) | WS_STYLE.WS_POPUP)));
        //}
    }
}
