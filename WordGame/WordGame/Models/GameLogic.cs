using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Net.Http;
using System.Diagnostics;
using System.Text.Json;

using WordGame.Objects;
using WordGame.Utilities;

// Ctrl + M followed by Ctrl + S to collapse current region | Ctrl + M followed by Ctrl + E to expand current region.
#region Operators Used In This Program
/*  The following are intermediate to advanced concepts of C#7 and higher and .Net 6.0 and higher that are not covered in the C# class.
    The short explanation of these concepts below is for the benefit of other programmers who may not be familiar with these more advanced concepts.
    Links are provided in some instances to MSDN documentation. */

// Pseudo code in these descriptions are surrounded with double angle brackets <<pseudo-code>>

/*  Null Coallescing Operator
    
    Online Documentation: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
    
    Syntax: <<left-hand-operand>> ?? <<right-hand-operand>> 

    This operator returns the value of the left-hand-operand if it is not null, otherwise it returns the value of the right-hand-operand
    Operands must be a Nullable Type (see below) or a Reference value    
    This is a shorter way of coding something like: 

    return <<left-hand-operand>> != null ? <<left-hand-operand>> : new <<left-hand-operand-type-instance>>; 

    or:
    
    if(<<left-hand-operand>> != null) 
    { 
       return <<left-hand-operand>>;
    }
    else
    {
        return new <<left-hand-operand-type-instance>>; 
    }
*/

/*  Null Coallescing Assignment Operator
    
    Online Documentation: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
    
    Syntax: <<left-hand-operand>> ??= <<right-hand-operand>> 

    This operator returns the value of the left-hand-operand if it is not null, otherwise it returns the value of the right-hand-operand
    Operands must be a Nullable Type (see below) or a Reference value
    This is a shorter way of coding things like: 
    
    if(<<left-hand-operand>> == null) 
    { 
       <<left-hand-operand>> = new <<left-hand-operand-type-instance>>; 
    }
*/

/*  Nullable Types
 *  
    Online Documentation: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types
    
    Syntax: <<type>>? <<variable-name>>
*/

#endregion // End Operators Used In This Program


namespace WordGame
{
    /// <summary>
    /// This is our Model for our program it controls the actual game logic for our program. We have achieved a high degree of
    /// object-oriented decoupling from our UI code. With additional time the code in this class could be further seperated out
    /// into seperate models which deal with more specific single-purpose functionality.
    /// </summary>
    [Serializable]
    public class GameLogic : ObservableObject
    {
        #region Fields
        /// <summary>
        /// Holds the current main window frame page state. This indicates which page is currently focused by the UI.
        /// </summary>
        public PageState _current_page;


        /// <summary>
        /// Holds the ammount of time left for the player to guess the correct word.
        /// </summary>
        private TimeSpan _round_time;

        /// <summary>
        /// Holds the last checked interval of time between the current time and _round_time. 
        /// Used to check if one second has elapsed.
        /// </summary>
        private TimeSpan _check_second;

        /// <summary>
        /// This will hold the total time the player has been playing which is diplayed to the player on the end game page.
        /// </summary>
        private TimeSpan _total_time;

        /// <summary>
        /// Used to keep track of our in game round timer.
        /// </summary>
        private DateTime _time_elapsed;

        /// <summary>
        /// Game logic ticker, targeted to be called every 20 milliseconds or 50 times per second.
        /// The resolution of DispatchTimer is not reliable beyond 20 milliseconds.
        /// </summary>
        private DispatcherTimer _tick_timer;

        /// <summary>
        /// This is a constant that we will use to reset our round time whenever we need to reset it.
        /// We have it set to 120 seconds.
        /// </summary>
        private const int c_Starting_Round_Seconds = 120;

        /// <summary>
        /// Constant string messages used for databinding our post game message on the end game page.
        /// </summary>
        private const string YOU_LOSE = "YOU LOSE :P", YOU_WON = "YOU WON :D";

        /// <summary>
        /// This holds the hint for the winning word that is selected from the dictionary api.
        /// </summary>
        private string _hint;

        /// <summary>
        /// This is the winning word which is selected from our array of words.
        /// </summary>
        private string _winning_word;

        /// <summary>
        /// This is a message that is displayed to the player on the end game page which tells them whether they won or lost.
        /// </summary>
        private string _win_or_lose_message;

        /// <summary>
        /// This is a message used for databinding an indicator on the new round page showing the player their remaining guesses.
        /// </summary>
        private string _guesses_remaining;

        /// <summary>
        /// The total number of rounds the player has won. Displayed on the End Round Page.
        /// </summary>
        private int _wins;

        /// <summary>
        /// This tracks the number of guesses the player has made, for use in our logic when we need to check if they made a winning guess.
        /// </summary>
        private int _guess_count;

        /// <summary>
        /// This tracks the current number of letters in the player's current guess.
        /// </summary>
        private int _letters_in_guess_count;

        /// <summary>
        /// This a boolean flag representing the logical state of our currently displayed page for our game loop.
        /// </summary>
        private bool _to_page_end_round;

        /// <summary>
        /// This a boolean flag representing the logical state of our currently displayed page for our game loop.
        /// </summary>
        private bool _to_page_new_round;

        /// <summary>
        /// This a boolean flag representing the logical state of our currently displayed page for our game loop.
        /// </summary>
        private bool _round_in_progress;

        /// <summary>
        /// This is a boolean flag representing whether or not it is the first round played by the player.
        /// </summary>
        private bool _is_first_round;

        /// <summary>
        /// This is a class which inherits from ObservableCollection<LetterState>, it is a custom container which holds a collection of LetterState objects.
        /// This specific collection holds the states for the letters in our selection grid (the randomly shuffled grid of 26 letters)
        /// </summary>
        private LetterStateCollection _letter_states;

        /// <summary>
        /// This is a class which inherits from ObservableCollection<LetterState>, it is a custom container which holds a collection of LetterState objects.
        /// This specific collection holds the states for our current guess letters.
        /// </summary>
        private LetterStateCollection _guessed_letter_states;

        /// <summary>
        /// This is a class which inherits from ObservableCollection<LetterState>, it is a custom container which holds a collection of LetterState objects.
        /// This specific collection holds the states for the letters in our previous guesses.
        /// </summary>
        private LetterStateCollection _previously_guessed_letter_states;

        #endregion // End Fields

        #region Properties

        // These are just the public properties which are externally accessible to other classes. We have used these for data binding
        // to our WPF controls in the xaml files. They utilize custom get and set member access methods because we want set to forward
        // the specified value to the ObservableObject.SetProperty function (see the file Objects/ObservableObject.cs)

        public LetterStateCollection LetterStates
        {
            get => _letter_states;
            set => SetProperty(ref _letter_states, value);
        }
        public LetterStateCollection GuessedLetterStates
        {
            get => _guessed_letter_states;
            set => SetProperty(ref _guessed_letter_states, value);
        }
        public LetterStateCollection PreviouslyGuessedLetterStates
        {
            get => _previously_guessed_letter_states;
            set => SetProperty(ref _previously_guessed_letter_states, value);
        }

        public bool ToPageEndRound 
        {
            get => _to_page_end_round;
            set => SetProperty(ref _to_page_end_round, value);
        }

        public bool ToPageNewRound
        {
            get => _to_page_new_round;
            set => SetProperty(ref _to_page_new_round, value);
        }

        public bool RoundInProgress 
        { 
            get => _round_in_progress;
            private set => SetProperty(ref _round_in_progress, value);
        }

        public string Hint
        {
            get => _hint;
            private set => SetProperty(ref _hint, value);
        }

        public int Wins 
        {
            get => _wins;
            private set => SetProperty(ref _wins, value);
        }

        public int GuessCount
        {
            get => _guess_count;
            private set => SetProperty(ref _guess_count, value);
        }

        public string GuessesRemaining
        {
            get => _guesses_remaining;
            private set => SetProperty(ref _guesses_remaining, value);
        }

        public TimeSpan TotalTime
        {
            get => _total_time;
            private set => SetProperty(ref _total_time, value);
        }

        public string WinningWord
        {
            get => _winning_word;
            private set => SetProperty(ref _winning_word, value);
        }

        public TimeSpan RoundTime
        {
            get => _round_time;
            private set => SetProperty(ref _round_time, value);
        }

        public string WinOrLoseMessage
        {
            get => _win_or_lose_message;
            private set => SetProperty(ref _win_or_lose_message, value);
        }


        #endregion // End Properties

        #region Events and Commands
        public event ChangePageEventDelegate ChangePageEvent;

        private ICommand _get_hint_command;
        private ICommand _letter_selected_command;
        private ICommand _letter_deselected_command;
        private ICommand _start_round_command;
        private ICommand _guess_command;
        private ICommand _reset_game_command;
        private ICommand _exit_command;
        private ICommand _logic_navigate_command;

        public ICommand GetHintCommand
        {
            get => _get_hint_command ??= new RelayCommand(param => GetHint(), param => CanGetHint());
        }

        public ICommand LetterSelectedCommand
        {
            get => _letter_selected_command ??= new RelayCommand(param => LetterSelected(param), param => LetterCanBeSelected(param));
        }
        public ICommand LetterDeselectedCommand
        {
            get => _letter_deselected_command ??= new RelayCommand(param => LetterDeselected(param), param => LetterCanBeDeselected(param));
        }

        public ICommand StartRoundCommand
        {
            get => _start_round_command ??= new RelayCommand(param => StartRound(), param => CanStartRound());
        }
        
        public ICommand GuessCommand
        {
            get => _guess_command ??= new RelayCommand(param => Guess(), param => GuessCanBeMade());
        }

        public ICommand ResetGameCommand
        {
            get => _reset_game_command ??= new RelayCommand(param => ResetGame(), param => CanResetGame());
        }

        public ICommand ExitCommand
        {
            get => _exit_command ??= new RelayCommand(param => End(), param => true);
        }

        public ICommand LogicNavigateCommand
        {
            get => _logic_navigate_command ??= new RelayCommand(param => NavigatePage(param), param => CanNavigatePage());
        }

        #endregion // End Events

        #region Game Logic Member Methods
        public GameLogic()
        {
            // Variables which will accumulate over the course of our entire game logic model should be instantiated here.
            _current_page = PageState.MAIN_MENU;
            _tick_timer = new DispatcherTimer();
            TotalTime = TimeSpan.Zero;
            Wins = 0;
            _is_first_round = true;      

            // Initialize a collection of states which will keep track off our logical letters values.
            LetterStates = new LetterStateCollection { { 0, 0, 0, "A" }, { 1, 0, 0, "B" }, { 2, 0, 0, "C" }, { 3, 0, 0, "D" }, { 4, 0, 0, "E" }, { 5, 0, 0, "F" }, { 6, 0, 0,"G" }, { 7, 0, 0,"H" }, { 8, 0, 0,"I" }, { 9, 0, 0,"J" }, { 10, 0, 0,"K" }, { 11, 0, 0,"L" }, { 12, 0, 0,"M" },
                                                         { 13, 0, 0,"N" }, { 14, 0, 0,"O" }, { 15, 0, 0,"P" }, { 16, 0, 0,"Q" }, { 17, 0, 0,"R" }, { 18, 0, 0,"S" }, { 19, 0, 0,"T" }, { 20, 0, 0,"U" }, { 21, 0, 0,"V" }, { 22, 0, 0,"W" }, { 23, 0, 0,"X" }, { 24, 0, 0,"Y" }, { 25, 0, 0,"Z" } };
            // Initialize a collection of states which will keep track off our logical guessed letter values.
            GuessedLetterStates = new LetterStateCollection { { 0, 0, 0, "" }, { 1, 0, 0, "" }, { 2, 0, 0, "" }, { 3, 0, 0, "" }, { 4, 0, 0, "" } };
            // Initialize a collection of states which will keep track of our previous word guesses.
            PreviouslyGuessedLetterStates = new LetterStateCollection { { 0, 0, 0, ""},  { 1, 0, 0, "" },  { 2, 0, 0, "" },  { 3, 0, 0, "" },  { 4, 0, 0, "" },
                                                                        { 5, 0, 0, ""},  { 6, 0, 0, "" },  { 7, 0, 0, "" },  { 8, 0, 0, "" },  { 9, 0, 0, "" },
                                                                        { 10, 0, 0, ""}, { 11, 0, 0, "" }, { 12, 0, 0, "" }, { 13, 0, 0, "" }, { 14, 0, 0, "" },
                                                                        { 15, 0, 0, ""}, { 16, 0, 0, "" }, { 17, 0, 0, "" }, { 18, 0, 0, "" }, { 19, 0, 0, "" },
                                                                        { 20, 0, 0, ""}, { 21, 0, 0, "" }, { 22, 0, 0, "" }, { 23, 0, 0, "" }, { 24, 0, 0, "" } };

            // Reset is called when we need to reset values which do not need to persist between rounds, but still need to be reset.
            Reset();

            // Start the game's logic. Only called once.
            Start();
        }

        public void Reset()
        {
            // Any data variable which need to be reset per round will be reset here.
            RoundTime = TimeSpan.FromSeconds(c_Starting_Round_Seconds);
            WinningWord = "";
            Hint = "";
            GuessCount = 0;
            GuessesRemaining = "Guesses Remaining: " + GuessCount + " / 6";
            RoundInProgress = false;
            _letters_in_guess_count = 0;

            // This is just here to keep from reseting our collections to default values because we already set them in the constructor. (Probably a better way to do this.)
            if (!_is_first_round)
            {
                // Don't reset the letters for this one, they are just going to get shuffled again anyways.
                foreach(var ls in LetterStates)
                {
                    ls.State = 0;
                    ls.Count = 0;
                    ls.HasZeroCount = true;
                }

                foreach (var gls in GuessedLetterStates)
                {
                    gls.Letter = "";
                    gls.State = 0;
                    gls.Count = 0;
                    gls.HasZeroCount = true;
                }

                foreach (var pgls in PreviouslyGuessedLetterStates)
                {
                    pgls.Letter = "";
                    pgls.State = 0;
                    pgls.Count = 0;
                    pgls.HasZeroCount = true;
                }
            }
        }
        public void ResetGame() { _is_first_round = false; Reset(); }
        public bool CanResetGame() => true;

        /// <summary>
        /// Runs once at the start of the game.
        /// </summary>
        public void Start()
        {
            // Assign the tick event handler to the "Run" method. This is our GameLoop, where logic should be updated each tick.
            // Game Logic ticks every 0.02 seconds i.e. a target of 50 executions per second. Dispatch timer interval resolution is only reliable up to 20 miliseconds.
            // Start the timer. We will use DispatchTimer.Stop() later to clean up when the GameLoop is finished executing (right before the application exits.
            _tick_timer.Tick += Run;
            _tick_timer.Interval = TimeSpan.FromMilliseconds(20);
            _tick_timer.Start();
        }

        /// <summary>
        /// Runs once at the end of the game.
        /// </summary>
        public void End()
        {
            _tick_timer.Stop();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// This is the game loop. Game logic not tied to specific user input actions is handled here.
        /// Runs every tick of the game (Every 20 milliseconds).
        /// </summary>
        private void Run(object sender, EventArgs e)
        {
            
            // If we should switch to page new round reset the game state then invoke the change page event so that our
            // main window and it's child frame control is aware that we want to change pages.
            if (ToPageNewRound)
            {
                Reset();
                Trace.WriteLine("Run Loop-> Starting a new round.");
                ChangePageEvent?.Invoke(this, new ChangePageEventArgs(PageState.PAGE_NEW_ROUND));
                ToPageNewRound = false;
            }

            // If we should switch to page end round set the _is_first_round sentinel to false
            // then invoke the change page event so that our main window and it's child frame control
            // is aware that we want to change pages.
            if (ToPageEndRound)
            {
            
                _is_first_round = false;
                Trace.WriteLine("Run Loop-> Changing Page End Round.");
                ChangePageEvent?.Invoke(this, new ChangePageEventArgs(PageState.PAGE_END_ROUND));
                ToPageEndRound = false;
            }
            
            // Else we update our game round timer.
            if (RoundInProgress) { UpdateRoundTime(); }
        }

        /// <summary>
        /// This method updates the game round timer logic.
        /// It works by getting the current date time - the previous date time (_time_elapsed since last check).
        /// Then if it is greater than or equal to a timespan of 1 second we update our round timer.
        /// After we update our round timer we check to make sure that the round time has not reached zero.
        /// If there is no time left remaining for the round we set a loss for the player.
        /// </summary>
        public void UpdateRoundTime()
        {
            _check_second = DateTime.UtcNow - _time_elapsed;
            if (_check_second >= TimeSpan.FromSeconds(1))
            {
                _time_elapsed = DateTime.UtcNow;
                RoundTime -= TimeSpan.FromSeconds(1);

                if (RoundTime == TimeSpan.Zero)
                {
                    Trace.WriteLine("Out of Time! Game Over!");
                    SetLoss();
                }
            }
        }




        /// <summary>
        /// Custom shuffle algorithm because the use of ObservableCollections requires the use of
        /// ObservableCollection.Move in order to produce data binding updates.
        /// </summary>
        /// <param name="passes">This is an optional parameter in case the programmer wants to do more than one pass of the algorithm.</param>
        public void Shuffle( int passes = 1 )
        {
            var random = new Random();
            int current_idx = 0, new_idx = 0;

            for (int p = 0; p < passes; p++)
            {
                for(int i = 0; i < LetterStates.Count; i++)
                {
                    current_idx = i;
                    new_idx = current_idx;

                    while (current_idx == new_idx)
                        new_idx = random.Next(0, LetterStates.Count);

                    LetterStates[i].Index = new_idx;
                    LetterStates.Move(current_idx, new_idx);
                }
            }
        }

        /// <summary>
        /// This method handles the start of a new round. It is triggered as a relay command delegate whenever a new game is started.
        /// </summary>
        public void StartRound()
        {
            // Reset all the variables needed for a new round.
            Reset();
            // Shuffle the letters in our collection.
            Shuffle();

            // Get the winning word for this round.
            WinningWord = WordManager.ChooseWord();
            Trace.WriteLine($"The WinningWord is: {WinningWord}");

            // These should be reset in this order. Get the initial date time so that when we update our
            // round time we have a starting point for this round.
            _time_elapsed = DateTime.UtcNow;

            // Finally set the flag to let our game logic loop know we have started a new round and need to start updating logic in the loop.
            RoundInProgress = true;
        }

        public bool CanStartRound()
        {
            // If the round is not in progress then we can start the round.
            if (!RoundInProgress)
                return true;

            // Otherwise we have already started a round and can not start another one (use reset game button for debugging).
            return false;
        }

        /// <summary>
        /// This method is a command delegate that retrieves a hint for the current winning word when the player requests one.
        /// The way this works is by making a call to dictionary api (free online dictionary resource), through the use of an
        /// HttpClient message request. We put the result of that request into a new DictionaryWord instance after deserializing
        /// the returned Json object using the System Json Deserializing class. After that we set the string that corresponds to
        /// the dictionary element from the Json data object to our Hint property (which will update our xaml control to display it).
        /// (See /Utilities/DictionaryWord folder and corresponding C# code files there for more information on this.)
        /// </summary>
        public void GetHint()
        {
            if (WinningWord == "")
            {
                Trace.WriteLine($"WinningWord is empty");
                return;
            }

            HttpClient client = new HttpClient();

            var url = "https://api.dictionaryapi.dev/api/v2/entries/en/" + WinningWord;

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = new StringContent(String.Empty);
            var lookup = client.Send(request);

            var reader = new Utf8JsonReader(lookup.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult());
            var word = System.Text.Json.JsonSerializer.Deserialize<List<DictionaryWord>>(ref reader, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true})?[0];
            Hint = word.Meanings[0].Type[0].Definition;

            Trace.WriteLine($"Hint: {Hint}");

        }
        private bool CanGetHint()
        {
            if (Hint != "")
            {
                return false;
            }
            else if (String.IsNullOrEmpty(WinningWord))
            {
                Trace.WriteLine("Can not get hint! Winning Word is Null.");
                return false;
            }
            else
            {
                Trace.WriteLine("Hint can be retrieved!");
                return true;
            }
        }

        /// <summary>
        /// This is a command that is called whenever the user selects a  letter from our collection.
        /// </summary>
        /// <param name="param"></param>
        private void LetterSelected(object param)
        {

            // This will convert the passed param object, which corresponds to the tag property on our button control elements from a string into an int.
            // Then we will use that converted value to retrieve the correct letter state with the corresponding index.
            int idx_ls = Convert.ToInt32(param);
            var ls = LetterStates[idx_ls];
            // Find the first instance of a guessed letter state where the letter property is empty.
            int idx_gls = GuessedLetterStates.First(x => x.Letter == "").Index;
            var gls = GuessedLetterStates[idx_gls];

            // Assign the letter from the sent letter state to the next empty index of the GuessedLetters LetterStatesCollection.
            // Then Store the index from this LetterState in this GuessedLetterState for fast and easy lookup later.
            // Set our HasZeroCount sentinel flag to be equal to the boolean return of Count == 0, i.e. is count for this letter state zero?
            // Lastly, increment the state count property for the newly guessed letter and the total number of letter guesses.
            gls.Letter = ls.Letter;
            gls.State = idx_ls;
            ls.Count++;
            ls.HasZeroCount = ls.Count == 0;
            _letters_in_guess_count++;

            Trace.WriteLine($"\tThe guessed letter index is: {gls.State}.\n\tThe letter added to the current guess is is: {gls.Letter}.\n\tThe selection count of the letter is: {ls.Count}.\n\tThe current number of letter guesses is {_letters_in_guess_count}.\n\tThe flag HasZeroCount is {ls.HasZeroCount}");

            // Assign modified temporaries back to the correct states.
            LetterStates[idx_ls] = ls;
            GuessedLetterStates[idx_gls] = gls;
        }

        private bool LetterCanBeSelected(object param)
        {
            if (RoundInProgress && _letters_in_guess_count < 5)
                return true;

            return false;
        }

        /// <summary>
        /// This method is a command that handles when a letter has been deselected by the player.
        /// </summary>
        /// <param name="param"></param>
        private void LetterDeselected(object param)
        {
            // Cache local copies of our states so we are not doing so many collection lookups each time we process something and improved readability.
            int deselected_letter_index = Convert.ToInt32(param);
            var gls = GuessedLetterStates[deselected_letter_index];
            var ls = LetterStates[gls.State];

            Trace.WriteLine($"Player deselected the letter at index: {deselected_letter_index}");

            // Set the deselected guessed letter to empty and decrement the selection count of the letter state.
            gls.Letter = "";
            ls.Count--;
            ls.HasZeroCount = ls.Count == 0;

            Trace.WriteLine($"The letter {ls.Letter} has been selected {ls.Count} times. The flag HasZeroCount is {ls.HasZeroCount}");

            // Assign modified temporaries back to the correct states.
            LetterStates[gls.State] = ls;
            GuessedLetterStates[deselected_letter_index] = gls;
            --_letters_in_guess_count;
        }

        private bool LetterCanBeDeselected(object param)
        {
            int letter_to_check_index = Convert.ToInt32(param);

            if (RoundInProgress && _letters_in_guess_count > 0 && GuessedLetterStates[letter_to_check_index].Letter != "")
                return true;

            return false;
        }

        /// <summary>
        /// This method is sets up the correct values for a win if the player wins the game at any point.
        /// </summary>
        private void SetWin()
        {
            Trace.WriteLine($"Player Won! Current Number of Wins: {Wins}");
            Wins++;
            TotalTime += (TimeSpan.FromSeconds(c_Starting_Round_Seconds) - RoundTime);
            Hint = "";
            WinOrLoseMessage = YOU_WON;
            ToPageEndRound = true;
            RoundInProgress = false;
        }

        /// <summary>
        /// This method is sets up the correct values for a loss if the player loses the game at any point.
        /// </summary>
        private void SetLoss()
        {
            Trace.WriteLine($"Out of Tries... Game Over!");
            TotalTime += (TimeSpan.FromSeconds(c_Starting_Round_Seconds) - RoundTime);
            Hint = "";
            WinOrLoseMessage = YOU_LOSE;
            ToPageEndRound = true;
            RoundInProgress = false;
        }

        /// <summary>
        /// This method is a command that is called whenever the player makes a word guess.
        /// </summary>
        private void Guess()
        {
            // Calculate the index of our next previous guess before incrementing the guess count.
            // This is used in order to treat our 1-dimensional collection of previously guessed words
            // as if it were a 2-dimensional collection.
            int previous_guess_index = 5 * GuessCount;
            GuessCount++;

            // Cache the current guess for processing and make it lower case because we have our array of word strings in lower case.
            var guess = GuessedLetterStates.LettersToString().ToLower();
            Trace.WriteLine($"Guess made: {guess}, Tries left: {GuessCount}.");

            // Player wins so we set the appropriate flags, which will be picked up by the rest of our logic automatically in the game loop.
            if (WinningWord == guess)
            {
                SetWin();
                return;
            }

            // The player only gets 6 tries per round so if this is true it's game over.
            // So if this is try number 6 and it wasn't the winning word then we don't need to evaluate the rest.
            if (GuessCount > 5)
            {
                SetLoss();
                return;
            }


            // If we get to here that means we still have tries left but the guess wasn't the winning word.
            // So we need to do some checking to see which letters should be highlighted.
            for (int idx = 0; idx < guess.Length; idx++)
            {
                char c = guess[idx];

                if (WinningWord.Contains(c))
                {
                    if(c == WinningWord[idx])
                    {
                        // The letter is in the word and in the correct location within the word.
                        PreviouslyGuessedLetterStates[idx + previous_guess_index].State = 3; // Green
                        PreviouslyGuessedLetterStates[idx + previous_guess_index].Letter = GuessedLetterStates[idx].Letter;
                        GuessedLetterStates[idx].Letter = "";
                        LetterStates[GuessedLetterStates[idx].State].Count = 0;
                        LetterStates[GuessedLetterStates[idx].State].HasZeroCount = true;
                        Trace.WriteLine($"The Letter {c} is in the WinningWord and in the correct location.");
                    }
                    else
                    {
                        // The letter is in the word but not in the correct location within the word.
                        PreviouslyGuessedLetterStates[idx + previous_guess_index].State = 2; // Yellow
                        PreviouslyGuessedLetterStates[idx + previous_guess_index].Letter = GuessedLetterStates[idx].Letter;
                        GuessedLetterStates[idx].Letter = "";
                        LetterStates[GuessedLetterStates[idx].State].Count = 0;
                        LetterStates[GuessedLetterStates[idx].State].HasZeroCount = true;
                        Trace.WriteLine($"The Letter {c} is in the WinningWord but not in the correct location.");
                    }
                }
                else
                {
                    // Not in the word but has been tested.
                    PreviouslyGuessedLetterStates[idx + previous_guess_index].State = 1;
                    PreviouslyGuessedLetterStates[idx + previous_guess_index].Letter = GuessedLetterStates[idx].Letter;
                    LetterStates[GuessedLetterStates[idx].State].State = 1; // Gray
                    GuessedLetterStates[idx].Letter = "";
                    LetterStates[GuessedLetterStates[idx].State].Count = 0;
                    LetterStates[GuessedLetterStates[idx].State].HasZeroCount = true;
                    Trace.WriteLine($"The Letter {c} is not in the WinningWord.");
                }
            }

            // Update our string indicator so that our player is informed that have fewer guesses left.
            GuessesRemaining = "Guesses Remaining: " + GuessCount + " / 6";

            // Also reset the letter guess count otherwise the player won't be able to make any more guesses.
            _letters_in_guess_count = 0;
        }

        private bool GuessCanBeMade()
        {
            var guess = GuessedLetterStates.LettersToString().ToLower();

            if (RoundInProgress && _letters_in_guess_count == 5 && WordManager.Approved(guess))
                return true;

            return false;
        }

        private bool CanNavigatePage() => true;

        /// <summary>
        /// This method is a command that handles our customized page navigation. Any changes to our page state
        /// that we make here will be automatically picked up in our game logic loop.
        /// </summary>
        /// <param name="param"></param>
        private void NavigatePage(object param)
        {
            var page = (PageState)Convert.ToUInt32(param);
            Trace.WriteLine($"Navigate Page-> Current Page is: {_current_page}. We are changing to: {page}");

            if (page == PageState.PAGE_END_ROUND)
            {
                Trace.WriteLine("Navigate Page-> Setting ToPageEndRound to True.");
                ToPageEndRound = true;
            }
            else if (page == PageState.PAGE_NEW_ROUND)
            {
                Trace.WriteLine("Navigate Page-> Setting ToPageNewRound to True.");
                ToPageNewRound = true;
            }

            _current_page = page;
            Trace.WriteLine($"Navigate Page-> CurrentPage: {_current_page}");
        }

        #endregion // End Game Logic Member Methods
    }
}
