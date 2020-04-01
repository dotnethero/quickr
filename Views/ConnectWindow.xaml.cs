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
        private ConnectViewModel ViewModel { get; }

        internal ConnectWindow(ConnectViewModel viewModel)
        {
            InitializeComponent();
            DataContext = ViewModel = viewModel;
            ViewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            var result = ViewModel.EnsureConnectionIsValid();
            if (result.IsSuccess)
            {
                ViewModel.SaveChanges();
                DialogResult = true;
            }
            else
            {
                ShowError(result.Message);
            }
        }
        
        private void OnTestConnection(object sender, RoutedEventArgs e)
        {
            var result = ViewModel.EnsureConnectionIsValid();
            if (result.IsSuccess)
            {
                ShowSuccess(result.Message);
            }
            else
            {
                ShowError(result.Message);
            }
        }

        private void OnSaveChanges(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveChanges();
            DialogResult = false;
        }
        
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConnectViewModel.Current) && ViewModel.Current != null)
            {
                PasswordTextBox.Password = ViewModel.Current.Password;
            }
        }
        
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Current.Password = PasswordTextBox.Password;
        }

        private void ShowSuccess(string message)
        {
            const string title = "Success";
            MessageBox.Show(
                this,
                message,
                title, 
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ShowError(string message)
        {
            const string title = "Connection error";
            MessageBox.Show(
                this,
                message,
                title, 
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
