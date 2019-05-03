using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Quickr.Annotations;
using Quickr.Models;
using Quickr.Services;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class MainWindowViewModel: INotifyPropertyChanged
    {
        private readonly RedisProxy _proxy;

        public ICommand ConnectCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }

        public DatabaseEntry[] Databases { get; private set; }
        public object Current { get; private set; }

        public MainWindowViewModel()
        {
            // use DI later
            _proxy = new RedisProxy();

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
            if (item is KeyEntry key)
            {
                var type = _proxy.GetType(key);
                var ttl = _proxy.GetTimeToLive(key);
                var vm = new KeyViewModel();
                switch (type)
                {
                    case RedisType.Hash:
                        vm.Table = _proxy.GetHashes(key);
                        break;

                    case RedisType.List:
                        vm.Table = _proxy.GetList(key);
                        break;

                    case RedisType.Set:
                        vm.Table = _proxy.GetUnsortedSet(key);
                        break;

                    case RedisType.SortedSet:
                        vm.Table = _proxy.GetSortedSet(key);
                        break;

                    case RedisType.String:
                        vm.Table = null;
                        vm.Value = _proxy.GetString(key);
                        break;
                }

                vm.Name = key.FullName;
                vm.Expiration = ttl.ToString();
                Current = vm;
                OnPropertyChanged(nameof(Current));
            }
            else if (item is DatabaseEntry db)
            {
                var size = _proxy.GetSize(db);
                var vm = new DatabaseViewModel();
                vm.KeyCount = size;
                Current = vm;
                OnPropertyChanged(nameof(Current));
            }
            else
            {
                Current = null;
                OnPropertyChanged(nameof(Current));
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
