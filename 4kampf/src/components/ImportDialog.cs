using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using kampfpanzerin.git;
using kampfpanzerin.utils;
using kampfpanzerin.core.Serialization;
using System.IO;

namespace kampfpanzerin.components {
    public partial class ImportDialog : Form {
        private bool ValidationCancels = false;

        internal RepoDescriptor Repo {
            get {
                return (RepoDescriptor)repoList.SelectedItem;
            }
        }

        public string Team {
            get {
                return teamTxt.Text;
            }
        }

        public string UserName {
            get {
                return userNameTxt.Text;
            }
        }
        public string ProjectLocation {
            get {
                return this.locationTxt.Text;
            }
        }

        public ImportDialog() {
            InitializeComponent();
            this.userNameTxt.Text = Properties.Settings.Default.gitAuthor;
            this.teamTxt.Text = Properties.Settings.Default.bbTeam;
            this.locationTxt.Text = Properties.Settings.Default.lastProjectLocation;
            teamTxt_TextChanged(null, null);

            repoList.DisplayMember = "Name";
        }

        private void teamBtn_Click(object sender, EventArgs e) {
            Cursor.Current = Cursors.WaitCursor;
            var bbc = new BitBucketData();
            bbc.Team = teamTxt.Text;
            bbc.UserName = userNameTxt.Text;
            repoList.Items.Clear();
            var repos = GitHandler.GetBitBucketRepos(teamTxt.Text, BitBucketUtils.GetCredentials(bbc));
            if (repos == null)
            {
                MessageBox.Show("Got no repos!");
                return;
            }
            foreach (var item in repos) {
                repoList.Items.Add(item);
            }
            Cursor.Current = Cursors.Default;
        }

        private void teamTxt_TextChanged(object sender, EventArgs e) {
            fetchBtn.Enabled = !(string.IsNullOrEmpty(teamTxt.Text) ||string.IsNullOrEmpty(userNameTxt.Text));
        }

        private void teamTxt_Validating(object sender, CancelEventArgs e) {
            if (string.IsNullOrEmpty(((TextBox)sender).Text)) {
                e.Cancel = ValidationCancels;
                errorProvider1.SetError((Control)sender, "I need you to fill this field");
            }
        }

        private void repoList_Validating(object sender, CancelEventArgs e) {
            if (repoList.SelectedItem == null) {
                e.Cancel = ValidationCancels;
                errorProvider1.SetError((Control)sender, "Dude, I would like to know which damn project!");
            }
        }

        private void btnImport_Click(object sender, EventArgs e) {
            ValidationCancels = !ValidateChildren();
        }

        private void chooseFolderButton_Click(object sender, EventArgs e) {
            MessageBoxManager.Cancel = "Cancel";
            FolderBrowserDialog d = new FolderBrowserDialog();
            d.ShowNewFolderButton = true;
            d.Description = "Let's choose somewhere to import this bad boy to!";
            if (d.ShowDialog() == DialogResult.Cancel)
                return;

            locationTxt.Text = d.SelectedPath;
        }

        private void locationTxt_Validating(object sender, CancelEventArgs e) {
            string error = null;
            if (locationTxt.Text.Length == 0) {
                error = "Please enter a location";
                e.Cancel = ValidationCancels;
            } else if (!Directory.Exists(locationTxt.Text)) {
                error = "Err that's not a folder mate ://";
                e.Cancel = ValidationCancels;
            } else if (repoList.SelectedItem != null && Directory.Exists(locationTxt.Text + @"\" + Repo.Slug)) {
                error = "Dude! I can't create a project in a non-empty folder, man!";
                e.Cancel = ValidationCancels;
            }
            errorProvider1.SetError((Control)sender, error);
        }
    }
}
