using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Quickr.Views.Configuration
{
    public partial class EventNotificationPage : UserControl
    {
        public EventNotificationPage()
        {
            InitializeComponent();
        }

        private void Navigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }
    }
}
