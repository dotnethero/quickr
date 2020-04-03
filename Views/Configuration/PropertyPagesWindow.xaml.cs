using System.Windows;
using Quickr.ViewModels.Configuration;

namespace Quickr.Views
{
    public partial class PropertyPagesWindow : Window
    {
        private PropertyPagesViewModel ViewModel { get; }

        internal PropertyPagesWindow(PropertyPagesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = ViewModel = viewModel;
        }
    }
}
