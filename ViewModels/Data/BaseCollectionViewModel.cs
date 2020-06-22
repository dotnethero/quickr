using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Data
{
    internal abstract class BaseCollectionViewModel<TEntry> : BaseKeyViewModel where TEntry: BaseEntryViewModel
    {
        private TEntry _current;
        private ObservableCollection<TEntry> _entries;

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

        protected BaseCollectionViewModel(KeyEntry key, TimeSpan? ttl) : base(key, ttl)
        {
            Entries = new ObservableCollection<TEntry>();
        }
        
        private void BeforePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Current) && Current != null)
            {
                Current.ValueSaved -= SaveHandler;
                Current.ValueSaved -= DiscardHandler;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Current) && Current != null)
            {
                Current.ValueSaved += SaveHandler;
                Current.ValueDiscarded += DiscardHandler;
            }

            base.OnPropertyChanged(propertyName);
        }

        private async void SaveHandler(object sender, EventArgs e) => await SaveItem();
        private async void DiscardHandler(object sender, EventArgs e) => await DiscardItemChanges();

        protected abstract Task SaveItem();
        protected abstract Task DiscardItemChanges();
    }
}