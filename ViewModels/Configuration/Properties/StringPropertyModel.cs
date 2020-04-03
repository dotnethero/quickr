using Quickr.Models.Configuration;

namespace Quickr.ViewModels.Configuration
{
    internal class StringPropertyModel : BaseViewModel, IPropertyModel
    {
        private readonly string _original;
        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPropertyChanged));
            }
        }

        public bool IsPropertyChanged => Value != _original;

        public ConfigKeyValue Config { get; }

        public StringPropertyModel(ConfigKeyValue config)
        {
            Config = config;
            Value = _original = string.IsNullOrEmpty(config.Value) ? config.DefaultValue : config.Value;
        }

        public string Serialize() => Value;

        public override string ToString() => Serialize();
    }
}