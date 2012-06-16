using ImageService.Common;
using System.Windows;

namespace WpfClient
{
    public class WPFNotifier : IClientNotifier
    {

        public void Warning(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void Message(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Error(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
