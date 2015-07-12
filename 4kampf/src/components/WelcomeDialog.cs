using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kampfpanzerin {
    public enum WelcomeDialogResult
    {
        CREATE,
        IMPORT,
        OPEN,
        QUIT
    }

    public partial class WelcomeDialog : Form
    {
        public WelcomeDialogResult result { get; set; }

        public WelcomeDialog() {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            this.result = WelcomeDialogResult.CREATE;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.result = WelcomeDialogResult.IMPORT;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.result = WelcomeDialogResult.OPEN;
            this.Close();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            this.result = WelcomeDialogResult.QUIT;
            this.Close();
        }
    }
}
