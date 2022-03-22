using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordGame_new
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string winning_word = "store";
        string guessed_word;
        string firstLetter = "";
        List<string> isIn = new List<string>();
        List<string> guessed = new List<string>();
        List<string> winning_guess = new List<string>();
        List<string> guessed_word_Bname = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();
        }

        private void SetUpGame()
        {
            List<string> letters = new List<string>()
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y"
            };

            Random random = new Random();

            foreach (Button button in mainGrid.Children.OfType<Button>())
            {
                int index = random.Next(letters.Count);
                string nextLetter = letters[index];
                button.Content = nextLetter;
                letters.RemoveAt(index);
            }

            //Sound();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            button.Background = Brushes.Blue;

            guessed_word_Bname.Add(button.Name);

            guessed_word += button.Content.ToString();

            box.Text = guessed_word;
        }

        private void Guess_Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (box.Text.Length < 5)
            {
                return;
            }
            else if (Approved(guessed_word))
            {
                sort(guessed_word_Bname, guessed_word);
                Guess_Box.Text += "\n" + guessed_word.ToString();
                ChangeColor(winning_word, guessed_word, firstLetter, isIn, guessed, winning_guess);
                guessed_word = "";
                guessed_word_Bname.Clear();
            }
            else
            {
                guessed_word = "";
                box.Text = "";
                guessed_word_Bname.Clear();
                ChangeColor(winning_word, guessed_word, firstLetter, isIn, guessed, winning_guess);
            }

        }

        private bool Approved(string guess)
        {
            string[] words = { "store", "astro", "bunch", "cords" };

            if (words.Contains(guess)) { return true; }
            else { return false; }
        }

        private void ChangeColor(string winning_word, string text2, string firstLetter, List<string> isIn, List<string> guessed, List<string> winning_guess)
        {
            foreach (Button buttons in mainGrid.Children.OfType<Button>())
            {
                if (winning_word == text2)
                {
                    if (winning_guess.Contains(buttons.Name))
                    {
                        buttons.Background = Brushes.Gold;
                    }
                    else if (!winning_guess.Contains(buttons.Name))
                    {
                        buttons.Background = Brushes.Black;
                    }
                }
                else if (buttons.Name == firstLetter)
                {
                    buttons.Background = Brushes.Red;
                }
                else if (isIn.Contains(buttons.Name))
                {
                    buttons.Background = Brushes.Green;
                }
                else if (guessed.Contains(buttons.Name))
                {
                    buttons.Background = Brushes.LightGray;
                }
                else
                {
                    buttons.Background = Brushes.Coral;
                }
            }
        }

        private void sort(List<string> guessed_word_Bname, string guessed_word)
        {
            for (int i=0; i < 5; i++)
            {
                if (guessed_word[i] == winning_word[0])
                {
                    firstLetter = guessed_word_Bname[i].ToString();
                    winning_guess.Add(guessed_word_Bname[i]);
                }

            }
            
            for (int i = 0; i < 5; i++)
            {
                if (guessed_word[i] != winning_word[0])
                {
                    if (winning_word.Contains(guessed_word[i]))
                    {
                        isIn.Add(guessed_word_Bname[i]);
                        winning_guess.Add(guessed_word_Bname[i]);
                    }
                    else
                    {
                        guessed.Add(guessed_word_Bname[i]);
                    }
                }
                
            }
        }
    }
}
