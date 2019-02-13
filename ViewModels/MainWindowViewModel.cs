using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json;
using Quickr.Annotations;
using Quickr.Utils;
using Quickr.ViewModels.Database;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class MainWindowViewModel: INotifyPropertyChanged
    {
        public ICommand ConnectCommand { get; }
        public ICommand SelectCommand { get; }

        public DatabaseViewModel[] Databases { get; private set; }
        public HashEntry[] CurrentHashes { get; set; }
        public string CurrentValue { get; set; }

        public MainWindowViewModel()
        {
            ConnectCommand = new Command(Connect);
            SelectCommand = new ParameterCommand(Select);
        }

        private void Select(object item)
        {
            if (item is KeyViewModel key)
            {
                CurrentHashes = key.GetHashes();
                OnPropertyChanged(nameof(CurrentHashes));

                var val = CurrentHashes.FirstOrDefault(x => x.Name == "value");
                if (val != default(HashEntry))
                {
                    dynamic obj = JsonConvert.DeserializeObject(val.Value);
                    CurrentValue = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    OnPropertyChanged(nameof(CurrentValue));
                }
            }
        }

        private void Connect()
        {
            var connection = RedisMultiplexer.Connect();

            var count = connection
                .GetServer()
                .ConfigGet("databases")
                .FirstOrDefault()
                .Value
                .ToInt32();

            Databases = Enumerable
                .Range(0, count)
                .Select(x => new DatabaseViewModel(x, GetKeys(connection, x)))
                .ToArray();

            OnPropertyChanged(nameof(Databases));
        }

        private static RedisKey[] GetKeys(IConnectionMultiplexer connection, int dbIndex)
        {
            return connection
                .GetServer()
                .Keys(dbIndex)
                .ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
