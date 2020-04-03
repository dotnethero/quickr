using System.Collections.Generic;
using System.Linq;
using Quickr.Services;

namespace Quickr.ViewModels.Configuration
{
    internal class PropertyPagesViewModel
    {
        public List<string> Sections { get; }
        public NetworkPropertyPageModel NetworkPage { get; }

        public PropertyPagesViewModel(RedisConnection connection)
        {
            var config = connection.ConfigGet();
            var network = config["Network"];
            NetworkPage = new NetworkPropertyPageModel(connection, network);
            Sections = config.Select(x => x.Key).ToList();
        }
    }
}