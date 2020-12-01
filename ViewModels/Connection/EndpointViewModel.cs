using Quickr.Models;

namespace Quickr.ViewModels.Connection
{
    class EndpointViewModel : BaseViewModel
    {
        readonly EndpointModel _original;
        string _name;
        string _host;
        int? _port;
        string _password;
        bool _useSsl;

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public string Host
        {
            get => _host;
            set
            {
                if (value == _host) return;
                _host = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public int? Port
        {
            get => _port;
            set
            {
                if (value == _port) return;
                _port = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public bool UseSsl
        {
            get => _useSsl;
            set
            {
                if (value == _useSsl) return;
                _useSsl = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsValueChanged));
            }
        }

        public bool IsValueChanged =>
            Name != _original.Name ||
            Host != _original.Host ||
            Port != _original.Port ||
            Password != _original.Password ||
            UseSsl != _original.UseSsl;

        public EndpointViewModel()
        {
            _original = new EndpointModel();
        }

        public EndpointViewModel(EndpointModel original)
        {
            _original = original;

            Name = original.Name;
            Host = original.Host;
            Port = original.Port;
            Password = original.Password;
            UseSsl = original.UseSsl;
        }

        public EndpointModel GetTempModel()
        {
            return new EndpointModel
            {
                Name = Name,
                Host = Host,
                Port = Port,
                Password = Password,
                UseSsl = UseSsl
            };
        }

        public EndpointModel ApplyChanges()
        {
            _original.Name = Name;
            _original.Host = Host;
            _original.Port = Port;
            _original.Password = Password;
            _original.UseSsl = UseSsl;

            OnPropertyChanged(nameof(IsValueChanged));

            return _original;
        }
    }
}