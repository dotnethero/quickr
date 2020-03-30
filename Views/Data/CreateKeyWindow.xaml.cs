using System.Windows;

namespace Quickr.Views.Data
{
    /// <summary>
    /// Interaction logic for CreateKeyWindow.xaml
    /// </summary>
    public partial class CreateKeyWindow : Window
    {
        public CreateKeyWindow()
        {
            InitializeComponent();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
