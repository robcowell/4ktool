using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace kampfpanzerin.components {
    public partial class NewProjectWizard : Form {

        bool ValidationCalcels = false;

        public NewProjectWizard() {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) {
            MessageBoxManager.Cancel = "Cancel";
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.ShowNewFolderButton = true;
            d.Description = "Let's choose somewhere to build this bad boy!";
            if (d.ShowDialog() == DialogResult.Cancel)
                return;

            locationTxt.Text = d.SelectedPath;
        }

        private void form_Validating(object sender, CancelEventArgs e) {
        }

        private void nameTxt_Validating(object sender, CancelEventArgs e) {
            string error = null;
            if (nameTxt.Text.Length == 0) {
                error = "Please enter a name";
                e.Cancel = ValidationCalcels;
            }
            errorProvider1.SetError((Control)sender, error);
        }

        private void locationTxt_Validating(object sender, CancelEventArgs e) {
            string error = null;
            if (locationTxt.Text.Length == 0) {
                error = "Please enter a location";
                e.Cancel = ValidationCalcels;
            } else if (Directory.EnumerateFiles(locationTxt.Text).Any()) {
                error = "Dude! I can't create a project in a non-empty folder, man!";
                e.Cancel = ValidationCalcels;
            }
            errorProvider2.SetError((Control)sender, error);
        }

        private void btnSave_Click(object sender, EventArgs e) {
            this.ValidationCalcels = true;
            if (this.ValidateChildren()) {
                this.DialogResult = DialogResult.OK;
            }
            this.ValidationCalcels = false;
        }

        public string ProjectLocation { get { return this.locationTxt.Text; } }
        public string ProjectName { get { return this.nameTxt.Text; } }
        public bool UseClinkster { get { return this.clinkster.Checked; } }
        public bool UseBitBucket { get { return this.checkBox1.Checked; } }
    }
}
