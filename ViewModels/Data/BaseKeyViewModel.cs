using System;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal abstract class BaseKeyViewModel : BaseViewModel
    {
        private ValueViewModel _value;

        protected RedisConnection Connection { get; }

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

        protected BaseKeyViewModel(RedisConnection connection, KeyEntry key, TimeSpan? ttl)
        {
            Connection = connection;
            Key = key;
            Properties = CreatePropertiesViewModel(ttl);
            Value = null;
        }

        private PropertiesViewModel CreatePropertiesViewModel(TimeSpan? ttl)
        {
            var name = Key.FullName;
            var props = new PropertiesViewModel(name, ttl);
            props.ValueSaved += OnPropertiesSaved;
            props.ValueDiscarded += OnPropertiesDiscarded;
            return props;
        }

        private void OnPropertiesSaved(object sender, EventArgs args)
        {
            if (Properties.Expiration != Properties.OriginalExpiration)
            {
                Connection.SetTimeToLive(Key, Properties.Expiration);
                Properties.OriginalExpiration = Properties.Expiration;
            }
            if (Properties.Name != Properties.OriginalName)
            {
                Connection.RenameKey(Key, Properties.Name);
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