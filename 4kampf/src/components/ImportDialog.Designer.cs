namespace kampfpanzerin.components {
    partial class ImportDialog {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.repoList = new System.Windows.Forms.ListBox();
            this.teamTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fetchBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.userNameTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.chooseFolderButton = new System.Windows.Forms.Button();
            this.locationTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnImport.Location = new System.Drawing.Point(279, 354);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(61, 23);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Scheisse";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCancel.Location = new System.Drawing.Point(346, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Geh Kacken";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::kampfpanzerin.Properties.Resources.clive;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 69);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(90, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Import a project from Bitbucket";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(268, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Hunky Dory Boss, which project do you want to import?";
            // 
            // repoList
            // 
            this.repoList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.repoList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.repoList.ForeColor = System.Drawing.Color.White;
            this.repoList.FormattingEnabled = true;
            this.repoList.Location = new System.Drawing.Point(94, 191);
            this.repoList.Name = "repoList";
            this.repoList.Size = new System.Drawing.Size(326, 106);
            this.repoList.TabIndex = 4;
            this.repoList.Validating += new System.ComponentModel.CancelEventHandler(this.repoList_Validating);
            // 
            // teamTxt
            // 
            this.teamTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.teamTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.teamTxt.ForeColor = System.Drawing.Color.White;
            this.teamTxt.Location = new System.Drawing.Point(164, 87);
            this.teamTxt.Name = "teamTxt";
            this.teamTxt.Size = new System.Drawing.Size(256, 20);
            this.teamTxt.TabIndex = 1;
            this.teamTxt.TextChanged += new System.EventHandler(this.teamTxt_TextChanged);
            this.teamTxt.Validating += new System.ComponentModel.CancelEventHandler(this.teamTxt_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Team";
            // 
            // fetchBtn
            // 
            this.fetchBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.fetchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fetchBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.fetchBtn.Location = new System.Drawing.Point(359, 142);
            this.fetchBtn.Name = "fetchBtn";
            this.fetchBtn.Size = new System.Drawing.Size(61, 23);
            this.fetchBtn.TabIndex = 3;
            this.fetchBtn.Text = "Fetch";
            this.fetchBtn.UseVisualStyleBackColor = false;
            this.fetchBtn.Click += new System.EventHandler(this.teamBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(91, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "User name";
            // 
            // userNameTxt
            // 
            this.userNameTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.userNameTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userNameTxt.ForeColor = System.Drawing.Color.White;
            this.userNameTxt.Location = new System.Drawing.Point(164, 113);
            this.userNameTxt.Name = "userNameTxt";
            this.userNameTxt.Size = new System.Drawing.Size(256, 20);
            this.userNameTxt.TabIndex = 2;
            this.userNameTxt.TextChanged += new System.EventHandler(this.teamTxt_TextChanged);
            this.userNameTxt.Validating += new System.ComponentModel.CancelEventHandler(this.teamTxt_Validating);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label5.Location = new System.Drawing.Point(94, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(326, 2);
            this.label5.TabIndex = 21;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // chooseFolderButton
            // 
            this.chooseFolderButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.chooseFolderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chooseFolderButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.chooseFolderButton.Location = new System.Drawing.Point(395, 312);
            this.chooseFolderButton.Margin = new System.Windows.Forms.Padding(0);
            this.chooseFolderButton.Name = "chooseFolderButton";
            this.chooseFolderButton.Size = new System.Drawing.Size(27, 23);
            this.chooseFolderButton.TabIndex = 6;
            this.chooseFolderButton.Text = "...";
            this.chooseFolderButton.UseVisualStyleBackColor = false;
            this.chooseFolderButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
            // 
            // locationTxt
            // 
            this.locationTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.locationTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.locationTxt.ForeColor = System.Drawing.Color.White;
            this.locationTxt.Location = new System.Drawing.Point(181, 314);
            this.locationTxt.Name = "locationTxt";
            this.locationTxt.Size = new System.Drawing.Size(205, 20);
            this.locationTxt.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(91, 316);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Target Location";
            // 
            // ImportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(435, 389);
            this.Controls.Add(this.chooseFolderButton);
            this.Controls.Add(this.locationTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.userNameTxt);
            this.Controls.Add(this.fetchBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.teamTxt);
            this.Controls.Add(this.repoList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCancel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import a project from Bitbucket";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox repoList;
        private System.Windows.Forms.TextBox teamTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button fetchBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox userNameTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.TextBox locationTxt;
        private System.Windows.Forms.Label label6;
    }
}