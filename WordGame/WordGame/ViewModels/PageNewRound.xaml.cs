using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class PageNewRound : Page, INotifyPropertyChanged
    {
        // Fields are set to private by design they are for internal class use only.

        // Music variables
        private bool _stop_music = false;
        private readonly string c_Directory;
        private const string c_Song1 = @"Assets\ForThePoor.wav", c_Song2 = @"Assets\Jeopardy-theme-song.wav", c_Song3 = @"Assets\Beam.wav", c_Song4 = "stop";
        private const string c_Start = "START", c_Guesses = "Guesses: ", c_Hint = "Click for Hint";
        private string _song;
        private double _volume;

        // This probably should be seperated out into a seperate class because we are using
        // INotifyPropertyChanged which should not be directly on the UI code itself.
        private Visibility _volume_slider_visibility;
        public Visibility VolumeSliderVisibility 
        {
            get => _volume_slider_visibility;
            private set { _volume_slider_visibility = value; OnPropertyChanged(); }
        }
        public double Volume
        {
            get => _volume;
            set { _volume = value; OnPropertyChanged(); }
        }

        public string Song
        {
            get => _song;
            private set { _song = value; OnPropertyChanged(); }
        }


        public event HintRequestDelegate HintRequestMade;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // If we have any listeners to the PropertyChanged event then call the
            // PropertyChanged event handler for that event.
            // This utilizes reflection to update the UI with our logic properties.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PageNewRound()
        {
            // Setup for WPF component controls always make sure this is called first.
            InitializeComponent();

            Volume = 10.0;
            Song = c_Song2;
            SongPlayer.Stop();

            c_Directory = AppDomain.CurrentDomain.BaseDirectory;
            Debug.WriteLine("AppDomain: " + c_Directory);
        }

        private void MusicComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var item = sender as ComboBoxItem;

            if (item == song_one) { Song = c_Song1; SongPlayer.Play(); }
            else if (item == song_two) { Song = c_Song2; SongPlayer.Play(); }
            else if (item == song_three) { Song = c_Song3; SongPlayer.Play(); }
            else if (item == no_sound) { Song = c_Song4; SongPlayer.Stop(); }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            var data_context = DataContext as GameLogic;

            var buttons = from elements in MainGrid.Children.OfType<Button>().ToList() select elements;
            var letter_buttons = from elements in buttons where elements.Name.Contains("LB") select elements;
            for (int idx = 0; idx < letter_buttons.Count(); idx++)
            {
                //var lb = letter_buttons.ElementAt(idx);
                letter_buttons.ElementAt(idx).SetBinding(ContentProperty, $"LetterStates[{idx}].Letter");
                Trace.WriteLine($"Button Content: {letter_buttons.ElementAt(idx).Content}, Index: {idx}");

                var dt1 = new DataTrigger() { Value = "True", Binding = new Binding($"LetterStates[{idx}].HasZeroCount") };
                dt1.Setters.Add(new Setter{ Property = Border.BorderBrushProperty, Value = Brushes.LightBlue });
                var dt2 = new DataTrigger() { Value = "False", Binding = new Binding($"LetterStates[{idx}].HasZeroCount") };
                dt2.Setters.Add(new Setter { Property = Border.BorderBrushProperty, Value = Brushes.Red });
                var dt3 = new DataTrigger() { Value = 0, Binding = new Binding($"LetterStates[{idx}].State") };
                dt3.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = Brushes.LightBlue });
                var dt4 = new DataTrigger() { Value = 1, Binding = new Binding($"LetterStates[{idx}].State") };
                dt4.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = Brushes.Gray });
                var dt5 = new DataTrigger() { Value = 2, Binding = new Binding($"LetterStates[{idx}].State") };
                dt5.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = Brushes.Yellow });
                var dt6 = new DataTrigger() { Value = 3, Binding = new Binding($"LetterStates[{idx}].State") };
                dt6.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = Brushes.Green });

                Style style = new Style() { BasedOn = letter_buttons.ElementAt(idx).Style, TargetType = typeof(Button) };    
                style.Triggers.Add(dt1);
                style.Triggers.Add(dt2);
                style.Triggers.Add(dt3);
                style.Triggers.Add(dt4);
                style.Triggers.Add(dt5);
                style.Triggers.Add(dt6);

                letter_buttons.ElementAt(idx).Style = style;

            }

            var guess_buttons = from elements in buttons where elements.Name.Contains("GB") select elements;
            for (int idx = 0; idx < guess_buttons.Count(); idx++)
                guess_buttons.ElementAt(idx).SetBinding(ContentProperty, $"GuessedLetterStates[{idx}].Letter");

            var previous_guess_buttons = from elements in MainGrid.Children.OfType<TextBox>().ToList() where elements.Name.Contains("PG") select elements;
            for (int idx = 0; idx < previous_guess_buttons.Count(); idx++)
            {
                previous_guess_buttons.ElementAt(idx).SetBinding(TextBox.TextProperty, $"PreviouslyGuessedLetterStates[{idx}].Letter");
            }
        }

        private void StartRoundButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            SongPlayer.Play();
            btn.Command.Execute(btn.CommandParameter);
        }

        private void VolumeSliderUncollapse_MouseEnter(object sender, MouseEventArgs e)
        {
            VolumeSliderVisibility = Visibility.Visible;
        }
        private void VolumeSliderCollapse_MouseLeave(object sender, MouseEventArgs e)
        {
            VolumeSliderVisibility = Visibility.Collapsed;
        }
    }
}