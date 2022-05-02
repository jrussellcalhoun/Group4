namespace WordGame.Objects
{
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
