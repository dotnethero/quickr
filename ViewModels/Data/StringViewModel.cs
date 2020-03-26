using System;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal class StringViewModel: BaseKeyViewModel
    {
        public StringViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Value = new ValueViewModel(Proxy.GetString(Key));
            Value.ValueSaved += OnValueSaved;
        }

        protected void OnValueSaved(object sender, EventArgs e)
        {
            Proxy.SetString(Key, Value.CurrentValue);
            Value.OriginalValue = Value.CurrentValue;
        }
    }
}
