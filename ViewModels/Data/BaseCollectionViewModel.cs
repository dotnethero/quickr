using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
                _current = value;
                OnPropertyChanged();
            }
        }

        public override bool IsUnsaved => Entries.Any(x => !x.IsValueSaved);
        public override bool IsKeyRemoved => !Entries.Any();

        protected BaseCollectionViewModel(KeyEntry key, TimeSpan? ttl) : base(key, ttl)
        {
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(Current))
            {
                if (Current != null)
                {
                    // TODO: Unsubscribe previous
                    Current.ValueSaved += OnValueSaved;
                    Current.ValueDiscarded += OnValueDiscarded;
                }
            }
            base.OnPropertyChanged(propertyName);
        }

        protected abstract void OnValueSaved(object sender, EventArgs e);

        protected abstract void OnValueDiscarded(object sender, EventArgs e);
    }
}