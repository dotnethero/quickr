using System.Windows;
using Quickr.ViewModels.Configuration;

namespace Quickr.Views.Configuration
{
    public partial class PropertyPagesWindow : Window
    {
        internal PropertyPagesWindow(PropertyPagesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
