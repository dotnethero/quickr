using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Quickr.Models;
using Quickr.Services;
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
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            var conn = new ConnectWindow();
            var model = new EndPointModel("localhost", 6379);
            conn.DataContext = model;
            conn.Owner = this;
            if (conn.ShowDialog() == true)
            {
                ViewModel.ConnectCommand.Execute(model);
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
