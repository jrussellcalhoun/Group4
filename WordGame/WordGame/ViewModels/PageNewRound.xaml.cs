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
using WordGame.Objects;

namespace WordGame
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class PageNewRound : Page, INotifyPropertyChanged
    {
        // Fields are set to private by design they are for internal class use only.

        // Music variables
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // If we have any listeners to the PropertyChanged event then call the
            // PropertyChanged event handler for that event.
            // This utilizes reflection to update the UI with our logic properties.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// New round constructor
        /// </summary>
        public PageNewRound()
        {
            // Setup for WPF component controls always make sure this is called first.
            InitializeComponent();

            Volume = 10.0;
            VolumeSliderVisibility = Visibility.Collapsed;
            Song = c_Song2;
            SongPlayer.Stop();

            c_Directory = AppDomain.CurrentDomain.BaseDirectory;
            Debug.WriteLine("AppDomain: " + c_Directory);
        }

        /// <summary>
        /// This is an event handler for when the music selection has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MusicComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var combo_box = sender as ComboBox;
            Trace.WriteLine($"MusicComboBox Item selection changes. {combo_box.SelectedValue} is now the current item selection.");

            if      (combo_box.SelectedItem == song_one)   { Song = c_Song1; SongPlayer.Stop(); SongPlayer.Play(); }
            else if (combo_box.SelectedItem == song_two)   { Song = c_Song2; SongPlayer.Stop(); SongPlayer.Play(); }
            else if (combo_box.SelectedItem == song_three) { Song = c_Song3; SongPlayer.Stop(); SongPlayer.Play(); }
            else if (combo_box.SelectedItem == no_sound)   { Song = c_Song4; SongPlayer.Stop(); }
        }

        private void SongPlayer_Ended(object sender, EventArgs e)
        {
            if (Song != c_Song4)
            { 
                var player = sender as MediaElement;
                player.Position = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// This method is a factory pattern implementation that constructs styles with data triggers based on input parameters.
        /// 
        /// </summary>
        /// <param name="idx_of_control"></param>
        /// <param name="based_on_style"></param>
        /// <param name="target_type"></param>
        /// <param name="bindings"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private Style StateCollectionStyleFactory(int idx_of_control, Style based_on_style, Type target_type, Dictionary<string, Dictionary<object, Assignment>> bindings)
        {
            var style = new Style() { BasedOn = based_on_style, TargetType = target_type };

            // "CollectionName|Property"
            foreach (var b in bindings)
            {
                string key = b.Key;
                int sub_index = key.IndexOf('|');
                if (sub_index == -1) throw new ArgumentException("Invalid string formation of key in StyleFactoryDictionary");

                string collection_name = key.Substring(0, sub_index);
                var sub_length = key.Length - sub_index;
                string property_name = key.Substring(sub_index + 1, sub_length - 1);

                foreach (var assignment in b.Value)
                {
                    var dt = new DataTrigger() { Value = assignment.Key, Binding = new Binding($"{collection_name}[{idx_of_control}].{property_name}") };
                    dt.Setters.Add(new Setter { Property = assignment.Value.Dependency, Value = assignment.Value.Value });
                    style.Triggers.Add(dt);
                }
            }

            return style;
        }

        /// <summary>
        /// This is an overloaded event handler for the OnPageLoaded event.
        /// We are overloading this so that we can iteratively bind our letter states from our observable colletions onto our individual buttons.
        /// This method utilizes a factory pattern style implementation in order to create our bindings with custom data triggers iteratively.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            // Cache the data context for processing.
            var data_context = DataContext as GameLogic;

            // This code utilizes Linq query syntax in order to make specific selections from the children of our MainGrid control.
            // We only want to select buttons here, we place them into a temporary list in order to iterate over them.
            var buttons = from elements in MainGrid.Children.OfType<Button>().ToList() select elements;

            // Here we select all buttons from our temporary buttons list but we select only the elements whose name contains the prefixed
            // characters "LB". This corresponds to the names of our letter buttons.
            var letter_buttons = from elements in buttons where elements.Name.Contains("LB") select elements;
            for (int idx = 0; idx < letter_buttons.Count(); idx++)
            {
                letter_buttons.ElementAt(idx).SetBinding(ContentProperty, $"LetterStates[{idx}].Letter");
                Trace.WriteLine($"Button Content: {letter_buttons.ElementAt(idx).Content}, Index: {idx}");

                var assignments_has_zero_count = new Dictionary<object, Assignment> 
                { 
                    { "True", new Assignment(Border.BorderBrushProperty, Brushes.RoyalBlue) },
                    { "False", new Assignment(Border.BorderBrushProperty, Brushes.Red) }
                };

                var assignments_state = new Dictionary<object, Assignment>
                {
                    { 0, new Assignment(Button.BackgroundProperty, Brushes.LightBlue) },
                    { 1, new Assignment(Button.BackgroundProperty, Brushes.Gray) },
                    { 2, new Assignment(Button.BackgroundProperty, Brushes.Yellow) },
                    { 3, new Assignment(Button.BackgroundProperty, Brushes.Green) }
                };

                letter_buttons.ElementAt(idx).Style = StateCollectionStyleFactory(idx, letter_buttons.ElementAt(idx).Style, typeof(Button),
                    new Dictionary<string, Dictionary<object, Assignment>>{ 
                        { "LetterStates|HasZeroCount", assignments_has_zero_count },
                        { "LetterStates|State", assignments_state} });
            }

            // Here we select all buttons from our temporary buttons list but we select only the elements whose name contains the prefixed
            // characters "GB". This corresponds to the names of our guess letter buttons.
            var guess_buttons = from elements in buttons where elements.Name.Contains("GB") select elements;
            for (int idx = 0; idx < guess_buttons.Count(); idx++)
                guess_buttons.ElementAt(idx).SetBinding(ContentProperty, $"GuessedLetterStates[{idx}].Letter");

            // Here we select all buttons from our temporary buttons list but we select only the elements whose name contains the prefixed
            // characters "PG". This corresponds to the names of our previously guessed letter buttons.
            var previous_guess_buttons = from elements in MainGrid.Children.OfType<TextBox>().ToList() where elements.Name.Contains("PG") select elements;
            for (int idx = 0; idx < previous_guess_buttons.Count(); idx++)
            {
                previous_guess_buttons.ElementAt(idx).SetBinding(TextBox.TextProperty, $"PreviouslyGuessedLetterStates[{idx}].Letter");

                var assignments_state = new Dictionary<object, Assignment>
                {
                    { 0, new Assignment(Button.BackgroundProperty, Brushes.LightBlue) },
                    { 1, new Assignment(Button.BackgroundProperty, Brushes.Gray) },
                    { 2, new Assignment(Button.BackgroundProperty, Brushes.Yellow) },
                    { 3, new Assignment(Button.BackgroundProperty, Brushes.Green) }
                };

                previous_guess_buttons.ElementAt(idx).Style = StateCollectionStyleFactory(idx, previous_guess_buttons.ElementAt(idx).Style, typeof(TextBox),
                new Dictionary<string, Dictionary<object, Assignment>>{
                    { "PreviouslyGuessedLetterStates|State", assignments_state} });
            }
        }

        /// <summary>
        /// Event handler for when the start round button has been clicked. It passes to the attached command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartRoundButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            SongPlayer.Play();
            btn.Command.Execute(btn.CommandParameter);
        }

        /// <summary>
        /// The following delegate event handler turns the volume slider visible when the mouse enters the bound border box surrounding it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeSliderUncollapse_MouseEnter(object sender, MouseEventArgs e)
        {
            VolumeSliderVisibility = Visibility.Visible;
        }

        /// <summary>
        /// The following delegate event handler turns the volume slider invisible when the mouse leaves the bound border box surrounding it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeSliderCollapse_MouseLeave(object sender, MouseEventArgs e)
        {
            VolumeSliderVisibility = Visibility.Collapsed;
        }
    }
}