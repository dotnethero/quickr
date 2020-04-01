using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Quickr.Models;
using Quickr.Models.Keys;
using Quickr.Properties;
using Quickr.Services;
using Quickr.Utils;

namespace Quickr.ViewModels
{
    internal class ConnectionResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public ConnectionResult(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
        }
    }

    internal class ConnectViewModel: BaseViewModel
    {
        private readonly RedisMultiplexer _multiplexer;
        private EndPointModel _current;
        private bool _isReady;

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ServerEntry Server { get; private set; }
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

        public bool IsReady
        {
            get => _isReady;
            set
            {
                if (value == _isReady) return;
                _isReady = value;
                OnPropertyChanged();
            }
        }

        public ConnectViewModel(RedisMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;

            Endpoints = new ObservableCollection<EndPointModel>(Settings.Current.Endpoints);
            EnsureAtLeastOneItemPresent();
            IsReady = true;

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

        public async Task<ConnectionResult> EnsureConnectionIsValid(bool test, CancellationToken token)
        {
            if (string.IsNullOrEmpty(Current.Host))
            {
                return new ConnectionResult(false, "Host can not be empty!");
            }

            try
            {
                IsReady = false;
                var task = _multiplexer.ConnectAsync(Current);
                var index = Task.WaitAny(Task.Delay(5000, token), task);
                if (index == 0)
                {
                    throw new TimeoutException("Connection timeout!");
                }
                if (!test)
                {
                    Server = await task.ConfigureAwait(false);
                }
                return new ConnectionResult(true, "Connection succeeded!");
            }
            catch (Exception ex)
            {
                return new ConnectionResult(false, ex.Message);
            }
            finally
            {
                IsReady = true;
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