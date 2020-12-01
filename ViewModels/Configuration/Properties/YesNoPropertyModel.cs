using System;
using Quickr.Models.Configuration;

namespace Quickr.ViewModels.Configuration
{
    class YesNoPropertyModel: BaseViewModel, IPropertyModel
    {
        bool _original;
        bool _isYes;
        bool _isSaveFailed;

        public bool IsYes
        {
            get => _isYes;
            set
            {
                if (value == _isYes) return;
                _isYes = value;
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

        public bool IsPropertyChanged => IsYes != _original;

        public ConfigKeyValue Config { get; }

        public YesNoPropertyModel(ConfigKeyValue config)
        {
            Config = config;

            var str = string.IsNullOrEmpty(config.Value) ? config.DefaultValue : config.Value;
            IsYes = _original = str.ToLowerInvariant() switch
            {
                "yes" => true,
                "no" => false,
                _ => throw new InvalidOperationException($"Neither value '{config.Value}' not default value '{config.DefaultValue}' can be parsed")
            };
        }
        
        public void ApplyCurrentValue()
        {
            _original = IsYes;
            OnPropertyChanged(nameof(IsPropertyChanged));
        }

        public string Serialize() => IsYes ? "yes" : "no";

        public override string ToString() => Serialize();
    }
}