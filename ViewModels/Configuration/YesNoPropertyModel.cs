using System;
using Quickr.Models.Configuration;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Configuration
{
    internal class YesNoPropertyModel: BaseViewModel, IPropertyModel
    {
        public ValueViewModel<bool> IsYes { get; }

        public bool IsPropertyChanged => IsYes.IsValueChanged;

        public YesNoPropertyModel(ConfigKeyValue spec)
        {
            var str = string.IsNullOrEmpty(spec.Value) ? spec.DefaultValue : spec.Value;
            var yes = str.ToLowerInvariant() switch
            {
                "yes" => true,
                "no" => false,
                _ => throw new InvalidOperationException($"Neither value '{spec.Value}' not default value '{spec.DefaultValue}' can be parsed")
            };
            IsYes = new ValueViewModel<bool>(yes, yes);
        }

        public string Serialize() => IsYes.CurrentValue ? "yes" : "no";

        public override string ToString() => Serialize();
    }
}