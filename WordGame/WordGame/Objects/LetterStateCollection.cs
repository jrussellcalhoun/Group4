using System.Collections.ObjectModel;

namespace WordGame.Objects
{
    // This is mostly for the sake of brevity and initializer list syntax.
    public class LetterStateCollection : ObservableCollection<LetterState>
    {
        public void Add(int index, int state, int count, string letter) =>
            this.Add(new LetterState() { Index = index, State = state, HasZeroCount = count == 0, Count = count, Letter = letter });

        public string LettersToString()
        {
            string letters_as_string = "";
            foreach (var ls in this)
                letters_as_string += ls.Letter;

            return letters_as_string;
        }
    }
}
