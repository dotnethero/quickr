using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Utils;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal enum KeyType
    {
        String,
        List,
        Set,
        SortedSet,
        HashSet,
    }

    internal class CreateKeyViewModel: BaseViewModel
    {
        private readonly FolderEntry _folder;
        private readonly KeyViewModelFactory _keyFactory;

        private int _tab;
        private KeyType _type;
        private BaseKeyViewModel _keyViewModel;

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

        public BaseKeyViewModel Key
        {
            get => _keyViewModel;
            set
            {
                if (_keyViewModel != null) _folder.RemoveChild(_keyViewModel.Key);
                _keyViewModel = value;
                OnPropertyChanged();
            }
        }

        public PropertiesViewModel Properties { get; }

        public CreateKeyViewModel(FolderEntry folder, KeyViewModelFactory keyFactory)
        {
            _folder = folder;
            _keyFactory = keyFactory;

            Properties = new PropertiesViewModel("", null);

            PrevCommand = new Command(() => Tab--);
            NextCommand = new Command(() => Tab++);
        }

        public Task<bool> Save()
        {
            Key.Key.IsSelected = true;
            return Key.Save();
        }

        public void Cancel()
        {
            if (_keyViewModel != null)
            {
                _folder.RemoveChild(_keyViewModel.Key);
            }
        }

        private void OnTabChanged(int prev, int tab)
        {
            if (prev == 0 && tab == 1)
            {
                CreateModel();
            }
        }

        private void CreateModel()
        {
            var requiredStart = _folder.IsRoot ? "" : _folder.FullName + Constants.RegionSeparator;
            var fullname = requiredStart + Properties.Name;
            var entry = _folder.AddChild(fullname);
            Key = _keyFactory.Create(entry, Type, Properties.Expiration);
        }
    }
}