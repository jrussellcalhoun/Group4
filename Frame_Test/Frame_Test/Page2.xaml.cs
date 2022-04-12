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

namespace Frame_Test
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        int total_wins;
        public Page2(bool won, string total_time, int total_tries, string winning_word, int wins)
        {
            InitializeComponent();
            if (won)
            {
                win_or_lose_box.Text = "YOU WON :D";
                total_wins += wins;
            }
            else
            {
                win_or_lose_box.Text = "YOU LOSE :P";
            }
            game_info.Text = $"winning word: {winning_word}\n" +
                    $"Time remaining:\t{total_time}\n" +
                    $"Guessed used:\t{total_tries}\n" +
                    $"Total wins:\t{total_wins}\n";
        }

        private void new_game_button_Click(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Application.Current.MainWindow;
            window.Frame.Content = new Page1(total_wins);
        }
    }
}
