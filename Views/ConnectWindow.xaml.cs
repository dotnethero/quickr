﻿using System.ComponentModel;
using System.Linq;
using System.Windows;
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
            DataContext = _viewModel = viewModel;
            Owner = owner;
            _viewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConnectViewModel.Current))
            {
                PasswordTextBox.Password = _viewModel.Current.Password;
            }
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            // check connection
            Settings.Current.Endpoints = _viewModel.Endpoints.Where(x => !x.IsNew).ToList();
            Settings.Current.Save();
            DialogResult = true;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Current.Password = PasswordTextBox.Password;
        }
    }
}
