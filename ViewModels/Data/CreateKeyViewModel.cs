using System.Threading.Tasks;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Utils;

namespace Quickr.ViewModels.Data
{
    enum KeyType
    {
        String,
        List,
        Set,
        SortedSet,
        HashSet,
    }

    class CreateKeyViewModel: BaseViewModel
    {
        readonly FolderEntry _folder;
        readonly KeyViewModelFactory _keyFactory;
        readonly KeyEntry _keyEntry;

        int _tab;
        KeyType _type;
        BaseValueViewModel _value;

        public ICommand PrevCommand { get; }
        public ICommand NextCommand { get; }

        public int Tab
        {
            get => _tab;
            set
            {
                if (value == _tab) return;
                var prev = _tab;
                _tab = value;
                OnTabChanged(prev, _tab);
                OnPropertyChanged();
            }
        }

        public KeyType Type
        {
            get => _type;
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public BaseValueViewModel Value
        {
            get => _value;
            set
            {
                if (_value != null && !_value.Key.Exists) _folder.RemoveChild(_value.Key);
                _value = value;
                OnPropertyChanged();
            }
        }

        public PropertiesViewModel Properties { get; }

        public CreateKeyViewModel(FolderEntry folder, KeyViewModelFactory keyFactory)
        {
            _folder = folder;
            _keyFactory = keyFactory;
            _keyEntry = CreateModel(folder);

            Properties = new PropertiesViewModel(_keyEntry, null);

            PrevCommand = new Command(() => Tab--);
            NextCommand = new Command(() => Tab++);
        }


        public Task<bool> Save()
        {
            Value.Key.IsSelected = true;
            return Value.Save();
        }

        public void Cancel()
        {
            if (_value != null && !_value.Key.Exists) _folder.RemoveChild(_value.Key);
        }

        void OnTabChanged(int prev, int tab)
        {
            if (prev == 0 && tab == 1)
            {
                Value = _keyFactory.CreateValueViewModel(_keyEntry, Type);
            }
        }

        static KeyEntry CreateModel(FolderEntry folder)
        {
            var requiredStart = folder.IsRoot ? "" : folder.FullName + Constants.RegionSeparator;
            var fullname = requiredStart + "";
            return folder.AddChild(fullname, false);
        }
    }
}