using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class SortedSetEntryViewModel : BaseEntryViewModel
    {
        private double _originalScore;
        private double _currentScore;

        // ReSharper disable CompareOfFloatsByEqualityOperator

        public double OriginalScore
        {
            get => _originalScore;
            set
            {
                if (_originalScore == value) return;
                _originalScore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueSaved));
            }
        }

        public double CurrentScore
        {
            get => _currentScore;
            set
            {
                if (_currentScore == value) return;
                _currentScore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueSaved));
            }
        }

        public override bool IsValueSaved => 
            OriginalValue == CurrentValue && 
            OriginalScore == CurrentScore;

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
    }
}