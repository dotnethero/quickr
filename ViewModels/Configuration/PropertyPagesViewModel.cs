using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class PropertyPagesViewModel
    {
        public NetworkPropertyPageModel NetworkPage { get; }
        public GeneralPropertyPageModel GeneralPage { get; }

        public PropertyPagesViewModel(RedisConnection connection)
        {
            var config = connection.ConfigGet();
            NetworkPage = new NetworkPropertyPageModel(connection, config["Network"]);
            GeneralPage = new GeneralPropertyPageModel(connection, config["General"]);
        }
    }
}