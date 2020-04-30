namespace Quickr.ViewModels.Data
{
    internal abstract class BaseEntryViewModel : BaseViewModel
    {
        private string _originalValue;
        private string _currentValue;

        public string OriginalValue
        {
            get => _originalValue;
            set
            {
                if (_originalValue == value) return;
                _originalValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueSaved));
                OnPropertyChanged(nameof(IsNew));
            }
        }

        public string CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue == value) return;
                _currentValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueSaved));
            }
        }

        public virtual bool IsValueSaved => OriginalValue == CurrentValue;
        public virtual bool IsNew => OriginalValue == null;

        protected BaseEntryViewModel()
        {
        }

        protected BaseEntryViewModel(string value)
        {
            _originalValue = value;
            _currentValue = value;
        }
    }
}