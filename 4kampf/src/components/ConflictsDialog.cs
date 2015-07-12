using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kampfpanzerin.components {

 

        public partial class ConflictsDialog : Form {
           public List<string> Resolved {
            get {
                List<string> resolved = new List<string>();
                foreach (object o in conflicts.CheckedItems) {
                    resolved.Add((string)o);
                }
                return resolved;
            }
        }

        public ConflictsDialog(List<string> conflicts) {
            InitializeComponent();
            conflicts.ForEach(c => this.conflicts.Items.Add(c, false));
        }
    }
}
