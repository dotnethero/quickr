using System.ComponentModel;
using System.Linq;
using System.Windows;
using Quickr.Models;
using Quickr.Properties;
using Quickr.ViewModels;

namespace Quickr.Views
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        private readonly ConnectViewModel _viewModel;

        internal ConnectWindow(ConnectViewModel viewModel, Window owner)
        {
            InitializeComponent();
            Owner = owner;
            DataContext = _viewModel = viewModel;
            CheckExisting();
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
            if (TestConnection())
            {
                SaveChanges();
                DialogResult = true;
            }
        }
        
        private void OnSaveChanges(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            DialogResult = false;
        }

        private void SaveChanges()
        {
            Settings.Current.Endpoints = _viewModel.Endpoints.ToList();
            Settings.Current.Save();
        }
        
        private void CheckExisting()
        {
            if (_viewModel.Endpoints.Count == 0)
            {
                _viewModel.Endpoints.Add(new EndPointModel());
            }

            if (_viewModel.Current == null)
            {
                _viewModel.Current = _viewModel.Endpoints.First();
            }
        }
        
        private void OnAdd(object sender, RoutedEventArgs e)
        {
            var model = new EndPointModel();
            _viewModel.Endpoints.Add(model);
            _viewModel.Current = model;
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            _viewModel.Endpoints.Remove(_viewModel.Current);
            CheckExisting();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Current.Password = PasswordTextBox.Password;
        }

        private void OnTestConnection(object sender, RoutedEventArgs e)
        {
            TestConnection();
        }

        private bool TestConnection()
        {
            var current = _viewModel.Current;
            if (string.IsNullOrEmpty(current.Server))
            {
                MessageBox.Show(
                    this,
                    "Connection property \"Server\" can not be empty!", 
                    "Test connection", 
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
            return true;
        }
    }
}
