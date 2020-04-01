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
            DataContext = ViewModel = viewModel;
            ViewModel.Window = this;
            ViewModel.ConnectToTest();
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
