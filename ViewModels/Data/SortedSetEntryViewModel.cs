using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    class SortedSetEntryViewModel : BaseEntryViewModel
    {
        double _originalScore;
        double _currentScore;

        // ReSharper disable CompareOfFloatsByEqualityOperator

        public double OriginalScore
        {
            get => _originalScore;
            set
            {
                if (_originalScore == value) return;
                _originalScore = value;
                OnValuePropertyChanged();
            }
        }

        public double CurrentScore
        {
            get => _currentScore;
            set
            {
                if (_currentScore == value) return;
                _currentScore = value;
                OnValuePropertyChanged();
            }
        }
        
        public override bool IsValueChanged => 
            OriginalValue != CurrentValue ||
            OriginalScore != CurrentScore;

        public override bool IsValueSaved => !IsValueChanged;

        public static SortedSetEntryViewModel FromEntry(SortedSetEntry entry) => new SortedSetEntryViewModel(entry);
        public static SortedSetEntryViewModel Empty() => new SortedSetEntryViewModel();

        protected SortedSetEntryViewModel()
        {
        }

        protected SortedSetEntryViewModel(SortedSetEntry entry) : base(entry.Element)
        {
            _originalScore = entry.Score;
            _currentScore = entry.Score;
        }

        public RedisValue ToOriginalValue() => OriginalValue;
        public SortedSetEntry ToEntry() => new SortedSetEntry(CurrentValue, CurrentScore);
    }
}