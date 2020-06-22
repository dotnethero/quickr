using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Quickr.Models.Keys;
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
        }

        private async Task<bool> Select(BaseEntry entry)
        {
            if (!ViewModel.IsUnsaved)
            {
                await ViewModel.Select(entry);
                return true;
            }

            var result = MessageBox.Show(
                this,
                "Key has unsaved changes! Do you want to save them?",
                "Unsaved changes!",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    var saveResult = await ViewModel.Save();
                    if (saveResult)
                    {
                        await ViewModel.Select(entry);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case MessageBoxResult.No:
                    await ViewModel.Select(entry);
                    return true;
            }

            return false;
        }

        private async void BeforeClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeView && e.OriginalSource is DependencyObject el)
            {
                var item = FindAncestor<TreeViewItem, ToggleButton>(el);
                if (item != null && item.DataContext is BaseEntry entry)
                {
                    var proceed = await Select(entry);
                    if (proceed)
                    {
                        item.IsSelected = true;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private static T FindAncestor<T, TExcluded>(DependencyObject el) where T: DependencyObject
        {
            while (el != null)
            {
                if (el is T ancestor) return ancestor;
                if (el is TExcluded _) return null;
                el = VisualTreeHelper.GetParent(el);
            }
            return null;
        }
    }
}
