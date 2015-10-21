namespace kampfpanzerin
{
    partial class AppForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToVSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uniformsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableStandardUniformsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableCamControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableSyncTrackerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildShadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rerender4klangMuskcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unpackedReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packedReleasefastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packedReleaseslowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.rUNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minifyShadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colourHelperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableMultithread4klangInProdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useExtraPPShaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loopTrackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLinenumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.setVisualStudioDevCommandpromptLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeUniform = new System.Windows.Forms.Button();
            this.listBoxUniforms = new System.Windows.Forms.ListBox();
            this.splitMainLR = new System.Windows.Forms.SplitContainer();
            this.splitLHS = new System.Windows.Forms.SplitContainer();
            this.pnlCam = new System.Windows.Forms.Panel();
            this.walkButton = new System.Windows.Forms.Button();
            this.stepUpButton = new System.Windows.Forms.Button();
            this.btnCamReset = new System.Windows.Forms.Button();
            this.freeFlyButton = new System.Windows.Forms.Button();
            this.camAutoButton = new System.Windows.Forms.Button();
            this.stepDownButton = new System.Windows.Forms.Button();
            this.lockFlyButton = new System.Windows.Forms.Button();
            this.preview = new System.Windows.Forms.PictureBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.splitRHS = new System.Windows.Forms.SplitContainer();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnScreenshot = new System.Windows.Forms.Button();
            this.btnColourHelper = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnPull = new System.Windows.Forms.Button();
            this.btnPush = new System.Windows.Forms.Button();
            this.btnCleanProj = new System.Windows.Forms.Button();
            this.btnGotoProjectFolder = new System.Windows.Forms.Button();
            this.btnExportProj = new System.Windows.Forms.Button();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.btnOpenProj = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCleanBuild = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnRenderMusic = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnLineNumbers = new System.Windows.Forms.Button();
            this.btnTracker = new System.Windows.Forms.Button();
            this.btnEnvelopes = new System.Windows.Forms.Button();
            this.btnStandardUniforms = new System.Windows.Forms.Button();
            this.btnCamToggle = new System.Windows.Forms.Button();
            this.btnFullscreen = new System.Windows.Forms.Button();
            this.btnLoop = new System.Windows.Forms.Button();
            this.btnRefreshShaders = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pullFromBitBucketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pushToBitbucketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.klangPlayer = new kampfpanzerin.MusicPlayer();
            this.log = new kampfpanzerin.TextBoxWithScrollLeft();
            this.tabControl1 = new kampfpanzerin.TabControlFlat();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.edVert = new ScintillaNET.Scintilla();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.edFrag = new ScintillaNET.Scintilla();
            this.tabPP = new System.Windows.Forms.TabPage();
            this.edPost = new ScintillaNET.Scintilla();
            this.timeLine = new kampfpanzerin.TimeLine();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMainLR)).BeginInit();
            this.splitMainLR.Panel1.SuspendLayout();
            this.splitMainLR.Panel2.SuspendLayout();
            this.splitMainLR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLHS)).BeginInit();
            this.splitLHS.Panel1.SuspendLayout();
            this.splitLHS.Panel2.SuspendLayout();
            this.splitLHS.SuspendLayout();
            this.pnlCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitRHS)).BeginInit();
            this.splitRHS.Panel1.SuspendLayout();
            this.splitRHS.Panel2.SuspendLayout();
            this.splitRHS.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edVert)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edFrag)).BeginInit();
            this.tabPP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edPost)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.uniformsToolStripMenuItem,
            this.buildToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.elpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(943, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripSeparator5,
            this.pullFromBitBucketToolStripMenuItem,
            this.pushToBitbucketToolStripMenuItem,
            this.toolStripSeparator6,
            this.openProjectLocationToolStripMenuItem,
            this.exportToVSToolStripMenuItem,
            this.cleanToolStripMenuItem1,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.fileToolStripMenuItem.Text = "&Project";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.createToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.createToolStripMenuItem.Text = "&Create...";
            this.createToolStripMenuItem.Click += new System.EventHandler(this.createToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.saveAllToolStripMenuItem.Text = "&Save all";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // openProjectLocationToolStripMenuItem
            // 
            this.openProjectLocationToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openProjectLocationToolStripMenuItem.Name = "openProjectLocationToolStripMenuItem";
            this.openProjectLocationToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.openProjectLocationToolStripMenuItem.Text = "&Go to project folder";
            this.openProjectLocationToolStripMenuItem.Click += new System.EventHandler(this.openProjectLocationToolStripMenuItem_Click);
            // 
            // exportToVSToolStripMenuItem
            // 
            this.exportToVSToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exportToVSToolStripMenuItem.Name = "exportToVSToolStripMenuItem";
            this.exportToVSToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exportToVSToolStripMenuItem.Text = "&Export to Visual Studio";
            this.exportToVSToolStripMenuItem.Click += new System.EventHandler(this.exportToVSToolStripMenuItem_Click);
            // 
            // cleanToolStripMenuItem1
            // 
            this.cleanToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.cleanToolStripMenuItem1.Name = "cleanToolStripMenuItem1";
            this.cleanToolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
            this.cleanToolStripMenuItem1.Text = "&Clean";
            this.cleanToolStripMenuItem1.Click += new System.EventHandler(this.cleanToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // uniformsToolStripMenuItem
            // 
            this.uniformsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableStandardUniformsToolStripMenuItem,
            this.enableCamControlToolStripMenuItem,
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem,
            this.enableSyncTrackerToolStripMenuItem});
            this.uniformsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.uniformsToolStripMenuItem.Name = "uniformsToolStripMenuItem";
            this.uniformsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.uniformsToolStripMenuItem.Text = "&Uniforms";
            // 
            // enableStandardUniformsToolStripMenuItem
            // 
            this.enableStandardUniformsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.enableStandardUniformsToolStripMenuItem.Name = "enableStandardUniformsToolStripMenuItem";
            this.enableStandardUniformsToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.enableStandardUniformsToolStripMenuItem.Text = "Enable &standard uniforms";
            this.enableStandardUniformsToolStripMenuItem.ToolTipText = "vec3 un {resX, resY, time}";
            this.enableStandardUniformsToolStripMenuItem.Click += new System.EventHandler(this.enableStandardUniformsToolStripMenuItem_Click);
            // 
            // enableCamControlToolStripMenuItem
            // 
            this.enableCamControlToolStripMenuItem.AutoToolTip = true;
            this.enableCamControlToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.enableCamControlToolStripMenuItem.Name = "enableCamControlToolStripMenuItem";
            this.enableCamControlToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.enableCamControlToolStripMenuItem.Text = "Enable &cam control uniforms";
            this.enableCamControlToolStripMenuItem.ToolTipText = "vec3 cp (position), vec3 up (up vector), vec3 fd (forward vector)";
            this.enableCamControlToolStripMenuItem.Click += new System.EventHandler(this.enableCamControlToolStripMenuItem_Click);
            // 
            // enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem
            // 
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem.Name = "enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem";
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem.Text = "Enable &4klang envelope uniforms";
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem.ToolTipText = "float ev[MAX_INSTRUMENTS]";
            this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem.Click += new System.EventHandler(this.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem_Click);
            // 
            // enableSyncTrackerToolStripMenuItem
            // 
            this.enableSyncTrackerToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.enableSyncTrackerToolStripMenuItem.Name = "enableSyncTrackerToolStripMenuItem";
            this.enableSyncTrackerToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.enableSyncTrackerToolStripMenuItem.Text = "Enable s&ync timeline uniforms";
            this.enableSyncTrackerToolStripMenuItem.Click += new System.EventHandler(this.enableSyncTrackerToolStripMenuItem_Click);
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rebuildShadersToolStripMenuItem,
            this.rerender4klangMuskcToolStripMenuItem,
            this.toolStripSeparator4,
            this.debugToolStripMenuItem,
            this.unpackedReleaseToolStripMenuItem,
            this.packedReleasefastToolStripMenuItem,
            this.packedReleaseslowToolStripMenuItem,
            this.cleanToolStripMenuItem,
            this.toolStripSeparator3,
            this.rUNToolStripMenuItem});
            this.buildToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.buildToolStripMenuItem.Text = "&Build";
            // 
            // rebuildShadersToolStripMenuItem
            // 
            this.rebuildShadersToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.rebuildShadersToolStripMenuItem.Name = "rebuildShadersToolStripMenuItem";
            this.rebuildShadersToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.rebuildShadersToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.rebuildShadersToolStripMenuItem.Text = "Rebuild s&haders";
            this.rebuildShadersToolStripMenuItem.Click += new System.EventHandler(this.rebuildShadersToolStripMenuItem_Click);
            // 
            // rerender4klangMuskcToolStripMenuItem
            // 
            this.rerender4klangMuskcToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.rerender4klangMuskcToolStripMenuItem.Name = "rerender4klangMuskcToolStripMenuItem";
            this.rerender4klangMuskcToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.rerender4klangMuskcToolStripMenuItem.Text = "Render &music";
            this.rerender4klangMuskcToolStripMenuItem.Click += new System.EventHandler(this.rerender4klangMuskcToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(184, 6);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.debugToolStripMenuItem.Text = "&Debug";
            this.debugToolStripMenuItem.Click += new System.EventHandler(this.debugToolStripMenuItem_Click);
            // 
            // unpackedReleaseToolStripMenuItem
            // 
            this.unpackedReleaseToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.unpackedReleaseToolStripMenuItem.Name = "unpackedReleaseToolStripMenuItem";
            this.unpackedReleaseToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.unpackedReleaseToolStripMenuItem.Text = "&Unpacked release";
            this.unpackedReleaseToolStripMenuItem.Click += new System.EventHandler(this.unpackedReleaseToolStripMenuItem_Click);
            // 
            // packedReleasefastToolStripMenuItem
            // 
            this.packedReleasefastToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.packedReleasefastToolStripMenuItem.Name = "packedReleasefastToolStripMenuItem";
            this.packedReleasefastToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.packedReleasefastToolStripMenuItem.Text = "&Packed release (fast)";
            this.packedReleasefastToolStripMenuItem.Click += new System.EventHandler(this.packedReleasefastToolStripMenuItem_Click);
            // 
            // packedReleaseslowToolStripMenuItem
            // 
            this.packedReleaseslowToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.packedReleaseslowToolStripMenuItem.Name = "packedReleaseslowToolStripMenuItem";
            this.packedReleaseslowToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.packedReleaseslowToolStripMenuItem.Text = "Packed release (&slow)";
            this.packedReleaseslowToolStripMenuItem.Click += new System.EventHandler(this.packedReleaseslowToolStripMenuItem_Click);
            // 
            // cleanToolStripMenuItem
            // 
            this.cleanToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.cleanToolStripMenuItem.Name = "cleanToolStripMenuItem";
            this.cleanToolStripMenuItem.RightToLeftAutoMirrorImage = true;
            this.cleanToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.cleanToolStripMenuItem.Text = "&Clean build files";
            this.cleanToolStripMenuItem.Click += new System.EventHandler(this.cleanToolStripMenuItem_Click_1);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(184, 6);
            // 
            // rUNToolStripMenuItem
            // 
            this.rUNToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.rUNToolStripMenuItem.Name = "rUNToolStripMenuItem";
            this.rUNToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.rUNToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.rUNToolStripMenuItem.Text = "&Run last build";
            this.rUNToolStripMenuItem.Click += new System.EventHandler(this.rUNToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetCameraToolStripMenuItem,
            this.minifyShadersToolStripMenuItem,
            this.colourHelperToolStripMenuItem,
            this.screenshotToolStripMenuItem});
            this.toolsToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // resetCameraToolStripMenuItem
            // 
            this.resetCameraToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.resetCameraToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
            this.resetCameraToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.resetCameraToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.resetCameraToolStripMenuItem.Text = "&Reset camera";
            this.resetCameraToolStripMenuItem.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click);
            // 
            // minifyShadersToolStripMenuItem
            // 
            this.minifyShadersToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.minifyShadersToolStripMenuItem.Name = "minifyShadersToolStripMenuItem";
            this.minifyShadersToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.minifyShadersToolStripMenuItem.Text = "&Minify shaders";
            this.minifyShadersToolStripMenuItem.Click += new System.EventHandler(this.minifyShadersToolStripMenuItem_Click);
            // 
            // colourHelperToolStripMenuItem
            // 
            this.colourHelperToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.colourHelperToolStripMenuItem.Name = "colourHelperToolStripMenuItem";
            this.colourHelperToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.colourHelperToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.colourHelperToolStripMenuItem.Text = "&Colour helper...";
            this.colourHelperToolStripMenuItem.ToolTipText = "Pro tip: Select a vec3 in your GLSL and hit Ctrl+G to edit it as a colour!";
            this.colourHelperToolStripMenuItem.Click += new System.EventHandler(this.colourHelperToolStripMenuItem_Click);
            // 
            // screenshotToolStripMenuItem
            // 
            this.screenshotToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.screenshotToolStripMenuItem.Name = "screenshotToolStripMenuItem";
            this.screenshotToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.screenshotToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.screenshotToolStripMenuItem.Text = "&Screenshot";
            this.screenshotToolStripMenuItem.Click += new System.EventHandler(this.screenshotToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableMultithread4klangInProdToolStripMenuItem,
            this.useExtraPPShaderToolStripMenuItem,
            this.loopTrackToolStripMenuItem,
            this.fullscreenToolStripMenuItem,
            this.showLinenumbersToolStripMenuItem,
            this.showToolbarToolStripMenuItem,
            this.toolStripSeparator2,
            this.setVisualStudioDevCommandpromptLocationToolStripMenuItem});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // enableMultithread4klangInProdToolStripMenuItem
            // 
            this.enableMultithread4klangInProdToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.enableMultithread4klangInProdToolStripMenuItem.Name = "enableMultithread4klangInProdToolStripMenuItem";
            this.enableMultithread4klangInProdToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.enableMultithread4klangInProdToolStripMenuItem.Text = "Enable &multithreaded 4klang in prod";
            this.enableMultithread4klangInProdToolStripMenuItem.ToolTipText = "Saves ~50 bytes in final prod, but requires multicore. Disable for precalc music." +
    "";
            this.enableMultithread4klangInProdToolStripMenuItem.Click += new System.EventHandler(this.enableMultithread4klangInProdToolStripMenuItem_Click);
            // 
            // useExtraPPShaderToolStripMenuItem
            // 
            this.useExtraPPShaderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.useExtraPPShaderToolStripMenuItem.Name = "useExtraPPShaderToolStripMenuItem";
            this.useExtraPPShaderToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.useExtraPPShaderToolStripMenuItem.Text = "Enable &postprocessing shader";
            this.useExtraPPShaderToolStripMenuItem.Click += new System.EventHandler(this.useExtraPPShaderToolStripMenuItem_Click);
            // 
            // loopTrackToolStripMenuItem
            // 
            this.loopTrackToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.loopTrackToolStripMenuItem.Name = "loopTrackToolStripMenuItem";
            this.loopTrackToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.loopTrackToolStripMenuItem.Text = "&Loop prod";
            this.loopTrackToolStripMenuItem.Click += new System.EventHandler(this.loopTrackToolStripMenuItem_Click);
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.fullscreenToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.fullscreenToolStripMenuItem.Text = "&Fullscreen";
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // showLinenumbersToolStripMenuItem
            // 
            this.showLinenumbersToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.showLinenumbersToolStripMenuItem.Name = "showLinenumbersToolStripMenuItem";
            this.showLinenumbersToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.showLinenumbersToolStripMenuItem.Text = "Show line &numbers";
            this.showLinenumbersToolStripMenuItem.Click += new System.EventHandler(this.showLinenumbersToolStripMenuItem_Click);
            // 
            // showToolbarToolStripMenuItem
            // 
            this.showToolbarToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.showToolbarToolStripMenuItem.Name = "showToolbarToolStripMenuItem";
            this.showToolbarToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.showToolbarToolStripMenuItem.Text = "Show toolbar";
            this.showToolbarToolStripMenuItem.Click += new System.EventHandler(this.showToolbarToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.White;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(263, 6);
            // 
            // setVisualStudioDevCommandpromptLocationToolStripMenuItem
            // 
            this.setVisualStudioDevCommandpromptLocationToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.setVisualStudioDevCommandpromptLocationToolStripMenuItem.Name = "setVisualStudioDevCommandpromptLocationToolStripMenuItem";
            this.setVisualStudioDevCommandpromptLocationToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.setVisualStudioDevCommandpromptLocationToolStripMenuItem.Text = "&Set VS dev prompt location...";
            this.setVisualStudioDevCommandpromptLocationToolStripMenuItem.Click += new System.EventHandler(this.setVisualStudioDevCommandpromptLocationToolStripMenuItem_Click);
            // 
            // elpToolStripMenuItem
            // 
            this.elpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.elpToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.elpToolStripMenuItem.Name = "elpToolStripMenuItem";
            this.elpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.elpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // removeUniform
            // 
            this.removeUniform.Location = new System.Drawing.Point(0, 0);
            this.removeUniform.Name = "removeUniform";
            this.removeUniform.Size = new System.Drawing.Size(75, 23);
            this.removeUniform.TabIndex = 0;
            // 
            // listBoxUniforms
            // 
            this.listBoxUniforms.Location = new System.Drawing.Point(0, 0);
            this.listBoxUniforms.Name = "listBoxUniforms";
            this.listBoxUniforms.Size = new System.Drawing.Size(120, 95);
            this.listBoxUniforms.TabIndex = 0;
            // 
            // splitMainLR
            // 
            this.splitMainLR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitMainLR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitMainLR.Location = new System.Drawing.Point(0, 27);
            this.splitMainLR.Margin = new System.Windows.Forms.Padding(0);
            this.splitMainLR.Name = "splitMainLR";
            // 
            // splitMainLR.Panel1
            // 
            this.splitMainLR.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitMainLR.Panel1.Controls.Add(this.splitLHS);
            this.splitMainLR.Panel1MinSize = 0;
            // 
            // splitMainLR.Panel2
            // 
            this.splitMainLR.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitMainLR.Panel2.Controls.Add(this.splitRHS);
            this.splitMainLR.Panel2MinSize = 0;
            this.splitMainLR.Size = new System.Drawing.Size(943, 437);
            this.splitMainLR.SplitterDistance = 291;
            this.splitMainLR.SplitterWidth = 2;
            this.splitMainLR.TabIndex = 8;
            // 
            // splitLHS
            // 
            this.splitLHS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitLHS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLHS.Location = new System.Drawing.Point(0, 0);
            this.splitLHS.Margin = new System.Windows.Forms.Padding(0);
            this.splitLHS.Name = "splitLHS";
            this.splitLHS.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitLHS.Panel1
            // 
            this.splitLHS.Panel1.Controls.Add(this.pnlCam);
            this.splitLHS.Panel1.Controls.Add(this.preview);
            this.splitLHS.Panel1.Text = "CustomTrackBar Enabled";
            // 
            // splitLHS.Panel2
            // 
            this.splitLHS.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitLHS.Panel2.Controls.Add(this.btnClearLog);
            this.splitLHS.Panel2.Controls.Add(this.log);
            this.splitLHS.Size = new System.Drawing.Size(291, 437);
            this.splitLHS.SplitterDistance = 312;
            this.splitLHS.SplitterWidth = 7;
            this.splitLHS.TabIndex = 0;
            this.splitLHS.TabStop = false;
            // 
            // pnlCam
            // 
            this.pnlCam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCam.BackColor = System.Drawing.Color.Transparent;
            this.pnlCam.Controls.Add(this.walkButton);
            this.pnlCam.Controls.Add(this.stepUpButton);
            this.pnlCam.Controls.Add(this.btnCamReset);
            this.pnlCam.Controls.Add(this.freeFlyButton);
            this.pnlCam.Controls.Add(this.camAutoButton);
            this.pnlCam.Controls.Add(this.stepDownButton);
            this.pnlCam.Controls.Add(this.lockFlyButton);
            this.pnlCam.Location = new System.Drawing.Point(149, 292);
            this.pnlCam.Name = "pnlCam";
            this.pnlCam.Size = new System.Drawing.Size(142, 20);
            this.pnlCam.TabIndex = 2;
            // 
            // walkButton
            // 
            this.walkButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.walkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.walkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.walkButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.walkButton.Image = global::kampfpanzerin.Properties.Resources.Walk;
            this.walkButton.Location = new System.Drawing.Point(0, 0);
            this.walkButton.Name = "walkButton";
            this.walkButton.Size = new System.Drawing.Size(20, 20);
            this.walkButton.TabIndex = 0;
            this.walkButton.TabStop = false;
            this.toolTip1.SetToolTip(this.walkButton, "Walk");
            this.walkButton.UseVisualStyleBackColor = false;
            this.walkButton.Click += new System.EventHandler(this.walkButton_Click);
            // 
            // stepUpButton
            // 
            this.stepUpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.stepUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stepUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.stepUpButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.stepUpButton.Image = global::kampfpanzerin.Properties.Resources.Faster;
            this.stepUpButton.Location = new System.Drawing.Point(120, 0);
            this.stepUpButton.Name = "stepUpButton";
            this.stepUpButton.Size = new System.Drawing.Size(22, 20);
            this.stepUpButton.TabIndex = 0;
            this.stepUpButton.TabStop = false;
            this.toolTip1.SetToolTip(this.stepUpButton, "Faster");
            this.stepUpButton.UseVisualStyleBackColor = false;
            this.stepUpButton.Click += new System.EventHandler(this.stepUpButton_Click);
            // 
            // btnCamReset
            // 
            this.btnCamReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCamReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 3.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCamReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCamReset.Image = global::kampfpanzerin.Properties.Resources.Home;
            this.btnCamReset.Location = new System.Drawing.Point(100, 0);
            this.btnCamReset.Name = "btnCamReset";
            this.btnCamReset.Size = new System.Drawing.Size(20, 20);
            this.btnCamReset.TabIndex = 0;
            this.btnCamReset.TabStop = false;
            this.toolTip1.SetToolTip(this.btnCamReset, "Reset cam to origin");
            this.btnCamReset.UseVisualStyleBackColor = false;
            this.btnCamReset.Click += new System.EventHandler(this.btnCamReset_Click);
            // 
            // freeFlyButton
            // 
            this.freeFlyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.freeFlyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.freeFlyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.freeFlyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.freeFlyButton.Image = global::kampfpanzerin.Properties.Resources.Freefly;
            this.freeFlyButton.Location = new System.Drawing.Point(40, 0);
            this.freeFlyButton.Name = "freeFlyButton";
            this.freeFlyButton.Size = new System.Drawing.Size(20, 20);
            this.freeFlyButton.TabIndex = 0;
            this.freeFlyButton.TabStop = false;
            this.toolTip1.SetToolTip(this.freeFlyButton, "Fly (full freefly)");
            this.freeFlyButton.UseVisualStyleBackColor = false;
            this.freeFlyButton.Click += new System.EventHandler(this.freeFlyButton_Click);
            // 
            // camAutoButton
            // 
            this.camAutoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.camAutoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.camAutoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.camAutoButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.camAutoButton.Image = global::kampfpanzerin.Properties.Resources.SyncTracker;
            this.camAutoButton.Location = new System.Drawing.Point(60, 0);
            this.camAutoButton.Name = "camAutoButton";
            this.camAutoButton.Size = new System.Drawing.Size(20, 20);
            this.camAutoButton.TabIndex = 0;
            this.camAutoButton.TabStop = false;
            this.toolTip1.SetToolTip(this.camAutoButton, "Use sync tracker");
            this.camAutoButton.UseVisualStyleBackColor = false;
            this.camAutoButton.Click += new System.EventHandler(this.camAutoButton_Click);
            // 
            // stepDownButton
            // 
            this.stepDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.stepDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stepDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.stepDownButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.stepDownButton.Image = global::kampfpanzerin.Properties.Resources.Slower;
            this.stepDownButton.Location = new System.Drawing.Point(80, 0);
            this.stepDownButton.Name = "stepDownButton";
            this.stepDownButton.Size = new System.Drawing.Size(20, 20);
            this.stepDownButton.TabIndex = 0;
            this.stepDownButton.TabStop = false;
            this.toolTip1.SetToolTip(this.stepDownButton, "Slower");
            this.stepDownButton.UseVisualStyleBackColor = false;
            this.stepDownButton.Click += new System.EventHandler(this.stepDownButton_Click);
            // 
            // lockFlyButton
            // 
            this.lockFlyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lockFlyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lockFlyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.lockFlyButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lockFlyButton.Image = global::kampfpanzerin.Properties.Resources.Lockfly;
            this.lockFlyButton.Location = new System.Drawing.Point(20, 0);
            this.lockFlyButton.Name = "lockFlyButton";
            this.lockFlyButton.Size = new System.Drawing.Size(20, 20);
            this.lockFlyButton.TabIndex = 0;
            this.lockFlyButton.TabStop = false;
            this.toolTip1.SetToolTip(this.lockFlyButton, "Fly (Locked y)");
            this.lockFlyButton.UseVisualStyleBackColor = false;
            this.lockFlyButton.Click += new System.EventHandler(this.lockFlyButton_Click);
            // 
            // preview
            // 
            this.preview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.preview.Location = new System.Drawing.Point(0, 0);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(291, 312);
            this.preview.TabIndex = 0;
            this.preview.TabStop = false;
            this.preview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.preview_MouseClick);
            this.preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.preview_MouseDown);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnClearLog.Image = global::kampfpanzerin.Properties.Resources.Clear;
            this.btnClearLog.Location = new System.Drawing.Point(257, -1);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(22, 22);
            this.btnClearLog.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnClearLog, "Clear log");
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // splitRHS
            // 
            this.splitRHS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRHS.Location = new System.Drawing.Point(0, 0);
            this.splitRHS.Margin = new System.Windows.Forms.Padding(0);
            this.splitRHS.Name = "splitRHS";
            this.splitRHS.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitRHS.Panel1
            // 
            this.splitRHS.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitRHS.Panel1.Controls.Add(this.tabControl1);
            this.splitRHS.Panel1MinSize = 0;
            // 
            // splitRHS.Panel2
            // 
            this.splitRHS.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitRHS.Panel2.Controls.Add(this.timeLine);
            this.splitRHS.Panel2MinSize = 0;
            this.splitRHS.Size = new System.Drawing.Size(650, 437);
            this.splitRHS.SplitterDistance = 189;
            this.splitRHS.SplitterWidth = 1;
            this.splitRHS.TabIndex = 5;
            this.splitRHS.TabStop = false;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlToolbar.Controls.Add(this.pictureBox3);
            this.pnlToolbar.Controls.Add(this.pictureBox2);
            this.pnlToolbar.Controls.Add(this.pictureBox5);
            this.pnlToolbar.Controls.Add(this.pictureBox4);
            this.pnlToolbar.Controls.Add(this.pictureBox1);
            this.pnlToolbar.Controls.Add(this.btnScreenshot);
            this.pnlToolbar.Controls.Add(this.btnColourHelper);
            this.pnlToolbar.Controls.Add(this.btnFind);
            this.pnlToolbar.Controls.Add(this.btnRedo);
            this.pnlToolbar.Controls.Add(this.btnUndo);
            this.pnlToolbar.Controls.Add(this.btnPull);
            this.pnlToolbar.Controls.Add(this.btnPush);
            this.pnlToolbar.Controls.Add(this.btnCleanProj);
            this.pnlToolbar.Controls.Add(this.btnGotoProjectFolder);
            this.pnlToolbar.Controls.Add(this.btnExportProj);
            this.pnlToolbar.Controls.Add(this.btnSaveAll);
            this.pnlToolbar.Controls.Add(this.btnOpenProj);
            this.pnlToolbar.Controls.Add(this.btnNew);
            this.pnlToolbar.Controls.Add(this.btnCleanBuild);
            this.pnlToolbar.Controls.Add(this.btnRun);
            this.pnlToolbar.Controls.Add(this.btnRenderMusic);
            this.pnlToolbar.Controls.Add(this.btnBuild);
            this.pnlToolbar.Controls.Add(this.btnLineNumbers);
            this.pnlToolbar.Controls.Add(this.btnTracker);
            this.pnlToolbar.Controls.Add(this.btnEnvelopes);
            this.pnlToolbar.Controls.Add(this.btnStandardUniforms);
            this.pnlToolbar.Controls.Add(this.btnCamToggle);
            this.pnlToolbar.Controls.Add(this.btnFullscreen);
            this.pnlToolbar.Controls.Add(this.btnLoop);
            this.pnlToolbar.Controls.Add(this.btnRefreshShaders);
            this.pnlToolbar.Location = new System.Drawing.Point(334, 2);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(603, 23);
            this.pnlToolbar.TabIndex = 4;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.Image = global::kampfpanzerin.Properties.Resources.Seperator;
            this.pictureBox3.Location = new System.Drawing.Point(483, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(20, 20);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::kampfpanzerin.Properties.Resources.Seperator;
            this.pictureBox2.Location = new System.Drawing.Point(403, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox5.Image = global::kampfpanzerin.Properties.Resources.Seperator;
            this.pictureBox5.Location = new System.Drawing.Point(123, 0);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(20, 20);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox5.TabIndex = 5;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.Image = global::kampfpanzerin.Properties.Resources.Seperator;
            this.pictureBox4.Location = new System.Drawing.Point(183, 0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(20, 20);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 5;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::kampfpanzerin.Properties.Resources.Seperator;
            this.pictureBox1.Location = new System.Drawing.Point(283, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // btnScreenshot
            // 
            this.btnScreenshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScreenshot.FlatAppearance.BorderSize = 0;
            this.btnScreenshot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScreenshot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnScreenshot.Image = global::kampfpanzerin.Properties.Resources.Screenshot;
            this.btnScreenshot.Location = new System.Drawing.Point(383, 0);
            this.btnScreenshot.Name = "btnScreenshot";
            this.btnScreenshot.Size = new System.Drawing.Size(20, 20);
            this.btnScreenshot.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnScreenshot, "Screenshot");
            this.btnScreenshot.UseVisualStyleBackColor = true;
            this.btnScreenshot.Click += new System.EventHandler(this.btnScreenshot_Click);
            // 
            // btnColourHelper
            // 
            this.btnColourHelper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColourHelper.FlatAppearance.BorderSize = 0;
            this.btnColourHelper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColourHelper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnColourHelper.Image = global::kampfpanzerin.Properties.Resources.ColourHelper;
            this.btnColourHelper.Location = new System.Drawing.Point(363, 0);
            this.btnColourHelper.Name = "btnColourHelper";
            this.btnColourHelper.Size = new System.Drawing.Size(20, 20);
            this.btnColourHelper.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnColourHelper, "Colour helper (Pro tip: try highlighting a vec3 first!)");
            this.btnColourHelper.UseVisualStyleBackColor = true;
            this.btnColourHelper.Click += new System.EventHandler(this.btnColourHelper_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.FlatAppearance.BorderSize = 0;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFind.Image = global::kampfpanzerin.Properties.Resources.Find;
            this.btnFind.Location = new System.Drawing.Point(343, 0);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(20, 20);
            this.btnFind.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnFind, "Find/replace");
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedo.FlatAppearance.BorderSize = 0;
            this.btnRedo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRedo.Image = global::kampfpanzerin.Properties.Resources.Redo;
            this.btnRedo.Location = new System.Drawing.Point(323, 0);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(20, 20);
            this.btnRedo.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRedo, "Redo");
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.FlatAppearance.BorderSize = 0;
            this.btnUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnUndo.Image = global::kampfpanzerin.Properties.Resources.Undo;
            this.btnUndo.Location = new System.Drawing.Point(303, 0);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(20, 20);
            this.btnUndo.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnUndo, "Undo");
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnPull
            // 
            this.btnPull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPull.FlatAppearance.BorderSize = 0;
            this.btnPull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPull.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPull.Image = global::kampfpanzerin.Properties.Resources.bitbucket_pull;
            this.btnPull.Location = new System.Drawing.Point(143, 0);
            this.btnPull.Name = "btnPull";
            this.btnPull.Size = new System.Drawing.Size(20, 20);
            this.btnPull.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnPull, "Pull from BitBucket");
            this.btnPull.UseVisualStyleBackColor = true;
            this.btnPull.Click += new System.EventHandler(this.btnPull_Click);
            // 
            // btnPush
            // 
            this.btnPush.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPush.FlatAppearance.BorderSize = 0;
            this.btnPush.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPush.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPush.Image = global::kampfpanzerin.Properties.Resources.bitbucket_push;
            this.btnPush.Location = new System.Drawing.Point(163, 0);
            this.btnPush.Name = "btnPush";
            this.btnPush.Size = new System.Drawing.Size(20, 20);
            this.btnPush.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnPush, "Push to BitBucket");
            this.btnPush.UseVisualStyleBackColor = true;
            this.btnPush.Click += new System.EventHandler(this.btnPush_Click);
            // 
            // btnCleanProj
            // 
            this.btnCleanProj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanProj.FlatAppearance.BorderSize = 0;
            this.btnCleanProj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCleanProj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCleanProj.Image = global::kampfpanzerin.Properties.Resources.CleanProject;
            this.btnCleanProj.Location = new System.Drawing.Point(103, 0);
            this.btnCleanProj.Name = "btnCleanProj";
            this.btnCleanProj.Size = new System.Drawing.Size(20, 20);
            this.btnCleanProj.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnCleanProj, "Clean project");
            this.btnCleanProj.UseVisualStyleBackColor = true;
            this.btnCleanProj.Click += new System.EventHandler(this.btnCleanProj_Click);
            // 
            // btnGotoProjectFolder
            // 
            this.btnGotoProjectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGotoProjectFolder.FlatAppearance.BorderSize = 0;
            this.btnGotoProjectFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGotoProjectFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnGotoProjectFolder.Image = global::kampfpanzerin.Properties.Resources.GotoFolder;
            this.btnGotoProjectFolder.Location = new System.Drawing.Point(63, 0);
            this.btnGotoProjectFolder.Name = "btnGotoProjectFolder";
            this.btnGotoProjectFolder.Size = new System.Drawing.Size(20, 20);
            this.btnGotoProjectFolder.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnGotoProjectFolder, "Open project folder");
            this.btnGotoProjectFolder.UseVisualStyleBackColor = true;
            this.btnGotoProjectFolder.Click += new System.EventHandler(this.btnGotoProjectFolder_Click);
            // 
            // btnExportProj
            // 
            this.btnExportProj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportProj.FlatAppearance.BorderSize = 0;
            this.btnExportProj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportProj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExportProj.Image = global::kampfpanzerin.Properties.Resources.ExportProj;
            this.btnExportProj.Location = new System.Drawing.Point(83, 0);
            this.btnExportProj.Name = "btnExportProj";
            this.btnExportProj.Size = new System.Drawing.Size(20, 20);
            this.btnExportProj.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnExportProj, "Export project to Visual Studio");
            this.btnExportProj.UseVisualStyleBackColor = true;
            this.btnExportProj.Click += new System.EventHandler(this.btnExportProj_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAll.FlatAppearance.BorderSize = 0;
            this.btnSaveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSaveAll.Image = global::kampfpanzerin.Properties.Resources.SaveAll;
            this.btnSaveAll.Location = new System.Drawing.Point(43, 0);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(20, 20);
            this.btnSaveAll.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnSaveAll, "Save all");
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // btnOpenProj
            // 
            this.btnOpenProj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenProj.FlatAppearance.BorderSize = 0;
            this.btnOpenProj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenProj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnOpenProj.Image = global::kampfpanzerin.Properties.Resources.Open;
            this.btnOpenProj.Location = new System.Drawing.Point(23, 0);
            this.btnOpenProj.Name = "btnOpenProj";
            this.btnOpenProj.Size = new System.Drawing.Size(20, 20);
            this.btnOpenProj.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnOpenProj, "Open project");
            this.btnOpenProj.UseVisualStyleBackColor = true;
            this.btnOpenProj.Click += new System.EventHandler(this.btnOpenProj_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnNew.Image = global::kampfpanzerin.Properties.Resources.New;
            this.btnNew.Location = new System.Drawing.Point(3, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(20, 20);
            this.btnNew.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnNew, "Create project");
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCleanBuild
            // 
            this.btnCleanBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanBuild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCleanBuild.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCleanBuild.Image = global::kampfpanzerin.Properties.Resources.CleanBuild;
            this.btnCleanBuild.Location = new System.Drawing.Point(583, 0);
            this.btnCleanBuild.Name = "btnCleanBuild";
            this.btnCleanBuild.Size = new System.Drawing.Size(20, 20);
            this.btnCleanBuild.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnCleanBuild, "Clean build files");
            this.btnCleanBuild.UseVisualStyleBackColor = true;
            this.btnCleanBuild.Click += new System.EventHandler(this.btnCleanBuild_Click);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRun.Image = global::kampfpanzerin.Properties.Resources.Run;
            this.btnRun.Location = new System.Drawing.Point(563, 0);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(20, 20);
            this.btnRun.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRun, "Run last build");
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnRenderMusic
            // 
            this.btnRenderMusic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenderMusic.FlatAppearance.BorderSize = 0;
            this.btnRenderMusic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenderMusic.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRenderMusic.Image = global::kampfpanzerin.Properties.Resources.BuildAudio;
            this.btnRenderMusic.Location = new System.Drawing.Point(523, 0);
            this.btnRenderMusic.Name = "btnRenderMusic";
            this.btnRenderMusic.Size = new System.Drawing.Size(20, 20);
            this.btnRenderMusic.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRenderMusic, "Rebuild music");
            this.btnRenderMusic.UseVisualStyleBackColor = true;
            this.btnRenderMusic.Click += new System.EventHandler(this.btnRenderMusic_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuild.FlatAppearance.BorderSize = 0;
            this.btnBuild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuild.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnBuild.Image = global::kampfpanzerin.Properties.Resources.BuildProd;
            this.btnBuild.Location = new System.Drawing.Point(543, 0);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(20, 20);
            this.btnBuild.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnBuild, "Build prod (Debug build)");
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnLineNumbers
            // 
            this.btnLineNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLineNumbers.FlatAppearance.BorderSize = 0;
            this.btnLineNumbers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLineNumbers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnLineNumbers.Image = global::kampfpanzerin.Properties.Resources.LineNumbers;
            this.btnLineNumbers.Location = new System.Drawing.Point(463, 0);
            this.btnLineNumbers.Name = "btnLineNumbers";
            this.btnLineNumbers.Size = new System.Drawing.Size(20, 20);
            this.btnLineNumbers.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnLineNumbers, "Show line numbers");
            this.btnLineNumbers.UseVisualStyleBackColor = true;
            this.btnLineNumbers.Click += new System.EventHandler(this.btnLineNumbers_Click);
            // 
            // btnTracker
            // 
            this.btnTracker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTracker.FlatAppearance.BorderSize = 0;
            this.btnTracker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTracker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnTracker.Image = global::kampfpanzerin.Properties.Resources.SyncTracker;
            this.btnTracker.Location = new System.Drawing.Point(263, 0);
            this.btnTracker.Name = "btnTracker";
            this.btnTracker.Size = new System.Drawing.Size(20, 20);
            this.btnTracker.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnTracker, "Enable sync tracker and uniforms");
            this.btnTracker.UseVisualStyleBackColor = true;
            this.btnTracker.Click += new System.EventHandler(this.btnTracker_Click);
            // 
            // btnEnvelopes
            // 
            this.btnEnvelopes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnvelopes.FlatAppearance.BorderSize = 0;
            this.btnEnvelopes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnvelopes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEnvelopes.Image = global::kampfpanzerin.Properties.Resources.Envelopes;
            this.btnEnvelopes.Location = new System.Drawing.Point(243, 0);
            this.btnEnvelopes.Name = "btnEnvelopes";
            this.btnEnvelopes.Size = new System.Drawing.Size(20, 20);
            this.btnEnvelopes.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnEnvelopes, "Enable 4klang envelopes in float array ev if available");
            this.btnEnvelopes.UseVisualStyleBackColor = true;
            this.btnEnvelopes.Click += new System.EventHandler(this.btnEnvelopes_Click);
            // 
            // btnStandardUniforms
            // 
            this.btnStandardUniforms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStandardUniforms.FlatAppearance.BorderSize = 0;
            this.btnStandardUniforms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStandardUniforms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStandardUniforms.Image = global::kampfpanzerin.Properties.Resources.StandardUniforms;
            this.btnStandardUniforms.Location = new System.Drawing.Point(203, 0);
            this.btnStandardUniforms.Name = "btnStandardUniforms";
            this.btnStandardUniforms.Size = new System.Drawing.Size(20, 20);
            this.btnStandardUniforms.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnStandardUniforms, "Enable standard uniforms");
            this.btnStandardUniforms.UseVisualStyleBackColor = true;
            this.btnStandardUniforms.Click += new System.EventHandler(this.btnStandardUniforms_Click);
            // 
            // btnCamToggle
            // 
            this.btnCamToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCamToggle.FlatAppearance.BorderSize = 0;
            this.btnCamToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCamToggle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCamToggle.Image = global::kampfpanzerin.Properties.Resources.Camera;
            this.btnCamToggle.Location = new System.Drawing.Point(223, 0);
            this.btnCamToggle.Name = "btnCamToggle";
            this.btnCamToggle.Size = new System.Drawing.Size(20, 20);
            this.btnCamToggle.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnCamToggle, "Enable camera system and uniforms");
            this.btnCamToggle.UseVisualStyleBackColor = true;
            this.btnCamToggle.Click += new System.EventHandler(this.btnCamToggle_Click);
            // 
            // btnFullscreen
            // 
            this.btnFullscreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFullscreen.FlatAppearance.BorderSize = 0;
            this.btnFullscreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFullscreen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFullscreen.Image = ((System.Drawing.Image)(resources.GetObject("btnFullscreen.Image")));
            this.btnFullscreen.Location = new System.Drawing.Point(423, 0);
            this.btnFullscreen.Name = "btnFullscreen";
            this.btnFullscreen.Size = new System.Drawing.Size(20, 20);
            this.btnFullscreen.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnFullscreen, "Fullscreen");
            this.btnFullscreen.UseVisualStyleBackColor = true;
            this.btnFullscreen.Click += new System.EventHandler(this.btnFullscreen_Click);
            // 
            // btnLoop
            // 
            this.btnLoop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoop.FlatAppearance.BorderSize = 0;
            this.btnLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnLoop.Image = global::kampfpanzerin.Properties.Resources.Loop;
            this.btnLoop.Location = new System.Drawing.Point(443, 0);
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.Size = new System.Drawing.Size(20, 20);
            this.btnLoop.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnLoop, "Loop prod");
            this.btnLoop.UseVisualStyleBackColor = true;
            this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
            // 
            // btnRefreshShaders
            // 
            this.btnRefreshShaders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshShaders.FlatAppearance.BorderSize = 0;
            this.btnRefreshShaders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshShaders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRefreshShaders.Image = global::kampfpanzerin.Properties.Resources.Refresh;
            this.btnRefreshShaders.Location = new System.Drawing.Point(503, 0);
            this.btnRefreshShaders.Name = "btnRefreshShaders";
            this.btnRefreshShaders.Size = new System.Drawing.Size(20, 20);
            this.btnRefreshShaders.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRefreshShaders, "Rebuild shaders");
            this.btnRefreshShaders.UseVisualStyleBackColor = true;
            this.btnRefreshShaders.Click += new System.EventHandler(this.btnRefreshShaders_Click);
            // 
            // pullFromBitBucketToolStripMenuItem
            // 
            this.pullFromBitBucketToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.pullFromBitBucketToolStripMenuItem.Name = "pullFromBitBucketToolStripMenuItem";
            this.pullFromBitBucketToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.pullFromBitBucketToolStripMenuItem.Text = "Pull from Bitbucket";
            this.pullFromBitBucketToolStripMenuItem.Click += new System.EventHandler(this.pullFromBitBucketToolStripMenuItem_Click);
            // 
            // pushToBitbucketToolStripMenuItem
            // 
            this.pushToBitbucketToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.pushToBitbucketToolStripMenuItem.Name = "pushToBitbucketToolStripMenuItem";
            this.pushToBitbucketToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.pushToBitbucketToolStripMenuItem.Text = "Push to Bitbucket";
            this.pushToBitbucketToolStripMenuItem.Click += new System.EventHandler(this.pushToBitbucketToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(189, 6);
            // 
            // klangPlayer
            // 
            this.klangPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.klangPlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.klangPlayer.Location = new System.Drawing.Point(0, 464);
            this.klangPlayer.Name = "klangPlayer";
            this.klangPlayer.Size = new System.Drawing.Size(943, 62);
            this.klangPlayer.TabIndex = 9;
            // 
            // log
            // 
            this.log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.log.Font = new System.Drawing.Font("Lucida Console", 9F);
            this.log.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.log.Location = new System.Drawing.Point(0, 0);
            this.log.Margin = new System.Windows.Forms.Padding(0);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(291, 116);
            this.log.TabIndex = 4;
            this.log.TextChanged += new System.EventHandler(this.log_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPP);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(650, 189);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage1.Controls.Add(this.edVert);
            this.tabPage1.ForeColor = System.Drawing.Color.White;
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(642, 160);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Vertex shader";
            // 
            // edVert
            // 
            this.edVert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edVert.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.edVert.ConfigurationManager.Language = "cpp";
            this.edVert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edVert.ForeColor = System.Drawing.Color.White;
            this.edVert.Indentation.SmartIndentType = ScintillaNET.SmartIndent.Simple;
            this.edVert.Indentation.TabWidth = 4;
            this.edVert.IsBraceMatching = true;
            this.edVert.Location = new System.Drawing.Point(0, 0);
            this.edVert.Margin = new System.Windows.Forms.Padding(0);
            this.edVert.Margins.Margin0.Width = 25;
            this.edVert.Name = "edVert";
            this.edVert.Size = new System.Drawing.Size(642, 160);
            this.edVert.Styles.BraceBad.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.BraceLight.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.CallTip.FontName = "Segoe UI\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.ControlChar.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.Default.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edVert.Styles.Default.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.Default.ForeColor = System.Drawing.Color.White;
            this.edVert.Styles.IndentGuide.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.LastPredefined.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.LineNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edVert.Styles.LineNumber.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.Styles.LineNumber.ForeColor = System.Drawing.Color.DarkGray;
            this.edVert.Styles.Max.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edVert.TabIndex = 0;
            this.edVert.TextChanged += new System.EventHandler(this.edVert_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage2.Controls.Add(this.edFrag);
            this.tabPage2.ForeColor = System.Drawing.Color.White;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(642, 160);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Scene frag shader";
            // 
            // edFrag
            // 
            this.edFrag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edFrag.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.edFrag.ConfigurationManager.Language = "cpp";
            this.edFrag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edFrag.ForeColor = System.Drawing.Color.White;
            this.edFrag.Indentation.SmartIndentType = ScintillaNET.SmartIndent.Simple;
            this.edFrag.Indentation.TabWidth = 4;
            this.edFrag.IsBraceMatching = true;
            this.edFrag.Location = new System.Drawing.Point(0, 0);
            this.edFrag.Margin = new System.Windows.Forms.Padding(0);
            this.edFrag.Margins.Margin0.Width = 25;
            this.edFrag.Name = "edFrag";
            this.edFrag.Size = new System.Drawing.Size(642, 160);
            this.edFrag.Styles.BraceBad.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.BraceLight.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.CallTip.FontName = "Segoe UI\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.ControlChar.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.Default.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edFrag.Styles.Default.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.Default.ForeColor = System.Drawing.Color.White;
            this.edFrag.Styles.IndentGuide.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.LastPredefined.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.LineNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edFrag.Styles.LineNumber.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.Styles.LineNumber.ForeColor = System.Drawing.Color.Black;
            this.edFrag.Styles.Max.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edFrag.TabIndex = 1;
            this.edFrag.TextChanged += new System.EventHandler(this.edFrag_TextChanged);
            // 
            // tabPP
            // 
            this.tabPP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPP.Controls.Add(this.edPost);
            this.tabPP.ForeColor = System.Drawing.Color.White;
            this.tabPP.Location = new System.Drawing.Point(4, 25);
            this.tabPP.Margin = new System.Windows.Forms.Padding(0);
            this.tabPP.Name = "tabPP";
            this.tabPP.Size = new System.Drawing.Size(642, 160);
            this.tabPP.TabIndex = 2;
            this.tabPP.Text = "Postprocessing frag shader";
            // 
            // edPost
            // 
            this.edPost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edPost.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.edPost.ConfigurationManager.Language = "cpp";
            this.edPost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edPost.ForeColor = System.Drawing.Color.White;
            this.edPost.Indentation.SmartIndentType = ScintillaNET.SmartIndent.Simple;
            this.edPost.Indentation.TabWidth = 4;
            this.edPost.IsBraceMatching = true;
            this.edPost.Location = new System.Drawing.Point(0, 0);
            this.edPost.Margin = new System.Windows.Forms.Padding(0);
            this.edPost.Margins.FoldMarginColor = System.Drawing.Color.Empty;
            this.edPost.Margins.FoldMarginHighlightColor = System.Drawing.Color.Empty;
            this.edPost.Margins.Margin0.Width = 25;
            this.edPost.Name = "edPost";
            this.edPost.Size = new System.Drawing.Size(642, 160);
            this.edPost.Styles.BraceBad.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.BraceLight.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.CallTip.FontName = "Segoe UI\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.ControlChar.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.Default.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edPost.Styles.Default.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.Default.ForeColor = System.Drawing.Color.White;
            this.edPost.Styles.IndentGuide.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.LastPredefined.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.LineNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.edPost.Styles.LineNumber.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.Styles.LineNumber.ForeColor = System.Drawing.Color.DarkGray;
            this.edPost.Styles.Max.FontName = "Verdana\0\0\0\0\0\0\0\0\0\0\0\0\0";
            this.edPost.TabIndex = 0;
            this.edPost.TextChanged += new System.EventHandler(this.edPost_TextChanged);
            // 
            // timeLine
            // 
            this.timeLine.AutoScroll = true;
            this.timeLine.AutoScrollMinSize = new System.Drawing.Size(0, 70);
            this.timeLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.timeLine.camMode = false;
            this.timeLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeLine.Location = new System.Drawing.Point(0, 0);
            this.timeLine.Name = "timeLine";
            this.timeLine.Size = new System.Drawing.Size(650, 247);
            this.timeLine.TabIndex = 2;
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(189, 6);
            // 
            // AppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(943, 527);
            this.Controls.Add(this.klangPlayer);
            this.Controls.Add(this.pnlToolbar);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.splitMainLR);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "AppForm";
            this.Text = "4kampf | TRSI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitMainLR.Panel1.ResumeLayout(false);
            this.splitMainLR.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMainLR)).EndInit();
            this.splitMainLR.ResumeLayout(false);
            this.splitLHS.Panel1.ResumeLayout(false);
            this.splitLHS.Panel2.ResumeLayout(false);
            this.splitLHS.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLHS)).EndInit();
            this.splitLHS.ResumeLayout(false);
            this.pnlCam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            this.splitRHS.Panel1.ResumeLayout(false);
            this.splitRHS.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRHS)).EndInit();
            this.splitRHS.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edVert)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edFrag)).EndInit();
            this.tabPP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edPost)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Button removeUniform;
        private System.Windows.Forms.ListBox listBoxUniforms;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetCameraToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem loopTrackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToVSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minifyShadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setVisualStudioDevCommandpromptLocationToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitMainLR;
        private System.Windows.Forms.SplitContainer splitLHS;
        public System.Windows.Forms.SplitContainer splitRHS;
        public TabControlFlat tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        public ScintillaNET.Scintilla edVert;
        public ScintillaNET.Scintilla edFrag;
        private System.Windows.Forms.ToolStripMenuItem elpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unpackedReleaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packedReleasefastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packedReleaseslowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem rUNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rebuildShadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rerender4klangMuskcToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripMenuItem useExtraPPShaderToolStripMenuItem;
        public System.Windows.Forms.TabPage tabPP;
        public ScintillaNET.Scintilla edPost;
        public System.Windows.Forms.ToolStripMenuItem enableMultithread4klangInProdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uniformsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem enableCamControlToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem enableStandardUniformsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem enableSyncTrackerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenshotToolStripMenuItem;
        public System.Windows.Forms.Panel pnlCam;
        private System.Windows.Forms.Button walkButton;
        private System.Windows.Forms.Button stepUpButton;
        private System.Windows.Forms.Button btnCamReset;
        private System.Windows.Forms.Button freeFlyButton;
        private System.Windows.Forms.Button stepDownButton;
        private System.Windows.Forms.Button lockFlyButton;
        private System.Windows.Forms.Button camAutoButton;
        public System.Windows.Forms.PictureBox preview;
        public TimeLine timeLine;
        private System.Windows.Forms.ToolStripMenuItem colourHelperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanToolStripMenuItem1;
        private System.Windows.Forms.Button btnRefreshShaders;
        public System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnOpenProj;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Button btnFind;
        public System.Windows.Forms.Button btnLoop;
        public System.Windows.Forms.Button btnCamToggle;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnRenderMusic;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button btnFullscreen;
        private System.Windows.Forms.Button btnExportProj;
        private System.Windows.Forms.ToolTip toolTip1;
        private TextBoxWithScrollLeft log;
        private System.Windows.Forms.Button btnColourHelper;
        public System.Windows.Forms.ToolStripMenuItem showLinenumbersToolStripMenuItem;
        public System.Windows.Forms.Button btnLineNumbers;
        public System.Windows.Forms.Button btnTracker;
        public System.Windows.Forms.Button btnEnvelopes;
        private System.Windows.Forms.PictureBox pictureBox4;
        public System.Windows.Forms.ToolStripMenuItem showToolbarToolStripMenuItem;
        private System.Windows.Forms.Button btnCleanProj;
        private System.Windows.Forms.Button btnCleanBuild;
        private System.Windows.Forms.Button btnScreenshot;
        public System.Windows.Forms.Button btnStandardUniforms;
        private System.Windows.Forms.ToolStripMenuItem openProjectLocationToolStripMenuItem;
        private System.Windows.Forms.Button btnGotoProjectFolder;
        public MusicPlayer klangPlayer;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button btnPush;
        private System.Windows.Forms.Button btnPull;
        private System.Windows.Forms.ToolStripMenuItem pullFromBitBucketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pushToBitbucketToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}

