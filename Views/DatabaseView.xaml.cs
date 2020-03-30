using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Quickr.Views
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : UserControl
    {
        public DatabaseView()
        {
            InitializeComponent();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (sender is TextBox text && e.Key == Key.Enter)
            {
                var bindingExpression = BindingOperations.GetBindingExpression(text, TextBox.TextProperty);
                bindingExpression?.UpdateSource();
            }
        }
    }
}
