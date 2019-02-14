using System.ComponentModel;
using System.Linq;
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

        public DatabaseEntry[] Databases { get; private set; }
        public HashEntry[] DataSet { get; private set; }
        public string CurrentValue { get; private set; }

        public MainWindowViewModel()
        {
            // use DI later
            _proxy = new RedisProxy();

            // commands
            ConnectCommand = new Command(Connect);
            SelectCommand = new ParameterCommand(Select);
        }

        private void Select(object item)
        {
            if (item is KeyEntry key)
            {
                var type = _proxy.GetType(key);
                switch (type)
                {
                    case RedisType.Hash:
                        DataSet = _proxy.GetHashes(key);
                        CurrentValue = GetValueFromHash(DataSet);
                        break;

                    case RedisType.String:
                        DataSet = null;
                        CurrentValue = _proxy.GetString(key);
                        break;
                }

                OnPropertyChanged(nameof(DataSet));
                OnPropertyChanged(nameof(CurrentValue));
            }
        }

        private void Connect()
        {
            Databases = _proxy.GetDatabases();
            OnPropertyChanged(nameof(Databases));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetValueFromHash(HashEntry[] dataSet)
        {
            var val = dataSet?.FirstOrDefault(x => x.Name == "value");
            return val.HasValue && val != default(HashEntry) ? val.Value.Value.PrettifyJson() : null;
        }
    }
}
