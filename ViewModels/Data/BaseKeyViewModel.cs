using System;
using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels.Data
{
    internal abstract class BaseKeyViewModel : BaseViewModel
    {
        public PropertiesViewModel Properties { get; }

        protected KeyEntry Key { get; }
        protected RedisProxy Proxy { get; }

        private ValueViewModel _value;

        public ValueViewModel Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_value != null)
                {
                    _value.OnValueSaved += OnValueSaved;
                }
                OnPropertyChanged();
            }
        }

        protected BaseKeyViewModel(RedisProxy proxy, KeyEntry key)
        {
            Proxy = proxy;
            Key = key;
            Properties = new PropertiesViewModel(proxy, key);
            Value = null;
        }

        protected abstract void OnValueSaved(object sender, EventArgs e);
    }
}