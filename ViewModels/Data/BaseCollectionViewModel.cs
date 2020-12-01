using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Data
{
    abstract class BaseCollectionViewModel<TEntry> : BaseValueViewModel where TEntry: BaseEntryViewModel
    {
        TEntry _current;
        ObservableCollection<TEntry> _entries;

        public ObservableCollection<TEntry> Entries
        {
            get => _entries;
            set
            {
                if (Equals(value, _entries)) return;
                BeforePropertyChanged();
                _entries = value;
                OnPropertyChanged();
            }
        }

        public TEntry Current
        {
            get => _current;
            set
            {
                if (_current == value) return;
                BeforePropertyChanged();
                _current = value;
                OnPropertyChanged();
            }
        }

        public override bool IsUnsaved => Entries.Any(x => !x.IsValueSaved);

        public override bool IsKeyRemoved => !Entries.Any();

        protected BaseCollectionViewModel(KeyEntry key) : base(key)
        {
            Entries = new ObservableCollection<TEntry>();
        }

        void BeforePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Current) && Current != null)
            {
                Current.ValueSaved -= SaveHandler;
                Current.ValueSaved -= DiscardHandler;
                Current.PropertyChanged -= PropertyChangedHandler;
            }

            if (propertyName == nameof(Entries) && Entries != null)
            {
                Entries.CollectionChanged -= CollectionChangedHandler;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Current) && Current != null)
            {
                Current.ValueSaved += SaveHandler;
                Current.ValueDiscarded += DiscardHandler;
                Current.PropertyChanged += PropertyChangedHandler;
            }

            if (propertyName == nameof(Entries) && Entries != null)
            {
                Entries.CollectionChanged += CollectionChangedHandler;
            }

            base.OnPropertyChanged(propertyName);
        }

        async void SaveHandler(object sender, EventArgs e) => await SaveItem();
        async void DiscardHandler(object sender, EventArgs e) => await DiscardItemChanges();

        void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseEntryViewModel.IsValueSaved)) 
                OnPropertyChanged(nameof(IsUnsaved));
        }

        void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsUnsaved));
        }

        protected abstract Task SaveItem();
        protected abstract Task DiscardItemChanges();
    }
}