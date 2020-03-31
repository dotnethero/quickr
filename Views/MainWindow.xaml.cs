using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Quickr.Models;
using Quickr.ViewModels;

namespace Quickr.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel { get; }

        internal MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = viewModel;

            ViewModel.Window = this;

            // for test purposes
            var model = new EndPointModel
            {
                Server = "localhost",
                Port = 6379
            };
            ViewModel.ConnectCommand.Execute(model);
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            var endpoints = new List<EndPointModel>
            {
                new EndPointModel
                {
                    Name = "localhost",
                    Server = "localhost",
                    Port = 6379,
                    IsNew = false
                },
                new EndPointModel
                {
                    Name = "azure-db",
                    Server = "redis-19774.c56.east-us.azure.cloud.redislabs.com",
                    Port = 19774,
                    Password = "vVPZlXbD868wlEy3bFhSPNdX51ITW7jt",
                    IsNew = false
                }
            };
            var model = new ConnectViewModel(endpoints);
            var conn = new ConnectWindow(model, this);
            if (conn.ShowDialog() == true)
            {
                ViewModel.ConnectCommand.Execute(model.Current);
            }
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ViewModel.SelectCommand.Execute(e.NewValue);
        }

        private void BeforeRightClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeView && e.OriginalSource is DependencyObject el)
            {
                var item = FindAncestor<TreeViewItem>(el);
                if (item != null)
                {
                    item.IsSelected = true;
                }
            }
        }

        private static T FindAncestor<T>(DependencyObject el) where T: DependencyObject
        {
            while (el != null)
            {
                if (el is T ancestor) return ancestor;
                el = VisualTreeHelper.GetParent(el);
            }
            return null;
        }
    }
}
