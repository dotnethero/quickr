using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Quickr.ViewModels.Connection;

namespace Quickr.Views
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        private CancellationTokenSource CancellationTokenSource { get; }
        private ConnectViewModel ViewModel { get; }

        internal ConnectWindow(ConnectViewModel viewModel)
        {
            InitializeComponent();
            DataContext = ViewModel = viewModel;
            ViewModel.PropertyChanged += OnPropertyChanged;
            CancellationTokenSource = new CancellationTokenSource();
        }

        private async void OnConnect(object sender, RoutedEventArgs e)
        {
            var token = CancellationTokenSource.Token;
            var result = await Task.Run(() => ViewModel.EnsureConnectionIsValid(false, token), token);
            if (token.IsCancellationRequested)
            {
                return;
            }
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

        private async void OnTestConnection(object sender, RoutedEventArgs e)
        {
            var token = CancellationTokenSource.Token;
            var result = await Task.Run(() => ViewModel.EnsureConnectionIsValid(true, token), token);
            if (token.IsCancellationRequested)
            {
                return;
            }
            if (result.IsSuccess)
            {
                ShowSuccess(result.Message);
            }
            else
            {
                ShowError(result.Message);
            }
        }
        
        private void OnClose(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource.Cancel();
            DialogResult = false;
        }

        private void OnSaveChanges(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveChanges();
            MessageBox.Show(
                this,
                "Connection options have been saved!",
                "Saved!", 
                MessageBoxButton.OK,
                MessageBoxImage.Information);
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
