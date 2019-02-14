using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Quickr.ViewModels;

namespace Quickr
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (DataContext as MainWindowViewModel)?.SelectCommand.Execute(e.NewValue);
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
