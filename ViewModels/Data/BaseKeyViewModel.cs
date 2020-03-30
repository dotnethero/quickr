using System;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal abstract class BaseKeyViewModel : BaseViewModel
    {
        private ValueViewModel _value;

        protected RedisProxy Proxy { get; }

        public KeyEntry Key { get; }
        public PropertiesViewModel Properties { get; }

        public ValueViewModel Value
        {
            get => _value;
            protected set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        protected BaseKeyViewModel(RedisProxy proxy, KeyEntry key)
        {
            Proxy = proxy;
            Key = key;
            Properties = CreatePropertiesViewModel();
            Value = null;
        }

        private PropertiesViewModel CreatePropertiesViewModel()
        {
            var name = Key.FullName;
            var expiration = Proxy.GetTimeToLive(Key);
            var props = new PropertiesViewModel(name, expiration);
            props.ValueSaved += OnPropertiesSaved;
            props.ValueDiscarded += OnPropertiesDiscarded;
            return props;
        }

        private void OnPropertiesSaved(object sender, EventArgs args)
        {
            if (Properties.Expiration != Properties.OriginalExpiration)
            {
                Proxy.SetTimeToLive(Key, Properties.Expiration);
                Properties.OriginalExpiration = Properties.Expiration;
            }
            if (Properties.Name != Properties.OriginalName)
            {
                Proxy.RenameKey(Key, Properties.Name);
                Key.FullName = Properties.Name;
                Properties.OriginalName = Properties.Name;
            }
        }
        
        private void OnPropertiesDiscarded(object sender, EventArgs args)
        {
            Properties.Name = Properties.OriginalName;
            Properties.Expiration = Properties.OriginalExpiration;
        }
    }
}