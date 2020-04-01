using System.ComponentModel;
using System.Windows;
using Quickr.ViewModels;

namespace Quickr.Views
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        private readonly ConnectViewModel _viewModel;

        internal ConnectWindow(ConnectViewModel viewModel)
        {
            InitializeComponent();
            DataContext = _viewModel = viewModel;
            _viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConnectViewModel.Current) && _viewModel.Current != null)
            {
                PasswordTextBox.Password = _viewModel.Current.Password;
            }
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            if (_viewModel.TestConnection())
            {
                _viewModel.SaveChanges();
                DialogResult = true;
            }
        }
        
        private void OnSaveChanges(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveChanges();
            DialogResult = false;
        }
        
        private void OnAdd(object sender, RoutedEventArgs e)
        {
            _viewModel.Add();
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            _viewModel.RemoveCurrent();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Current.Password = PasswordTextBox.Password;
        }

        private void OnTestConnection(object sender, RoutedEventArgs e)
        {
            _viewModel.TestConnection();
        }
    }
}
