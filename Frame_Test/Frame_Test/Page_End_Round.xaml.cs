using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page_End_Round : Page
    {
        private const string YOU_LOSE = "YOU LOSE :P", YOU_WON = "YOU WON :D";

        public Page_End_Round()
        {
            InitializeComponent();

            win_or_lose_box.Text = "";
            game_info.Text = "";
        }

        public void Update()
        {
            //if (_logic.Won)
            //{
            //    win_or_lose_box.Text = YOU_WON;
            //
            //}
            //else
            //{
            //    win_or_lose_box.Text = YOU_LOSE;
            //}

            //game_info.Text = $"Winning Word: {_logic.WinningWord}\n" +
            //                 $"Time Remaining: {_logic.TotalTime}\n" +
            //                 $"Guesses Used: {_logic.Tries}\n"       +
            //                 $"Total Wins: {_logic.Wins}";
        }

        private void new_round_button_Click(object sender, RoutedEventArgs e)
        {
            //var window = Application.Current.MainWindow as MainWindow;
            //window.ChangePage(MainWindow.Pages.GAME_UI);
            //_logic.NewRound = true;
            //_logic.Won = false;
            //_logic.GameOver = false;
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            //_logic.Quit = true;
        }

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {

            //var window = Application.Current.MainWindow as MainWindow;
            //window.ChangePage(MainWindow.Pages.SETTINGS);
        }
    }
}
