using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Quickr.Utils;
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
            ViewModel.ConnectToTest(); // NOTE: load test connection async
        }

        private async void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!ViewModel.IsUnsaved)
            {
                await ViewModel.Select(e.NewValue);
            }
            else
            {
                var result = MessageBox.Show(
                    this,
                    "Key has unsaved changes! Do you want to save them?",
                    "Unsaved changes!",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        await ViewModel.Save();
                        await ViewModel.Select(e.NewValue);
                        break;
                    case MessageBoxResult.No:
                        await ViewModel.Select(e.NewValue);
                        break;
                    case MessageBoxResult.Cancel:
                        // TODO: Cancel select
                        break;
                }
            }
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
