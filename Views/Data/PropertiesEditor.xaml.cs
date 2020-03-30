using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Quickr.Views.Data
{
    /// <summary>
    /// Interaction logic for PropertiesEditor.xaml
    /// </summary>
    public partial class PropertiesEditor : UserControl
    {
        public PropertiesEditor()
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
