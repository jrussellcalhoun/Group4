using System.Collections.ObjectModel;

namespace WordGame.Objects
{
    /// <summary>
    /// This is a collection that makes it easier to loop through Letters for logical processing while simultaneously having them
    /// bound to individual xaml controls (like our letter buttons). Though we must do the binding programatically, it is less
    /// verbose than binding individually each object.
    /// </summary>
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
