using System.Windows;
using Quickr.Models;

namespace Quickr.Views
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public EndPointModel ViewModel { get; set; }

        public ConnectWindow(EndPointModel model, Window owner)
        {
            InitializeComponent();
            DataContext = ViewModel = model;
            PasswordTextBox.Password = ViewModel.Password;
            Owner = owner;
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            // check connection
            DialogResult = true;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordTextBox.Password;
        }
    }
}
