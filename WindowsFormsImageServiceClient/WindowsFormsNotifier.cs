using System.Windows.Forms;
using ImageService.Common;

namespace WindowsFormsImageServiceClient
{
    public class WindowsFormsNotifier : IClientNotifier
    {

        public void Warning(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void Message(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Error(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
