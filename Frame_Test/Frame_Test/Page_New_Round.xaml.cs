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

namespace Word_Game
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page_New_Round : Page, INotifyPropertyChanged
    {
        // Fields are set to private by design they are for internal class use only.

        // UI Controls
        private Button _first_letter;
        private List<Button> _is_in;
        private List<Button> _guessed;
        private List<Button> _winning_guess;
        private List<Button> _guessed_word_buttons;
        private Brush _letter_default_brush;
        private Brush _letter_in_word_brush;
        private Brush _letter_correct_position_brush;

        // TODO: FIX ME
        // Need to change this code to use a MediaElement Control instead.
        // MediaPlayer is more for creating custom video players!
        private MediaPlayer _media_player;

        // This probably should be seperated out into a seperate class because we are using INotifyPropertyChanged which should not be directly on the UI code itself.
        private Visibility _volume_slider_visibility;
        public Visibility VolumeSliderVisibility 
        {
            get => _volume_slider_visibility;
            private set { _volume_slider_visibility = value; OnPropertyChanged(); }
        }

        // Music variables
        private bool _stop_music = false;
        private readonly string c_Directory;
        private const string c_Song1 = @"Assets\ForThePoor.wav", c_Song2 = @"Assets\Jeopardy-theme-song.wav", c_Song3 = @"Assets\Beam.wav", c_Song4 = "stop";
        private const string c_Start = "START", c_Guesses = "Guesses: ", c_Hint = "Click for Hint";

        public event HintRequestDelegate HintRequestMade;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // If we have any listeners to the PropertyChanged event then call the PropertyChanged event handler for that event.
            // This utilizes reflection to update the UI with our logic properties.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private bool _should_reset_brush;
        //
        //// Properties are public by design. These are used for data binding and external access by other classes.
        //public bool ShouldResetLetterButtonBackground
        //{
        //    get { return _should_reset_brush; }
        //    set 
        //    { 
        //        OnPropertyChanged(nameof(ShouldResetLetterButtonBackground));
        //        OnPropertyChanged(nameof(LetterButtonBackground));
        //    }
        //}
        //
        //public Brush LetterButtonBackground => ShouldResetLetterButtonBackground ? _letter_default_brush : Brushes.Pink;
        //
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName) =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Page_New_Round()
        {
            // Setup for WPF component controls always make sure this is called first.
            InitializeComponent();

            _is_in = new List<Button>();
            _guessed = new List<Button>();
            _winning_guess = new List<Button>();
            _guessed_word_buttons = new List<Button>();
            _letter_default_brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x8E, 0xEC, 0xF5));

            // TODO: FIX ME
            // Need to change this code to use a MediaElement Control instead.
            // MediaPlayer is more for creating custom video players!
            //_media_player = new MediaPlayer();

            c_Directory = AppDomain.CurrentDomain.BaseDirectory;
            Debug.WriteLine("AppDomain: " + c_Directory);

            ResetGame(true);
        }

        public void ResetGame(bool newRound)
        {
            ClearGuesses();

            foreach (Button button in MainGrid.Children.OfType<Button>())
            {
                button.Background = _letter_default_brush;
            }

            if (newRound)
            {
                if (_stop_music)
                {
                    no_sound.IsSelected = true;
                }
                else
                {
                    song_one.IsSelected = true;
                }
            }
        }

        private void MusicComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = sender as ComboBoxItem;

            // Sentinel (stop_music) remembers player's choice of no music between game rounds.
                 if (item == song_one)   { _stop_music = false; PlayMusic(c_Song1); }
            else if (item == song_two)   { _stop_music = false; PlayMusic(c_Song2); }
            else if (item == song_three) { _stop_music = false; PlayMusic(c_Song3); }
            else if (item == no_sound)   { _stop_music = true;  PlayMusic(c_Song4); }
        }

        // Plays the music for the game.
        private void PlayMusic(string song)
        {
            if (song != c_Song4)
            {
                // TODO: FIX ME
                // Need to change this code to use a MediaElement Control instead.
                // MediaPlayer is more for creating custom video players!

                //var tmp = AppDomain.CurrentDomain.BaseDirectory + song;
                //Debug.WriteLine(tmp);
                //var uri = new Uri(tmp);
                //_media_player.Open(uri);
                //_media_player.Play();
            }
            else
            {
                //_media_player.Stop();
            }
        }

        private void ChangeColor()
        {
            if (_first_letter != null) { _first_letter.Background = Brushes.Red; }
            foreach (Button button in _is_in) { button.Background = Brushes.Green; }
            foreach (Button button in _guessed) { button.Background = Brushes.LightGray; }
        }

        private void ClearGuesses()
        {
            _guessed_word_buttons.Clear();
            _is_in.Clear();
            _winning_guess.Clear();
        }

        // Adds button to guessed_word_buttons and displays the buttons content the the textbox below the buttons.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Background = Brushes.Blue;
            _guessed_word_buttons.Add(button);
            //CurrentGuessBox.Text += button.Content;
        }

        private void GuessButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void HintButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HintRequestMade(this, null);
        }

        //private void Backspace_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (_guessed_word_buttons.Count == 0)
        //    { 
        //        return; 
        //    }
        //    else
        //    {
        //        if (_first_letter == _guessed_word_buttons[^1])
        //        {
        //            _guessed_word_buttons[^1].Background = Brushes.Red;
        //        }
        //        else if (_is_in.Contains(_guessed_word_buttons[^1]))
        //        {
        //            _guessed_word_buttons[^1].Background = Brushes.Green;
        //        }
        //        else if (_guessed.Contains(_guessed_word_buttons[^1]))
        //        {
        //            _guessed_word_buttons[^1].Background = Brushes.LightGray;
        //        }
        //        else
        //        {
        //            _guessed_word_buttons[^1].Background = _letter_default_brush;
        //        }
        //
        //        _guessed_word_buttons.RemoveAt(_guessed_word_buttons.Count - 1);
        //        CurrentGuessBox.Text = CurrentGuessBox.Text.Remove(CurrentGuessBox.Text.Length - 1);
        //    }
        //}

        private void StartRoundButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (StartRoundBox.Text == c_Start)
            //{
            //    //_logic.StartRoundTimer();
            //    //_logic.RoundStart = true;
            //}
            //else
            //{
            //    return;
            //}
        }

        private void ResetGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetGame(false);
        }

        private void VolumeSliderUncollapse_MouseEnter(object sender, MouseEventArgs e)
        {
            VolumeSliderVisibility = Visibility.Visible;
        }

        private void VolumeSliderCollapse_MouseLeave(object sender, MouseEventArgs e)
        {
            VolumeSliderVisibility = Visibility.Collapsed;
        }

        //public void TimerSecondElapsed(object sender, EventArgs e)
        //{
        //    var args = e as TimerSecondElapsedEventArgs;
        //
        //    StartRoundBox.Text = args.TimeLeft.ToString();
        //}

        //public void Update()
        //{
        //    if (_logic.RoundStart) { StartRoundBox.Text = _logic.FetchRoundTime(); }
        //}

        // Randomly assigns letters to the buttons(no repeats) and enables the buttons.


    }
}