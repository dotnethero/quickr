using System;
using System.Linq;
using System.Windows.Input;
using Quickr.Models;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;

namespace Quickr.ViewModels
{
    internal class MainWindowViewModel: BaseViewModel
    {
        private readonly RedisProxy _proxy;
        private readonly KeyViewModelFactory _kvmFactory;

        private object _current;

        public ICommand ConnectCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CloneCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand MarkAsExpiredCommand { get; set; }

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

        public MainWindowViewModel(RedisProxy proxy, KeyViewModelFactory kvmFactory)
        {
            _proxy = proxy;
            _kvmFactory = kvmFactory;

            // commands
            ConnectCommand = new ParameterCommand(Connect);
            SelectCommand = new ParameterCommand(Select);
            RefreshCommand = new ParameterCommand(Refresh);
            CloneCommand = new ParameterCommand(Clone);
            DeleteCommand = new ParameterCommand(Delete);
            MarkAsExpiredCommand = new ParameterCommand(MarkAsExpired);
        }

        private void Clone(object item)
        {
            switch (item)
            {
                case KeyEntry key:
                    var fullname = _proxy.CloneKey(key);
                    key.Parent.AddChild(fullname);
                    break;
            }
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
        
        private void MarkAsExpired(object item)
        {
            void MarkFolder(FolderEntry folder)
            {
                folder // may affect performance
                    .GetKeys()
                    .ToList()
                    .ForEach(x => _proxy.SetTimeToLive(x, TimeSpan.Zero));
            }

            switch (item)
            {
                case DatabaseEntry database:
                    MarkFolder(database);
                    database.RemoveChildren();
                    return;

                case FolderEntry folder:
                    var folderParent = folder.Parent;
                    MarkFolder(folder);
                    folderParent.RemoveChild(folder);
                    break;

                case KeyEntry key:
                    var keyParent = key.Parent;
                    _proxy.SetTimeToLive(key, TimeSpan.Zero);
                    keyParent.RemoveChild(key);
                    break;
            }
        }

        private void Refresh(object item)
        {
            if (item is FolderEntry folder)
            {
                folder.Refresh();
            }
        }

        private void Select(object item)
        {
            switch (item)
            {
                case KeyEntry key:
                    Current = _kvmFactory.CreateViewModel(key);
                    break;

                case DatabaseEntry db:
                    Current = new DatabaseViewModel(_proxy, db);
                    break;

                default:
                    Current = null;
                    break;
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
                    database.Refresh();
                }
                OnPropertyChanged(nameof(Databases));
            }
        }
    }
}
