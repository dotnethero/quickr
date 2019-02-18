using System.Windows;

namespace Quickr.Views
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            // check connection
            DialogResult = true;
        }
    }
}
