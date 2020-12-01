using System.Threading.Tasks;
using Quickr.Models.Keys;
using Quickr.ViewModels.Editors;
using StackExchange.Redis;

namespace Quickr.ViewModels.Data
{
    class StringViewModel: BaseValueViewModel
    {
        StringValueViewModel _value;

        public StringValueViewModel Value
        {
            get => _value;
            protected set
            {
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUnsaved));
            }
        }

        public override bool IsUnsaved => 
            Value != null && // loaded
            Value.IsUnsaved;

        public StringViewModel(KeyEntry key): base(key)
        {
            LoadAsync();
        }

        async void LoadAsync()
        {
            var str = Key.Exists ? await Key.GetDatabase().GetString(Key) : RedisValue.EmptyString;
            Value = new StringValueViewModel(str);
            Value.PropertyChanged += ValuePropertyChanged;
        }

        void ValuePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StringValueViewModel.IsUnsaved)) OnPropertyChanged(nameof(IsUnsaved));
        }

        public override async Task<bool> Save()
        {
            await Key.GetDatabase().SetString(Key, Value.CurrentValue);
            Value.OriginalValue = Value.CurrentValue;
            return true;
        }
    }
}
