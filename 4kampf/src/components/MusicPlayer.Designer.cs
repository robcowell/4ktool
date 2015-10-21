namespace kampfpanzerin {
    partial class MusicPlayer {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MusicPlayer));
            this.trkVolume = new MetroFramework.Controls.MetroTrackBar();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTrackLength = new System.Windows.Forms.Label();
            this.lblFPS = new System.Windows.Forms.Label();
            this.trkTransport = new MetroFramework.Controls.MetroTrackBar();
            this.mediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.lblBPM = new System.Windows.Forms.Label();
            this.pbBeat = new System.Windows.Forms.PictureBox();
            this.btnMute = new System.Windows.Forms.Button();
            this.btnRewind = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.vu = new kampfpanzerin.VUMeter();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBeat)).BeginInit();
            this.SuspendLayout();
            // 
            // trkVolume
            // 
            this.trkVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trkVolume.BackColor = System.Drawing.Color.Transparent;
            this.trkVolume.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.trkVolume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.trkVolume.LargeChange = 3;
            this.trkVolume.Location = new System.Drawing.Point(191, 13);
            this.trkVolume.Name = "trkVolume";
            this.trkVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trkVolume.Size = new System.Drawing.Size(135, 16);
            this.trkVolume.TabIndex = 19;
            this.trkVolume.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.trkVolume.UseCustomBackColor = true;
            this.trkVolume.Value = 1;
            this.trkVolume.ValueChanged += new System.EventHandler(this.trkVolume_ValueChanged);
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.lblTime.Location = new System.Drawing.Point(64, 5);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(97, 31);
            this.lblTime.TabIndex = 15;
            this.lblTime.Text = "000.00";
            // 
            // lblTrackLength
            // 
            this.lblTrackLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackLength.AutoSize = true;
            this.lblTrackLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.lblTrackLength.Location = new System.Drawing.Point(67, 42);
            this.lblTrackLength.Name = "lblTrackLength";
            this.lblTrackLength.Size = new System.Drawing.Size(40, 13);
            this.lblTrackLength.TabIndex = 17;
            this.lblTrackLength.Text = "000.00";
            // 
            // lblFPS
            // 
            this.lblFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFPS.AutoSize = true;
            this.lblFPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.lblFPS.Location = new System.Drawing.Point(118, 42);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(36, 13);
            this.lblFPS.TabIndex = 18;
            this.lblFPS.Text = "60 fps";
            // 
            // trkTransport
            // 
            this.trkTransport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkTransport.BackColor = System.Drawing.Color.Transparent;
            this.trkTransport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.trkTransport.Location = new System.Drawing.Point(191, 35);
            this.trkTransport.Name = "trkTransport";
            this.trkTransport.Size = new System.Drawing.Size(706, 22);
            this.trkTransport.TabIndex = 12;
            this.trkTransport.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.trkTransport.UseCustomBackColor = true;
            this.trkTransport.ValueChanged += new System.EventHandler(this.trkTransport_ValueChanged);
            // 
            // mediaPlayer
            // 
            this.mediaPlayer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mediaPlayer.Enabled = true;
            this.mediaPlayer.Location = new System.Drawing.Point(562, 6);
            this.mediaPlayer.Name = "mediaPlayer";
            this.mediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mediaPlayer.OcxState")));
            this.mediaPlayer.Size = new System.Drawing.Size(86, 30);
            this.mediaPlayer.TabIndex = 20;
            this.mediaPlayer.TabStop = false;
            this.mediaPlayer.Visible = false;
            this.mediaPlayer.OpenStateChange += new AxWMPLib._WMPOCXEvents_OpenStateChangeEventHandler(this.mediaPlayer_OpenStateChange);
            this.mediaPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.mediaPlayer_PlayStateChange);
            // 
            // lblBPM
            // 
            this.lblBPM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBPM.AutoSize = true;
            this.lblBPM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.lblBPM.Location = new System.Drawing.Point(344, 15);
            this.lblBPM.Name = "lblBPM";
            this.lblBPM.Size = new System.Drawing.Size(27, 13);
            this.lblBPM.TabIndex = 18;
            this.lblBPM.Text = "bpm";
            // 
            // pbBeat
            // 
            this.pbBeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbBeat.Image = global::kampfpanzerin.Properties.Resources.Beat;
            this.pbBeat.Location = new System.Drawing.Point(334, 16);
            this.pbBeat.Name = "pbBeat";
            this.pbBeat.Size = new System.Drawing.Size(10, 10);
            this.pbBeat.TabIndex = 21;
            this.pbBeat.TabStop = false;
            // 
            // btnMute
            // 
            this.btnMute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnMute.Image = global::kampfpanzerin.Properties.Resources.Mute;
            this.btnMute.Location = new System.Drawing.Point(163, 7);
            this.btnMute.Margin = new System.Windows.Forms.Padding(0);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(24, 24);
            this.btnMute.TabIndex = 13;
            this.btnMute.UseVisualStyleBackColor = true;
            this.btnMute.Click += new System.EventHandler(this.btnMute_Click);
            // 
            // btnRewind
            // 
            this.btnRewind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRewind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRewind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRewind.Image = global::kampfpanzerin.Properties.Resources.Rewind;
            this.btnRewind.Location = new System.Drawing.Point(162, 31);
            this.btnRewind.Margin = new System.Windows.Forms.Padding(0);
            this.btnRewind.Name = "btnRewind";
            this.btnRewind.Size = new System.Drawing.Size(24, 24);
            this.btnRewind.TabIndex = 14;
            this.btnRewind.UseVisualStyleBackColor = true;
            this.btnRewind.Click += new System.EventHandler(this.btnRewind_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnPlay.Image = global::kampfpanzerin.Properties.Resources.Play;
            this.btnPlay.Location = new System.Drawing.Point(2, 2);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(58, 58);
            this.btnPlay.TabIndex = 8;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // vu
            // 
            this.vu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.vu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.vu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.vu.Location = new System.Drawing.Point(746, 8);
            this.vu.Name = "vu";
            this.vu.Size = new System.Drawing.Size(151, 24);
            this.vu.TabIndex = 11;
            // 
            // MusicPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Controls.Add(this.pbBeat);
            this.Controls.Add(this.mediaPlayer);
            this.Controls.Add(this.vu);
            this.Controls.Add(this.trkVolume);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblTrackLength);
            this.Controls.Add(this.lblBPM);
            this.Controls.Add(this.lblFPS);
            this.Controls.Add(this.btnMute);
            this.Controls.Add(this.btnRewind);
            this.Controls.Add(this.trkTransport);
            this.Controls.Add(this.btnPlay);
            this.Name = "MusicPlayer";
            this.Size = new System.Drawing.Size(906, 62);
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBeat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private VUMeter vu;
        private MetroFramework.Controls.MetroTrackBar trkVolume;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblTrackLength;
        private System.Windows.Forms.Label lblFPS;
        private System.Windows.Forms.Button btnMute;
        private System.Windows.Forms.Button btnRewind;
        private MetroFramework.Controls.MetroTrackBar trkTransport;
        public AxWMPLib.AxWindowsMediaPlayer mediaPlayer;
        private System.Windows.Forms.Label lblBPM;
        private System.Windows.Forms.PictureBox pbBeat;
    }
}
