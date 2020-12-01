using Quickr.Models.Configuration;

namespace Quickr.ViewModels.Configuration
{
    class StringPropertyModel : BaseViewModel, IPropertyModel
    {
        string _original;
        string _value;
        bool _isSaveFailed;

        public string Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                _isSaveFailed = false;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPropertyChanged));
                OnPropertyChanged(nameof(IsSaveFailed));
            }
        }

        public bool IsSaveFailed
        {
            get => _isSaveFailed;
            set
            {
                if (value == _isSaveFailed) return;
                _isSaveFailed = value;
                OnPropertyChanged();
            }
        }

        public bool IsPropertyChanged => Value != _original;

        public ConfigKeyValue Config { get; }

        public StringPropertyModel(ConfigKeyValue config)
        {
            Config = config;
            Value = _original = string.IsNullOrEmpty(config.Value) ? config.DefaultValue : config.Value;
        }

        public void ApplyCurrentValue()
        {
            _original = Value;
            OnPropertyChanged(nameof(IsPropertyChanged));
        }

        public string Serialize() => Value;

        public override string ToString() => Serialize();
    }
}