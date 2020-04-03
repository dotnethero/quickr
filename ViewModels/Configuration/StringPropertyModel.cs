using Quickr.Models.Configuration;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Configuration
{
    internal class StringPropertyModel : BaseViewModel, IPropertyModel
    {
        public ValueViewModel<string> Value { get; }

        public bool IsPropertyChanged => Value.IsValueChanged;

        public StringPropertyModel(ConfigKeyValue spec)
        {
            var str = string.IsNullOrEmpty(spec.Value) ? spec.DefaultValue : spec.Value;
            Value = new ValueViewModel<string>(str, str);
        }

        public string Serialize() => Value.CurrentValue;

        public override string ToString() => Serialize();
    }
}