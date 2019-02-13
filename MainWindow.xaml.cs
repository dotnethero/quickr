using System.Windows;
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
    }
}
