using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal abstract class BaseCollectionViewModel<TEntry> : BaseKeyViewModel where TEntry: BaseEntryViewModel
    {
        private TEntry _current;
        
        public ObservableCollection<TEntry> Entries { get; set; }

        public TEntry Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                _current = value;
                OnPropertyChanged();
            }
        }

        protected BaseCollectionViewModel(RedisProxy proxy, KeyEntry key, TimeSpan? ttl) : base(proxy, key, ttl)
        {
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Current))
            {
                if (Current != null)
                {
                    Value = new ValueViewModel(Current.OriginalValue, Current.CurrentValue);
                    Value.ValueSaved += OnValueSaved;
                    Value.ValueDiscarded += OnValueDiscarded;
                    Value.PropertyChanged += OnValuePropertyChanged;
                    Current.PropertyChanged += OnCurrentPropertyChanged;
                }
                else
                {
                    Value = null;
                }
            }
            base.OnPropertyChanged(propertyName);
        }

        private void OnValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ValueViewModel.CurrentValue))
            {
                Current.CurrentValue = Value.CurrentValue;
            }
        }

        private void OnCurrentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseEntryViewModel.OriginalValue))
            {
                Value.OriginalValue = Current.OriginalValue;
            }
            if (e.PropertyName == nameof(BaseEntryViewModel.CurrentValue))
            {
                Value.CurrentValue = Current.CurrentValue;
            }
        }

        protected abstract void OnValueSaved(object sender, EventArgs e);

        protected abstract void OnValueDiscarded(object sender, EventArgs e);
    }
}