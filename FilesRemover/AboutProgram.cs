using System;
using System.Windows.Forms;

namespace FilesRemover
{
    public partial class AboutProgram : Form
    {
        private static AboutProgram _instance;

        public AboutProgram()
        {
            InitializeComponent();
        }

        private void AboutProgram_Load(object sender, EventArgs e)
        {

        }

        public static AboutProgram GetInstance()
        {
            if (_instance == null) _instance = new AboutProgram();
            return _instance;
        }

        private void AboutProgram_FormClosing(object sender, FormClosingEventArgs e)
        {
            _instance = null;
        }
    }
}
