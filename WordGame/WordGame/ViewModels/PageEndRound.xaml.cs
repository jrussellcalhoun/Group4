using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WordGame
{
    /// <summary>
    /// Interaction logic for PageEndRound.xaml
    /// </summary>
    public partial class PageEndRound : Page
    {
        private const string YOU_LOSE = "YOU LOSE :P", YOU_WON = "YOU WON :D";

        public PageEndRound()
        {
            InitializeComponent();
        }

        private void new_round_button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("ViewModels/PageNewRound.xaml", UriKind.Relative));
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Not Yet Implemented!");
        }
    }
}
