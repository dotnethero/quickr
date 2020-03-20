using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    internal class HashEntryViewModel : BaseViewModel
    {
        private string _name;
        private string _value;
        private bool _isSaved;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                _isSaved = value;
                OnPropertyChanged();
            }
        }

        public static HashEntryViewModel FromHashEntry(HashEntry entry) => 
            new HashEntryViewModel
            {
                Name = entry.Name, 
                Value = entry.Value,
                IsSaved = true
            };
    }
}