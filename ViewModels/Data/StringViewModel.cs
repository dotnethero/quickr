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
            }
        }

        public override bool IsUnsaved => Value.IsValueChanged;

        public StringViewModel(KeyEntry key): base(key)
        {
            LoadAsync();
        }

        async void LoadAsync()
        {
            var str = Key.Exists ? await Key.GetDatabase().GetString(Key) : RedisValue.EmptyString;
            Value = new StringValueViewModel(str);
            Value.ValueSaved += async (sender, e) => await Save();
        }

        public override async Task<bool> Save()
        {
            await Key.GetDatabase().SetString(Key, Value.CurrentValue);
            Value.OriginalValue = Value.CurrentValue;
            return true;
        }
    }
}
