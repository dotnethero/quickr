using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Quickr.Annotations;
using Quickr.Models;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;
using Quickr.ViewModels.Data;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class MainWindowViewModel: INotifyPropertyChanged
    {
        private readonly RedisProxy _proxy;
        private object _current;

        public ICommand ConnectCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }

        public DatabaseEntry[] Databases { get; private set; }

        public object Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(RedisProxy proxy)
        {
            _proxy = proxy;

            // commands
            ConnectCommand = new ParameterCommand(Connect);
            SelectCommand = new ParameterCommand(Select);
            RefreshCommand = new ParameterCommand(Refresh);
            DeleteCommand = new ParameterCommand(Delete);
        }

        private void Delete(object item)
        {
            switch (item)
            {
                case DatabaseEntry database:
                    _proxy.Flush(database);
                    database.RemoveChildren();
                    return;

                case FolderEntry folder:
                    var folderParent = folder.Parent;
                    _proxy.Delete(folder);
                    folderParent.RemoveChild(folder);
                    break;

                case KeyEntry key:
                    var keyParent = key.Parent;
                    _proxy.Delete(key);
                    keyParent.RemoveChild(key);
                    break;
            }
        }

        private void Refresh(object item)
        {
            if (item is FolderEntry folder)
            {
                var keys =_proxy.GetKeys(folder.DbIndex, folder.SearchPattern);
                folder.UpdateChildren(keys);
            }
        }

        private void Select(object item)
        {
            switch (item)
            {
                case KeyEntry key:
                    Current = GetKeyViewModel(key);
                    break;

                case DatabaseEntry db:
                    var size = _proxy.GetSize(db);
                    var vm = new DatabaseViewModel();
                    vm.KeyCount = size;
                    Current = vm;
                    break;

                default:
                    Current = null;
                    break;
            }
        }

        private object GetKeyViewModel(KeyEntry key)
        {
            var type = _proxy.GetType(key);
            switch (type)
            {
                case RedisType.String:
                    return new StringViewModel(_proxy, key);

                case RedisType.Set:
                    return new UnsortedSetViewModel(_proxy, key);

                case RedisType.Hash:
                    return new HashSetViewModel(_proxy, key);

                case RedisType.List:
                    return new ListViewModel(_proxy, key);
                    break;

                case RedisType.SortedSet:
                    return new SortedSetViewModel(_proxy, key);
                    break;

                default:
                    return null;
            }
        }

        private void Connect(object model)
        {
            if (model is EndPointModel endPoint)
            {
                _proxy.ChangeConnection(endPoint);
                Databases = _proxy.GetDatabases();
                foreach (var database in Databases)
                {
                    database.UpdateChildren(_proxy.GetKeys(database.DbIndex));
                }
                OnPropertyChanged(nameof(Databases));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
