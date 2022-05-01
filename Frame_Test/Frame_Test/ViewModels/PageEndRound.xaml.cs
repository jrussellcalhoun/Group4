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

namespace WordGame
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class PageEndRound : Page
    {
        private const string YOU_LOSE = "YOU LOSE :P", YOU_WON = "YOU WON :D";

        public PageEndRound()
        {
            InitializeComponent();

            win_or_lose_box.Text = "";
            game_info.Text = "";
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
