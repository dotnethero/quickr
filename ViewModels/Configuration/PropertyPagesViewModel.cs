using System.Windows.Input;
using Quickr.Models.Keys;
using Quickr.Utils;

namespace Quickr.ViewModels.Configuration
{
    class PropertyPagesViewModel
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
        public SlowLogPropertyPageModel SlowLogPage { get; }
        public LatencyMonitorPropertyPageModel LatencyMonitorPage { get; }
        public EventNotificationPropertyPageModel EventNotificationPage { get; }
        public AdvancedConfigPropertyPageModel AdvancedConfigPage { get; }
        public ActiveDefragmentationPropertyPageModel ActiveDefragmentationPage { get; }

        public PropertyPagesViewModel(EndpointEntry endpoint)
        {
            var connection = endpoint.Connection;
            var config = connection.ConfigGet(endpoint.Endpoint);
            NetworkPage = new NetworkPropertyPageModel(endpoint, config["Network"]);
            GeneralPage = new GeneralPropertyPageModel(endpoint, config["General"]);
            SnapshottingPage = new SnapshottingPropertyPageModel(endpoint, config["Snapshotting"]);
            ReplicationPage = new ReplicationPropertyPageModel(endpoint, config["Replication"]);
            SecurityPage = new SecurityPropertyPageModel(endpoint, config["Security"]);
            ClientsPage = new ClientsPropertyPageModel(endpoint, config["Clients"]);
            MemoryManagementPage = new MemoryManagementPropertyPageModel(endpoint, config["Memory management"]);
            LazyFreeingPage = new LazyFreeingPropertyPageModel(endpoint, config["Lazy freeing"]);
            AppendOnlyModePage = new AppendOnlyModePropertyPageModel(endpoint, config["Append only mode"]);
            LuaScriptingPage = new LuaScriptingPropertyPageModel(endpoint, config["Lua scripting"]);
            RedisClusterPage = new RedisClusterPropertyPageModel(endpoint, config["Cluster"]);
            ClusterNatSupportPage = new ClusterNatSupportPropertyPageModel(endpoint, config["Cluster NAT support"]);
            SlowLogPage = new SlowLogPropertyPageModel(endpoint, config["Slow log"]);
            LatencyMonitorPage = new LatencyMonitorPropertyPageModel(endpoint, config["Latency monitor"]);
            EventNotificationPage = new EventNotificationPropertyPageModel(endpoint, config["Event notification"]);
            AdvancedConfigPage = new AdvancedConfigPropertyPageModel(endpoint, config["Advanced config"]);
            ActiveDefragmentationPage = new ActiveDefragmentationPropertyPageModel(endpoint, config["Active defragmentation"]);
            SaveCommand = new Command(Save);
        }

        void Save()
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
            SlowLogPage.Save();
            LatencyMonitorPage.Save();
            EventNotificationPage.Save();
            AdvancedConfigPage.Save();
            ActiveDefragmentationPage.Save();

            // always last to save password and don't lose connection
            SecurityPage.Save();
        }
    }
}