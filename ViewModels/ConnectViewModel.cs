using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Quickr.Models;

namespace Quickr.ViewModels
{
    internal class ConnectViewModel: BaseViewModel
    {
        private EndPointModel _current;

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

        public ConnectViewModel(IEnumerable<EndPointModel> endpoints)
        {
            Endpoints = new ObservableCollection<EndPointModel>(endpoints);
            Endpoints.Add(new EndPointModel { IsNew = true });
            Current = Endpoints.First();
        }
    }
}