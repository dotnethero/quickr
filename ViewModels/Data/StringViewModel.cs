using System;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal class StringViewModel: BaseKeyViewModel
    {
        public StringViewModel(KeyEntry key, TimeSpan? ttl): base(key, ttl)
        {
            SetupAsync();
        }

        private async void SetupAsync()
        {
            var str = await Key.GetDatabase().GetStringAsync(Key);
            Value = new ValueViewModel(str);
            Value.ValueSaved += OnValueSaved;
        }

        protected void OnValueSaved(object sender, EventArgs e)
        {
            Key.GetDatabase().SetString(Key, Value.CurrentValue);
            Value.OriginalValue = Value.CurrentValue;
        }
    }
}
