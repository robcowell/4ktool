using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kampfpanzerin.components {
        public partial class SaveWithCommitMessage : Form {
        public SaveWithCommitMessage() {
            InitializeComponent();
        }

        internal string GetCommitMessage() {
            return txtCommitMessage.Text;
        }
    }
}
