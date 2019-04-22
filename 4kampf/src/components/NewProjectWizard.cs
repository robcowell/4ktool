using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using kampfpanzerin.core.Serialization;
using kampfpanzerin.utils;
using System.Net;

namespace kampfpanzerin.components {
    public partial class NewProjectWizard : Form {

        bool ValidationCancels = false;
        private BitBucketData bbd;
        public BitBucketData BitBucketConfig {
            get {
                if (bbd != null) {
                    return bbd;
                } else {
                    return new BitBucketSettings(this.slugTxt.Text).Data;
                }
            }
        }

        public Project Project {
            get;
            private set;
        }

        public NewProjectWizard() {
            InitializeComponent();
            if (Properties.Settings.Default.lastProjectLocation.Length > 0) {
                DirectoryInfo parentDir = Directory.GetParent(Properties.Settings.Default.lastProjectLocation);
                this.locationTxt.Text = parentDir.FullName;
            } else {
                this.locationTxt.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            this.UserName.Text = Properties.Settings.Default.UserName;
            if (this.UserName.Text == null || this.UserName.Text.Length == 0) {
                this.UserName.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
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

        private void nameTxt_Validating(object sender, CancelEventArgs e) {
            string error = null;
            if (nameTxt.Text.Length == 0) {
                error = "Please enter a name";
                e.Cancel = ValidationCancels;
            } else if (Directory.Exists(locationTxt.Text + @"\" + nameTxt.Text)) {
                error = "Dude! That project exists, you should finnish stuff before before starting a new one, man!";
                e.Cancel = ValidationCancels;
            }
            errorProvider1.SetError((Control)sender, error);
        }

        private void userName_Validating(object sender, CancelEventArgs e) {
            string error = null;
            if (UserName.Text.Length == 0) {
                error = "Please enter a user name";
                e.Cancel = ValidationCancels;
            }
            errorProvider1.SetError((Control)sender, error);
        }

        private void locationTxt_Validating(object sender, CancelEventArgs e) {
            string error = null;
            if (locationTxt.Text.Length == 0) {
                error = "Please enter a location";
                e.Cancel = ValidationCancels;
            } else if (!Directory.Exists(locationTxt.Text)) {
                error = "Err that's not a folder mate ://";
                e.Cancel = ValidationCancels;
            }
            errorProvider2.SetError((Control)sender, error);
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (BitBucketConfig != null) {
                BitBucketConfig.RepoSlug = slugTxt.Text;
            }
            this.ValidationCancels = true;
            if (this.ValidateChildren()) {

                Cursor.Current = Cursors.WaitCursor;
                Project = new Project();
                Project.name = nameTxt.Text;
                Project.useBitBucket = UseBitBucket;
                if (UseBitBucket) {
                    NetworkCredential credentials;
                    credentials = BitBucketUtils.GetCredentials(BitBucketConfig);
                    if (credentials == null) {
                        MessageBox.Show("No Credentials given", "4krampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else {
                        string result = git.GitHandler.CreateBitBucketRepo(BitBucketConfig, credentials);
                        if (result == null) {
                            MessageBox.Show("Could not create repo, either it exists or credentials are wrong", "4krampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            DialogResult = DialogResult.None;
                            BitBucketUtils.ClearCredentials(BitBucketConfig);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        Project.bitBucketSettings = BitBucketConfig;
                        Project.gitRemote = result;
                        this.Project = Project;
                        this.DialogResult = DialogResult.OK;
                    }
                } else {
                    this.DialogResult = DialogResult.OK;
                }
            }
            this.ValidationCancels = false;
            Cursor.Current = Cursors.Default;
            Properties.Settings.Default.UserName = this.UserName.Text;
            git.GitHandler handler = new git.GitHandler();
            handler.SetUsername(this.UserName.Text);
            
        }

        public string ProjectLocation { get { return this.locationTxt.Text; } }
        public string ProjectName { get { return this.nameTxt.Text; } }
        public bool UseClinkster { get { return this.clinkster.Checked; } }

        public Synth Synth {
            get {
                return this.clinkster.Checked
                    ? Synth.clinkster :
                    this.oidos.Checked ? Synth.oidos : Synth.vierklang;
            }
        }
        public bool UseBitBucket { get { return this.checkBox1.Checked; } }

        private void bitbucketSettingsButton_Click(object sender, EventArgs e) {
            BitBucketSettings frm = new BitBucketSettings(nameTxt.Text);
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK) {
                bbd = frm.Data;
            }
            DialogResult = DialogResult.None;
        }

        private void nameTxt_TextChanged(object sender, EventArgs e) {
            slugTxt.Text = nameTxt.Text.GenerateSlug();
        }

        private void checkBox1_Validating(object sender, CancelEventArgs e) {
            if (!checkBox1.Checked) {
                return;
            }
            string error = null;
            if (BitBucketConfig == null || (BitBucketConfig.Team == null || BitBucketConfig.UserName == null || BitBucketConfig.RepoSlug == null)) {
                error = "Dude! If you want to use BitBucket, give me some information, man!";
                e.Cancel = ValidationCancels;
            }
            errorProvider2.SetError((Control)sender, error);
        }

    }
}
