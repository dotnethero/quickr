using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quickr.Views
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    public partial class ServerInfoView : UserControl
    {
        public ServerInfoView()
        {
            InitializeComponent();
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ListView list)
            {
                list.SelectedItem = null;
            }
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            if (sender is ListView list)
            {
                e.Handled = true;
                var parent = (UIElement)list.Parent;
                var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent, 
                    Source = sender
                };
                parent.RaiseEvent(args);
            }
        }
    }
}
