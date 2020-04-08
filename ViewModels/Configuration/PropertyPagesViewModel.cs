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
        public MemoryManagementPropertyPageModel MemoryManagementPage { get; }
        public LazyFreeingPropertyPageModel LazyFreeingPage { get; }
        public AppendOnlyModePropertyPageModel AppendOnlyModePage { get; }
        public LuaScriptingPropertyPageModel LuaScriptingPage { get; }
        public RedisClusterPropertyPageModel RedisClusterPage { get; }
        public ClusterNatSupportPropertyPageModel ClusterNatSupportPage { get; }
        public ShowLogPropertyPageModel ShowLogPage { get; }
        public LatencyMonitorPropertyPageModel LatencyMonitorPage { get; }
        public EventNotificationPropertyPageModel EventNotificationPage { get; }
        public AdvancedConfigPropertyPageModel AdvancedConfigPage { get; }
        public ActiveDefragmentationPropertyPageModel ActiveDefragmentationPage { get; }

        public PropertyPagesViewModel(RedisConnection connection)
        {
            var config = connection.ConfigGet();
            NetworkPage = new NetworkPropertyPageModel(connection, config["Network"]);
            GeneralPage = new GeneralPropertyPageModel(connection, config["General"]);
            SnapshottingPage = new SnapshottingPropertyPageModel(connection, config["Snapshotting"]);
            ReplicationPage = new ReplicationPropertyPageModel(connection, config["Replication"]);
            SecurityPage = new SecurityPropertyPageModel(connection, config["Security"]);
            ClientsPage = new ClientsPropertyPageModel(connection, config["Clients"]);
            MemoryManagementPage = new MemoryManagementPropertyPageModel(connection, config["Memory management"]);
            LazyFreeingPage = new LazyFreeingPropertyPageModel(connection, config["Lazy freeing"]);
            AppendOnlyModePage = new AppendOnlyModePropertyPageModel(connection, config["Append only mode"]);
            LuaScriptingPage = new LuaScriptingPropertyPageModel(connection, config["Lua scripting"]);
            RedisClusterPage = new RedisClusterPropertyPageModel(connection, config["Cluster"]);
            ClusterNatSupportPage = new ClusterNatSupportPropertyPageModel(connection, config["Cluster NAT support"]);
            ShowLogPage = new ShowLogPropertyPageModel(connection, config["Show log"]);
            LatencyMonitorPage = new LatencyMonitorPropertyPageModel(connection, config["Latency monitor"]);
            EventNotificationPage = new EventNotificationPropertyPageModel(connection, config["Event notification"]);
            AdvancedConfigPage = new AdvancedConfigPropertyPageModel(connection, config["Advanced config"]);
            ActiveDefragmentationPage = new ActiveDefragmentationPropertyPageModel(connection, config["Active defragmentation"]);
            SaveCommand = new Command(Save);
        }

        private void Save()
        {
            NetworkPage.Save();
            GeneralPage.Save();
            SnapshottingPage.Save();
            ReplicationPage.Save();
            ClientsPage.Save();
            MemoryManagementPage.Save();
            LazyFreeingPage.Save();
            AppendOnlyModePage.Save();
            LuaScriptingPage.Save();
            RedisClusterPage.Save();
            ClusterNatSupportPage.Save();
            ShowLogPage.Save();
            LatencyMonitorPage.Save();
            EventNotificationPage.Save();
            AdvancedConfigPage.Save();
            ActiveDefragmentationPage.Save();

            // always last to save password and don't lose connection
            SecurityPage.Save();
        }
    }
}