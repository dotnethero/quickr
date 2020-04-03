using System;
using Quickr.Models.Configuration;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Configuration
{
    internal class YesNoPropertyModel: BaseViewModel, IPropertyModel
    {
        private readonly bool _original;
        private bool _isYes;

        public bool IsYes
        {
            get => _isYes;
            set
            {
                if (value == _isYes) return;
                _isYes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPropertyChanged));
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

        public string Serialize() => IsYes ? "yes" : "no";

        public override string ToString() => Serialize();
    }
}