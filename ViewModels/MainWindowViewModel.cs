﻿using System.ComponentModel;
using System.Net;
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
        public KeyViewModel Current { get; private set; }

        public MainWindowViewModel()
        {
            // use DI later
            _proxy = new RedisProxy();

            // commands
            ConnectCommand = new Command(Connect);
            SelectCommand = new ParameterCommand(Select);
            RefreshCommand = new ParameterCommand(Refresh);
            DeleteCommand = new ParameterCommand(Delete);
        }

        private void Delete(object item)
        {
            if (item is KeyEntry key)
            {
                var parent = key.Parent;
                _proxy.Delete(key);
                parent.RemoveChild(key);
            }

            if (item is FolderEntry folder)
            {
                var parent = folder.Parent;
                _proxy.Delete(folder);
                parent.RemoveChild(folder);
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
                        vm.DataSet = _proxy.GetHashes(key);
                        break;

                    case RedisType.String:
                        vm.Value = _proxy.GetString(key);
                        break;
                }

                vm.Name = key.FullName;
                vm.Expiration = ttl.ToString();
                Current = vm;
                OnPropertyChanged(nameof(Current));
            }
            else
            {
                Current = null;
                OnPropertyChanged(nameof(Current));
            }
        }

        private void Connect()
        {
            var conn = new ConnectWindow();
            conn.DataContext = new object();
            conn.ShowDialog();

            _proxy.ChangeConnection(new DnsEndPoint("localhost", 6379));
            Databases = _proxy.GetDatabases();
            foreach (var database in Databases)
            {
                database.UpdateChildren(_proxy.GetKeys(database.DbIndex));
            }
            OnPropertyChanged(nameof(Databases));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
