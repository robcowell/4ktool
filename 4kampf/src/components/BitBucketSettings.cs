using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using kampfpanzerin.core.Serialization;
using kampfpanzerin.utils;


namespace kampfpanzerin.components {
    public partial class BitBucketSettings : Form {
        private string slug;
        public BitBucketSettings(string slug) {
            this.slug = slug;
            InitializeComponent();
            this.userName.Text = Properties.Settings.Default.gitAuthor;
            this.team.Text = Properties.Settings.Default.bbTeam;
            this.textBox3.Text = Properties.Settings.Default.gitEmail;
        }

        public BitBucketData Data {
            get {
                BitBucketData bbd = new BitBucketData();
                bbd.RepoSlug = slug;
                bbd.Team = this.team.Text;
                bbd.UserName = this.userName.Text;
                return bbd;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Properties.Settings.Default.gitAuthor = this.userName.Text;
            Properties.Settings.Default.bbTeam = this.team.Text;
            Properties.Settings.Default.gitEmail = this.textBox3.Text;
        }
    }
}
