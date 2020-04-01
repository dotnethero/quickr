using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Quickr.Models;
using Quickr.Properties;

namespace Quickr.ViewModels
{
    internal class ConnectViewModel: BaseViewModel
    {
        private EndPointModel _current;

        public Window Window { get; set; }
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

        public ConnectViewModel()
        {
            Endpoints = new ObservableCollection<EndPointModel>(Settings.Current.Endpoints);
            EnsureAtLeastOneItemPresent();
        }
        
        public void Add()
        {
            var model = new EndPointModel();
            Endpoints.Add(model);
            Current = model;
        }

        public void RemoveCurrent()
        {
            Endpoints.Remove(Current);
            EnsureAtLeastOneItemPresent();
        }
        
        public void SaveChanges()
        {
            Settings.Current.Endpoints = Endpoints.ToList();
            Settings.Current.Save();
        }

        public bool TestConnection()
        {
            if (string.IsNullOrEmpty(Current.Server))
            {
                MessageBox.Show(
                    Window,
                    "Connection property \"Server\" can not be empty!", 
                    "Test connection", 
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
            return true;
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