using System;
using System.Threading.Tasks;
using Quickr.Models.Keys;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal class StringViewModel: BaseKeyViewModel
    {
        private ValueViewModel _value;

        public ValueViewModel Value
        {
            get => _value;
            protected set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public override bool IsUnsaved => Value.IsValueChanged;

        public StringViewModel(KeyEntry key, TimeSpan? ttl): base(key, ttl)
        {
            SetupAsync();
        }
        
        private async void SetupAsync()
        {
            var str = await Key.GetDatabase().GetString(Key);
            Value = new ValueViewModel(str);
            Value.ValueSaved += async (sender, e) => await Save();
        }

        public override async Task<bool> Save()
        {
            await Key.GetDatabase().SetString(Key, Value.CurrentValue);
            Value.OriginalValue = Value.CurrentValue;
            return true;
        }
    }
}
