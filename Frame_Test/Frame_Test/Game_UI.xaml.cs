using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net;
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
    public partial class Game_UI : Page
    {
        // UI Controls
        private Button _first_letter;
        private List<Button> _is_in;
        private List<Button> _guessed;
        private List<Button> _winning_guess;
        private List<Button> _guessed_word_buttons;
        private Brush _my_brush;

        // Reference to the Game_Logic object.
        private Game_Logic _logic;

        // Music variables
        private bool _stop = false;
        private const string _song1 = "ForThePoor.wav", _song2 = "Jeopardy-theme-song.wav", _song3 = "Beam.wav";

        public Game_UI(Game_Logic logic)
        {
            // Setup for WPF component controls always make sure this is called first.
            InitializeComponent();

            _is_in = new List<Button>();
            _guessed = new List<Button>();
            _winning_guess = new List<Button>();
            _guessed_word_buttons = new List<Button>();
            _my_brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x8E, 0xEC, 0xF5));

            // Initilize the game logic reference
            _logic = logic;

            // Initialize UI elements here
            ResetGame(_logic.NewRound);
        }

        public void ResetGame(bool newRound)
        {
            _logic.RoundStart = false;
            StartRoundBox.Text = "START";
            GuessesBox.Text = "Guesses: ";
            HintBox.Text = "Click For Hint";
            CurrentGuessBox.Clear();

            foreach (Button button in MainGrid.Children.OfType<Button>())
            {
                button.Background = _my_brush;
            }

            if (newRound)
            {
                Load_Letters();
                _logic.WinningWord = Game_Logic.ChooseWord();

                if (_stop)
                {
                    no_sound.IsChecked = true;
                }
                else
                {
                    song_one.IsChecked = true;
                }

                _logic.NewRound = false;
            }
        }

        public void Update()
        {
            if (_logic.RoundStart) { StartRoundBox.Text = _logic.FetchRoundTime(); }
        }

        // Randomly assigns letters to the buttons(no repeats) and enables the buttons.
        private void Load_Letters()
        {
            List<string> letters = new List<string>()
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y"
            };

            Random random = new Random();

            foreach (Button button in MainGrid.Children.OfType<Button>())
            {
                button.IsEnabled = true;
                int index = random.Next(letters.Count);
                string nextLetter = letters[index];
                button.Content = nextLetter;
                letters.RemoveAt(index);
            }
        }
        

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            // Sentinel "stop" remembers player's choice of no music between game rounds.
            if (radioButton == song_one) { _stop = false; Sound(_song1); }
            else if (radioButton == song_two) { _stop = false; Sound(_song2); }
            else if (radioButton == song_three) { _stop = false; Sound(_song3); }
            else if (radioButton == no_sound) { _stop = true; Sound("stop"); }
        }

        // Plays the music for the game.
        private void Sound(string song)
        {
            SoundPlayer player = new SoundPlayer();
            if (song != "stop")
            {
                player.SoundLocation = song;
                player.Load();
                player.Play();
            }
            else
            {
                player.Stop();
            }
        }

        private void ChangeColor()
        {
            if (_first_letter != null) { _first_letter.Background = Brushes.Red; }
            foreach (Button button in _is_in) { button.Background = Brushes.Green; }
            foreach (Button button in _guessed) { button.Background = Brushes.LightGray; }
        }

        private void Sort()
        {
            foreach (Button button in _guessed_word_buttons)
            {
                if (_logic.WinningWord[0].ToString() == button.Content.ToString())
                {
                    _first_letter = button;
                    _winning_guess.Add(button);
                }
                else if (_logic.WinningWord.Contains(button.Content.ToString()))
                {
                    _is_in.Add(button);
                    _winning_guess.Add(button);
                }
                else
                {
                    _guessed.Add(button);
                }
            }
        }

        private void Clear_guesses()
        {
            _guessed_word_buttons.Clear();
            CurrentGuessBox.Text = "";
        }

        // Adds button to guessed_word_buttons and displays the buttons content the the textbox below the buttons.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Background = Brushes.Blue;
            _guessed_word_buttons.Add(button);
            CurrentGuessBox.Text += button.Content;
        }

        private void Guess_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // We can't accept guesses that are not 5 letters long.
            if (CurrentGuessBox.Text.Length != 5)
            {
                return;
            }
            else 
            {
                if (Game_Logic.Approved(CurrentGuessBox.Text) && CurrentGuessBox.Text == _logic.WinningWord)
                {
                    _logic.Wins++;
                    _logic.Tries++;
                    _logic.UpdateTotalTime();
                    _logic.GameWon(true);
                }
                else if (Game_Logic.Approved(CurrentGuessBox.Text))
                {
                    _logic.Tries++;
                    GuessesBox.Text += "\n" + CurrentGuessBox.Text;
                    Sort();
                    ChangeColor();
                    Clear_guesses();
                }
                else
                {
                    foreach (Button button in _guessed_word_buttons)
                    {
                        if (!(_first_letter == button || _is_in.Contains(button) || _guessed.Contains(button)))
                        {
                            button.Background = _my_brush;
                        }
                    }
                    Clear_guesses();
                    ChangeColor();
                }
            }

        }
        
        private void HintBox_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            WebClient client = new WebClient();
            string strPageCode = client.DownloadString("https://api.dictionaryapi.dev/api/v2/entries/en/" + _logic.WinningWord);

            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPageCode);

            string definition = dobj[0]["meanings"][0]["definitions"][0]["definition"].ToString();

            HintBox.Text = definition;
        }

        private void Backspace_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_guessed_word_buttons.Count == 0)
            { 
                return; 
            }
            else
            {
                if (_first_letter == _guessed_word_buttons[^1])
                {
                    _guessed_word_buttons[^1].Background = Brushes.Red;
                }
                else if (_is_in.Contains(_guessed_word_buttons[^1]))
                {
                    _guessed_word_buttons[^1].Background = Brushes.Green;
                }
                else if (_guessed.Contains(_guessed_word_buttons[^1]))
                {
                    _guessed_word_buttons[^1].Background = Brushes.LightGray;
                }
                else
                {
                    _guessed_word_buttons[^1].Background = _my_brush;
                }

                _guessed_word_buttons.RemoveAt(_guessed_word_buttons.Count - 1);
                CurrentGuessBox.Text = CurrentGuessBox.Text.Remove(CurrentGuessBox.Text.Length - 1);
            }
        }

        private void StartRoundBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (StartRoundBox.Text == "START")
            {
                _logic.Start_Round_Timer();
                _logic.RoundStart = true;
            }
            else
            {
                return;
            }
        }

        private void ResetUnfinishedGame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetGame(false);
        }
    }
}