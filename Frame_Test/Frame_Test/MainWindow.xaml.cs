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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Data;
using System.Diagnostics;


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

            //SetBinding(WidthProperty, new Binding("ScreenWidth") { Source = this, Mode=BindingMode.TwoWay});
            //SetBinding(HeightProperty, new Binding("ScreenHeight") { Source = this, Mode = BindingMode.TwoWay });

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

        //private void Frame_Navigated(object sender, NavigationEventArgs e)
        //{
        //    ((FrameworkElement)e.Content).DataContext = this.DataContext;
        //}

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

    }
}
