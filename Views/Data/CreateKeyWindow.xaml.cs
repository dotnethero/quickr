using System.ComponentModel;
using System.Windows;
using Quickr.ViewModels.Data;

namespace Quickr.Views.Data
{
    /// <summary>
    /// Interaction logic for CreateKeyWindow.xaml
    /// </summary>
    public partial class CreateKeyWindow : Window
    {
        private CreateKeyViewModel ViewModel { get; }

        internal CreateKeyWindow(CreateKeyViewModel viewModel)
        {
            InitializeComponent();
            DataContext = ViewModel = viewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult != true)
            {
                ViewModel.Cancel();
            }
        }

        private async void OnSave(object sender, RoutedEventArgs e)
        {
            await ViewModel.Save();
            DialogResult = true;
        }
    }
}
