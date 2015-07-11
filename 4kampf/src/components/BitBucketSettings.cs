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
    }
}
