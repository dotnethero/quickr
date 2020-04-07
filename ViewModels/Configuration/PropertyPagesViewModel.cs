using System.Windows.Input;
using Quickr.Services;
using Quickr.Utils;

namespace Quickr.ViewModels.Configuration
{
    internal class PropertyPagesViewModel
    {
        public ICommand SaveCommand { get; }

        public NetworkPropertyPageModel NetworkPage { get; }
        public GeneralPropertyPageModel GeneralPage { get; }
        public SnapshottingPropertyPageModel SnapshottingPage { get; }
        public ReplicationPropertyPageModel ReplicationPage { get; }
        public SecurityPropertyPageModel SecurityPage { get; }
        public ClientsPropertyPageModel ClientsPage { get; }

        public PropertyPagesViewModel(RedisConnection connection)
        {
            var config = connection.ConfigGet();
            NetworkPage = new NetworkPropertyPageModel(connection, config["Network"]);
            GeneralPage = new GeneralPropertyPageModel(connection, config["General"]);
            SnapshottingPage = new SnapshottingPropertyPageModel(connection, config["Snapshotting"]);
            ReplicationPage = new ReplicationPropertyPageModel(connection, config["Replication"]);
            SecurityPage = new SecurityPropertyPageModel(connection, config["Security"]);
            ClientsPage = new ClientsPropertyPageModel(connection, config["Clients"]);
            SaveCommand = new Command(Save);
        }

        private void Save()
        {
            NetworkPage.Save();
            GeneralPage.Save();
            SnapshottingPage.Save();
            ReplicationPage.Save();
            ClientsPage.Save();

            // always last to save password and don't lose connection
            SecurityPage.Save();
        }
    }
}