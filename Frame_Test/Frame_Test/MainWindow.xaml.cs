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


namespace Word_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Objects relevant to the entire lifetime of the game should be declared here.
        private Game_Logic _logic;
        private DispatcherTimer _tick_timer;

        // Frame Pages
        //private Settings    _settings;
        //private Login       _login;
        private Game_UI     _ui;
        private Play_Again  _play_again;

        public enum Pages
        {
            MAIN_MENU, GAME_UI, PLAY_AGAIN, SETTINGS, LOGIN
        }

        private Pages _current_page;

        public MainWindow()
        {
            // Initialization of internal WPF controls. Always make sure this is called first.
            InitializeComponent();
            
            // Initialize references to lifetime game objects.
            _current_page = Pages.MAIN_MENU;
            _tick_timer = new DispatcherTimer();
            _logic = new Game_Logic();
            _ui = new Game_UI(_logic);
            _play_again = new Play_Again(_logic);

            Start();
        }

        public void ChangePage(Pages page)
        {
            switch (page)
            {
                case Pages.LOGIN:
                    //Frame.Content = _login;
                    break;
                case Pages.SETTINGS:
                    //Frame.Content = _settings;
                    break;
                case Pages.GAME_UI:
                    Frame.Content = _ui;
                    _current_page = Pages.GAME_UI;
                    break;
                case Pages.PLAY_AGAIN:
                    Frame.Content = _play_again;
                    _current_page = Pages.PLAY_AGAIN;
                    break;
                default:
                    break;
            }
        }

        // Runs once at the start of the game.
        public void Start()
        {
            _tick_timer.Tick += Run;
            _tick_timer.Interval = TimeSpan.FromMilliseconds(20);
            _tick_timer.Start();


        }

        // Runs every tick of the game (Every 20 milliseconds).
        public void Run(object sender, EventArgs e)
        {
            if(_logic.Exit)
            { 
                End(); 
            }
            else
            {
                switch (_current_page)
                {
                    case Pages.LOGIN:
                        break;
                    case Pages.SETTINGS:
                        break;
                    case Pages.GAME_UI:
                        if (_logic.Won || _logic.GameOver) 
                        { 
                            ChangePage(Pages.PLAY_AGAIN);
                        }
                        
                        _ui.Update();
                        break;
                    case Pages.PLAY_AGAIN:
                        if(_logic.NewRound) 
                        {
                            Console.WriteLine("New Round");
                            _ui.ResetGame(true); 
                            _logic.NewRound = false; 
                            ChangePage(Pages.GAME_UI); 
                        }
                        _play_again.Update();
                        break;
                    default:
                        break;
                }
            }
        }

        // Runs once at the end of the game.
        public void End()
        {
            _tick_timer.Stop();
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            title_box.Visibility = Visibility.Hidden;
            instruction_box.Visibility = Visibility.Hidden;
            newGame_button.Visibility = Visibility.Hidden;

            ChangePage(Pages.GAME_UI);
        }
    }
}
