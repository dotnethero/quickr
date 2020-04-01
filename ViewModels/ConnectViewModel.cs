using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Quickr.Models;
using Quickr.Properties;
using Quickr.Services;
using Quickr.Utils;

namespace Quickr.ViewModels
{
    internal class ConnectionResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public ConnectionResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }

    internal class ConnectViewModel: BaseViewModel
    {
        private readonly RedisMultiplexer _multiplexer;
        private EndPointModel _current;
        
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<EndPointModel> Endpoints { get; }

        public EndPointModel Current
        {
            get => _current;
            set
            {
                if (Equals(value, _current)) return;
                _current = value;
                OnPropertyChanged();
            }
        }

        public ConnectViewModel(RedisMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;

            Endpoints = new ObservableCollection<EndPointModel>(Settings.Current.Endpoints);
            EnsureAtLeastOneItemPresent();

            // commands
            AddCommand = new Command(Add);
            DeleteCommand = new Command(DeleteCurrent);
        }

        public void Add()
        {
            var model = new EndPointModel();
            Endpoints.Add(model);
            Current = model;
        }

        public void DeleteCurrent()
        {
            Endpoints.Remove(Current);
            EnsureAtLeastOneItemPresent();
        }
        
        public void SaveChanges()
        {
            Settings.Current.Endpoints = Endpoints.ToList();
            Settings.Current.Save();
        }

        public ConnectionResult EnsureConnectionIsValid()
        {
            if (string.IsNullOrEmpty(Current.Host))
            {
                return new ConnectionResult(false, "Host can not be empty!");
            }
            try
            {
                using var server = _multiplexer.Connect(Current).Connection;
                return new ConnectionResult(true, "Connection succeeded!");
            }
            catch (Exception ex)
            {
                return new ConnectionResult(false, ex.Message);
            }
        }

        private void EnsureAtLeastOneItemPresent()
        {
            if (Endpoints.Count == 0)
            {
                Endpoints.Add(new EndPointModel());
            }
            if (Current == null)
            {
                Current = Endpoints.First();
            }
        }
        
    }
}