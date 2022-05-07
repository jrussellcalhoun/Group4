namespace WordGame.Objects
{
    /// <summary>
    /// This is a Model-like object which is of a Data Oriented Design. By that I mean it is here to encapsulate instances of 
    /// a specific data set in one place which can be processed by other models in our application or have the data bound to
    /// our view models (Windows, Pages).
    /// 
    /// LetterState holds:
    /// 
    /// _index variable which is used to store index data of other LetterStates which might be held in another
    /// collection. We need to crossreference collections in several places in our logic so it makes sense to keep track of these in
    /// one place.
    /// 
    /// _state variable which represents the logical state of the letter (i.e. things like is it currently selected by the user, or
    /// is it in the winning word and if so how accurate is it compared to the winning word?)
    /// 
    /// _count variable which stores the number of times the specific letter is in the collection, which is useful for
    /// when we need to ask logical questions like do we have 2 "A"s or 3 "A"s?
    /// 
    /// _has_zero_count variable is a boolean flag for easier data binding to xaml controls for representing the state of whether or not
    /// the letter is selected at all. i.e. do we need to give it the red outline or not?
    /// 
    /// _letter variable stores the actual string representation of the letter for the LetterState instance.
    /// </summary>
    public class LetterState : ObservableObject
    {
        private int _index;
        private int _state;
        private int _count;
        private bool _has_zero_count;
        private string _letter;

        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        public int State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public bool HasZeroCount
        {
            get => _has_zero_count;
            set => SetProperty(ref _has_zero_count, value);
        }

        public string Letter
        {
            get => _letter;
            set => SetProperty(ref _letter, value);
        }

        public LetterState() { }

        public LetterState(int index, int state, int count, string letter)
        {
            _index = index;
            _state = state;
            _count = count;
            _has_zero_count = true;
            _letter = letter;   
        }  
    }
}
