using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Quickr.Annotations;
using Quickr.Utils;
using Quickr.ViewModels.Database;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class MainWindowViewModel: INotifyPropertyChanged
    {
        public Command Connect { get; }

        public DatabaseViewModel[] Databases { get; private set; }

        public MainWindowViewModel()
        {
            Connect = new Command(ConnectInternal);
        }

        private void ConnectInternal()
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

        private RedisKey[] GetKeys(IConnectionMultiplexer connection, int dbIndex)
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
