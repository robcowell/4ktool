using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kampfpanzerin.components {
    public partial class CredenditalPrompt : Form {

        public CredenditalPrompt(string target, string username = "") {
            InitializeComponent();
            this.subtextLabel.Text = string.Format(subtextLabel.Text, target);
        }

    }
}
