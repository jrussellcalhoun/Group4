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
        public PageEndRound()
        {
            InitializeComponent();
        }

        //private void NewRoundButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var btn = sender as Button;
        //    btn.Command.Execute(btn.CommandParameter);
        //}

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Not Yet Implemented!");
        }
    }
}
