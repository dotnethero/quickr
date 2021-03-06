﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Quickr.Models;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.Utils;
using Quickr.ViewModels.Configuration;
using Quickr.ViewModels.Connection;
using Quickr.ViewModels.Data;
using Quickr.ViewModels.Server;
using Quickr.Views;
using Quickr.Views.Configuration;
using Quickr.Views.Data;

namespace Quickr.ViewModels
{
    internal class MainWindowViewModel: BaseViewModel
    {
        private readonly RedisMultiplexer _multiplexer;
        private readonly KeyViewModelFactory _keyFactory;

        private object _current;

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand PropertiesCommand { get; }
        public ICommand CreateKeyCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CloneCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand MarkAsExpiredCommand { get; set; }
        
        public MainWindow Window { get; set; }
        public ObservableCollection<ServerEntry> Servers { get; } = new ObservableCollection<ServerEntry>();

        public object Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged();
            }
        }

        public bool IsUnsaved => Current is BaseKeyViewModel key && key.IsUnsaved;
        public bool IsKeyRemoved => Current is BaseKeyViewModel key && key.IsKeyRemoved;

        public MainWindowViewModel(RedisMultiplexer multiplexer, KeyViewModelFactory keyFactory)
        {
            _multiplexer = multiplexer;
            _keyFactory = keyFactory;

            // commands
            ConnectCommand = new Command(Connect);
            DisconnectCommand = new ParameterCommand(Disconnect);
            PropertiesCommand = new ParameterCommand(ShowProperties);
            CreateKeyCommand = new ParameterCommand(CreateKey);
            SelectCommand = new ParameterCommand(async item => await Select(item));
            RefreshCommand = new ParameterCommand(Refresh);
            CloneCommand = new ParameterCommand(Clone);
            DeleteCommand = new ParameterCommand(Delete);
            MarkAsExpiredCommand = new ParameterCommand(MarkAsExpired);
        }

        public async Task<bool> Save()
        {
            if (Current is BaseKeyViewModel key) return await key.Save();
            return false;
        }

        private void Connect()
        {
            var model = new ConnectViewModel(_multiplexer);
            var window = new ConnectWindow(model) { Owner = Window };
            if (window.ShowDialog() == true && model.Server != null)
            {
                var server = model.Server;
                foreach (var database in server.Databases)
                {
                    database.Refresh(); // NOTE: load database async
                }
                Servers.Add(server);
            }
        }

        public async Task ConnectToTest()
        {
            var model = new EndpointModel
            {
                Name = "test",
                Host = "localhost",
                Port = 6379
            };
            await ConnectToEndpoint(model);
        }

        private async Task ConnectToEndpoint(EndpointModel endpoint)
        {
            var server = await _multiplexer.ConnectAsync(endpoint);
            foreach (var database in server.Databases)
            {
                database.Refresh(); // NOTE: load database async
            }
            Servers.Add(server);
        }

        private void Disconnect(object obj)
        {
            if (obj is ServerEntry server)
            {
                server.Connection.Dispose();
                Servers.Remove(server);
            }
        }
        
        private void ShowProperties(object obj)
        {
            if (obj is EndpointEntry endpoint)
            {
                var model = new PropertyPagesViewModel(endpoint);
                var window = new PropertyPagesWindow(model) { Owner = Window };
                if (window.ShowDialog() == true)
                {
                    // TODO:
                }
            }
        }

        private async void Clone(object item)
        {
            switch (item)
            {
                case KeyEntry key:
                    var fullname = await key.CloneAsync();
                    var entry = key.Parent.AddChild(fullname);
                    entry.IsSelected = true;
                    break;
            }
        }

        private async void Delete(object item)
        {
            switch (item)
            {
                case DatabaseEntry database:
                    if (FlushDatabaseMessage(database) == MessageBoxResult.Yes)
                    {
                        await database.Flush();
                    }
                    break;

                case FolderEntry folder:
                    if (DeleteFolderMessage(folder) == MessageBoxResult.Yes)
                    {
                        await folder.Delete();
                    }

                    break;

                case KeyEntry key:
                    await key.Delete();
                    break;
            }
        }

        private async void MarkAsExpired(object item)
        {
            switch (item)
            {
                case DatabaseEntry database:
                    if (MarkDatabaseAsExpired(database) == MessageBoxResult.Yes)
                    {
                        await database.MarkChildrenAsExpired();
                    }
                    break;

                case FolderEntry folder:
                    if (MarkFolderAsExpired(folder) == MessageBoxResult.Yes)
                    {
                        await folder.MarkAsExpired();
                    }
                    break;

                case KeyEntry key:
                    await key.MarkAsExpired();
                    break;
            }
        }
        
        private MessageBoxResult FlushDatabaseMessage(DatabaseEntry database)
        {
            return MessageBox.Show(
                Window, 
                $"This would remove all existing keys in [{database.Name}] database.\nDo you want to proceed?",
                $"Flush database", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);
        }
        
        private MessageBoxResult MarkDatabaseAsExpired(DatabaseEntry database)
        {
            return MessageBox.Show(
                Window,
                $"This would remove all existing keys in [{database.Name}] database.\nDo you want to proceed?",
                $"Mark database as expired",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
        }

        private MessageBoxResult DeleteFolderMessage(FolderEntry folder)
        {
            return MessageBox.Show(
                Window,
                $"This would remove all keys matching pattern \"{folder.FullName + Constants.RegionSeparator}*\"\nDo you want to proceed?",
                $"Delete folder",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
        }
        
        private MessageBoxResult MarkFolderAsExpired(FolderEntry folder)
        {
            return MessageBox.Show(
                Window,
                $"This would remove all keys matching pattern \"{folder.FullName + Constants.RegionSeparator}*\"\nDo you want to proceed?",
                $"Mark folder as expired",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
        }

        private async void Refresh(object item)
        {
            if (item is ServerEntry server)
            {
                server.Refresh();
            }
            if (item is FolderEntry folder)
            {
                await folder.Refresh();
            }
        }

        public async Task Select(object item)
        {
            switch (item)
            {
                case KeyEntry key:
                    Current = await _keyFactory.Create(key);
                    break;

                case DatabaseEntry db:
                    Current = new DatabaseViewModel(db);
                    break;
                    
                case InfoEntry entry:
                    Current = new InfoViewModel(entry.Connection, entry.Endpoint);
                    break;
                    
                case ClientsEntry entry:
                    Current = new ClientsViewModel(entry.Connection, entry.Endpoint);
                    break;
                    
                case SlowlogEntry entry:
                    Current = new SlowlogViewModel(entry.Connection, entry.Endpoint);
                    break;
                    
                case MemoryDoctorEntry entry:
                    Current = new MemoryDoctorViewModel(entry.Connection, entry.Endpoint);
                    break;

                case LatencyDoctorEntry entry:
                    Current = new LatencyDoctorViewModel(entry.Connection, entry.Endpoint);
                    break;

                default:
                    Current = null;
                    break;
            }
        }
        
        private async void CreateKey(object obj)
        {
            if (!(obj is FolderEntry folder)) return;
            var window = new CreateKeyWindow();
            var model = new CreateKeyModel();
            window.DataContext = model;
            window.Owner = Window;
            if (window.ShowDialog() == true)
            {
                var connection = folder.GetDatabase();
                var requiredStart = folder.IsRoot ? "" : folder.FullName + Constants.RegionSeparator;
                var fullname = requiredStart + model.Name;
                var entry = folder.AddChild(fullname);
                entry.IsSelected = true;
                switch (model.Type)
                {
                    case KeyType.String:
                        await connection.SetString(entry, string.Empty);
                        break;
                    case KeyType.List:
                        connection.ListRightPush(entry, string.Empty);
                        break;
                    case KeyType.Set:
                        connection.UnsortedSetAdd(entry, string.Empty);
                        break;
                    case KeyType.SortedSet:
                        connection.SortedSetAdd(entry, string.Empty, 0);
                        break;
                    case KeyType.HashSet:
                        connection.HashSet(entry, string.Empty, string.Empty);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
