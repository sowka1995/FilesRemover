using System.Windows.Forms;

namespace FilesRemover
{
    internal class MessageBoxLogger : ILogger
    {
        public void Log(string message)
        {
            MessageBox.Show(message);
        }
    }
}
