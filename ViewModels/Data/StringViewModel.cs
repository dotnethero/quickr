using System;
using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels.Data
{
    internal class StringViewModel
    {
        private readonly RedisProxy _proxy;

        public KeyEntry Key { get; }
        public PropertiesViewModel Properties { get; }
        public ValueViewModel Value { get; set; }

        public StringViewModel(RedisProxy proxy, KeyEntry key)
        {
            _proxy = proxy;
            Key = key;
            Properties = new PropertiesViewModel(proxy, key);
            Value = new ValueViewModel(proxy.GetString(key));
            Value.OnValueSaved += OnValueSaved;
        }

        private void OnValueSaved(object sender, EventArgs e)
        {
            _proxy.SetString(Key, Value.CurrentValue);
            Value.OriginalValue = Value.CurrentValue;
        }
    }
}
