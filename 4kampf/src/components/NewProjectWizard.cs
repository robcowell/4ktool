using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            if (nameTxt.Text.Length == 0) {
                error = "Please enter a location";
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
    }
}
