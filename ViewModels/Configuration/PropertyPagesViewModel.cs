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

        public PropertyPagesViewModel(RedisConnection connection)
        {
            var config = connection.ConfigGet();
            NetworkPage = new NetworkPropertyPageModel(connection, config["Network"]);
            GeneralPage = new GeneralPropertyPageModel(connection, config["General"]);
            SaveCommand = new Command(Save);
        }

        private void Save()
        {
            NetworkPage.Save();
            GeneralPage.Save();
        }
    }
}