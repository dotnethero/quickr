using System.Windows;
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
        public bool IsButtonsVisible
        {
            get => (bool) GetValue(IsButtonsVisibleProperty);
            set => SetValue(IsButtonsVisibleProperty, value);
        }

        public static readonly DependencyProperty IsButtonsVisibleProperty = 
            DependencyProperty.Register("IsButtonsVisible", typeof(bool), typeof(PropertiesEditor), new PropertyMetadata(true));

        public Thickness GroupBoxPadding
        {
            get => (Thickness)GetValue(GroupBoxPaddingProperty);
            set => SetValue(GroupBoxPaddingProperty, value);
        }

        public static readonly DependencyProperty GroupBoxPaddingProperty =
            DependencyProperty.Register("GroupBoxPadding", typeof(Thickness), typeof(PropertiesEditor), new PropertyMetadata(new Thickness(8, 8, 3, 2)));

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
