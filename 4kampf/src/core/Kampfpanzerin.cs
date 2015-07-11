using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using Tao.OpenGl;
using System.Xml.Serialization;
using System.Globalization;
using System.Runtime.InteropServices;
using kampfpanzerin.src.core.Compiler;
using kampfpanzerin.git;
using kampfpanzerin.components;
using kampfpanzerin.core.Serialization;

namespace kampfpanzerin
{
    static class Kampfpanzerin {
        private static AppForm form;
        private static string currentProjectDirectory; 
        private static bool projectDirty;
        public static Project project = new Project();
        public static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
        public static GitHandler Repo {
            get;
            private set;
        }


        [STAThread]
        static void Main() {
            NativeLoadHelper.SetupDllPath();
            form = AppForm.GetInstance();
            ApplySettings();
            form.SetFullscreen();
            form.Show();    // Note: Not done in AppForm constructor to prevent SetFullscreen() etc causing ugly resizes

            MessageBoxManager.Yes = "Create";
            MessageBoxManager.No = "Open";
            MessageBoxManager.Register();

            DialogResult d;
            do {
                MessageBoxManager.Cancel = "Quit";
                d = MessageBox.Show("Welcome to 4kampfpanzerin boss!\nShall we create a project or open one?", "4kampfpanzerin", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (d == DialogResult.Yes) {
                    CreateProject();
                } else if (d == DialogResult.No)
                    OpenProject();
                else
                    Application.Exit();
            } while (d != DialogResult.Cancel && currentProjectDirectory == null || currentProjectDirectory == "");

            GraphicsManager gfx = GraphicsManager.GetInstance();
            Application.EnableVisualStyles();
            while (!form.IsDisposed) {
                Application.DoEvents();
                gfx.Render();
                form.FrameUpdate();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static void GoToProjectFolder() {
            if (currentProjectDirectory != "")
                Process.Start(currentProjectDirectory);
        }

        public static void ApplySettings() {
            form.pnlToolbar.Visible = Properties.Settings.Default.showToolBar;
            form.pnlCam.Visible = Properties.Settings.Default.enableCamControls;

            if (Properties.Settings.Default.usePP && form.tabControl1.TabPages.Count < 3)
                form.tabControl1.TabPages.Add(form.tabPP);
            else if (!Properties.Settings.Default.usePP && form.tabControl1.TabPages.Count == 3)
                form.tabControl1.TabPages.Remove(form.tabPP);

            if (Properties.Settings.Default.useSyncTracker && form.splitRHS.Panel2Collapsed)
                form.splitRHS.Panel2Collapsed = false;
            else if (!Properties.Settings.Default.useSyncTracker && !form.splitRHS.Panel2Collapsed)
                form.splitRHS.Panel2Collapsed = true;

            if (Properties.Settings.Default.showLineNumbers) {
                form.edVert.Margins[0].Width = 25;
                form.edFrag.Margins[0].Width = 25;
                form.edPost.Margins[0].Width = 25;
            } else {
                form.edVert.Margins[0].Width = 0;
                form.edFrag.Margins[0].Width = 0;
                form.edPost.Margins[0].Width = 0;
            }

            form.enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem.Checked = project.use4klangEnv;
            form.enableCamControlToolStripMenuItem.Checked = Properties.Settings.Default.enableCamControls;
            form.enableStandardUniformsToolStripMenuItem.Checked = Properties.Settings.Default.enableStandardUniforms;
            form.loopTrackToolStripMenuItem.Checked = Properties.Settings.Default.enableLooping;
            form.fullscreenToolStripMenuItem.Checked = Properties.Settings.Default.fullscreen;
            form.useExtraPPShaderToolStripMenuItem.Checked = Properties.Settings.Default.usePP;
            form.enableMultithread4klangInProdToolStripMenuItem.Checked = Properties.Settings.Default.useSoundThread;
            form.enableSyncTrackerToolStripMenuItem.Checked = Properties.Settings.Default.useSyncTracker;
            form.showLinenumbersToolStripMenuItem.Checked = Properties.Settings.Default.showLineNumbers;
            form.showToolbarToolStripMenuItem.Checked = Properties.Settings.Default.showToolBar;

            form.btnCamToggle.BackColor = Properties.Settings.Default.enableCamControls ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnLoop.BackColor = Properties.Settings.Default.enableLooping ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnFullscreen.BackColor = Properties.Settings.Default.fullscreen ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnLineNumbers.BackColor = Properties.Settings.Default.showLineNumbers ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnTracker.BackColor = Properties.Settings.Default.useSyncTracker ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnEnvelopes.BackColor = project.use4klangEnv ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnStandardUniforms.BackColor = Properties.Settings.Default.enableStandardUniforms ? Color.FromArgb(100, 100, 100) : form.BackColor;

            form.klangPlayer.ApplySettings(project);
            form.timeLine.ApplySettings();

            Properties.Settings.Default.Save();
        }

        public static void ProcessStreamHandler(object sender, DataReceivedEventArgs e) {
            form.ConcatLog(e.Data);
        }

        public static void CreateProject() {
            form.klangPlayer.Stop();

            NewProjectWizard wzd = new NewProjectWizard();
            wzd.StartPosition = FormStartPosition.CenterParent;
            wzd.ShowDialog();
            if (wzd.DialogResult == DialogResult.OK) {
                form.klangPlayer.Unload();

                string dest = wzd.ProjectLocation + "/" + wzd.ProjectName;
                Project p = wzd.Project;
                p.useBitBucket = wzd.UseBitBucket;

                p.useClinkster = wzd.UseClinkster;

                string src = AppDomain.CurrentDomain.BaseDirectory + "skel";
                Utils.CopyFolderContents(src, dest);
                p.syncBars = form.timeLine.syncBars;
                p.camBars = form.timeLine.camBars;

                SaveProjectSettings(p, dest + "/");

                Repo = GitHandler.Init(dest, p);
                string msg = "* Project created! Now drop your ";
                msg += p.useClinkster?"music.asm":"4klang.obj and 4klang.h";
                msg += " in there and run Build->Render Music.\n\n(Or just run Build->Render Music now to render the example tune!\n\n";
                form.ConcatLog(msg);

                OpenProject(dest, true);

            } else { 
                return;
            }
        }

        public static void OpenProject(string dir = null, bool inhibitMusicRenderPrompt = false) {
            form.klangPlayer.Stop();
            if (dir == null) {
                MessageBoxManager.Cancel = "Cancel";
                FolderBrowserDialog d = new FolderBrowserDialog();
                d.ShowNewFolderButton = false;
                d.Description = "Take me to it dog!";
                d.SelectedPath = Properties.Settings.Default.lastProjectLocation;
                if (d.ShowDialog() != DialogResult.Cancel)
                    dir = d.SelectedPath;
            }

            if (dir == "" || dir == null)
                return;

            if (!File.Exists(dir + "/project.kml")) {
                MessageBox.Show("Wow, you are quite old fashioned trying to open such an old project.\nI will convert it to the new format on save.", "4kampfpanzerin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                form.timeLine.LoadData("sync.dat");
            } else {
                using (Stream stream = File.Open(dir + "/project.kml", FileMode.Open)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(Project));
                    project = (Project) serializer.Deserialize(stream);
                    stream.Close();
                }
                form.timeLine.SetProject(project);
                GraphicsManager.GetInstance().updateProject(project);
                ApplySettings();
            }

            if (!File.Exists(dir + "\\vert.glsl") || !File.Exists(dir + "\\frag.glsl")) {
                MessageBox.Show("That doesn't seem to be a project directory;\nI couldn't find vert.glsl and frag.glsl :(", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            currentProjectDirectory = dir;
            Directory.SetCurrentDirectory(currentProjectDirectory);
            form.Text = "4kampf | TRSI" + " " + currentProjectDirectory;
            Properties.Settings.Default.lastProjectLocation = currentProjectDirectory;
            Properties.Settings.Default.Save();

            LoadShader();
            BuildShader();
            
            if (File.Exists("music.wav"))
                form.klangPlayer.LoadWAV(currentProjectDirectory + "\\music.wav");
            else if (!inhibitMusicRenderPrompt) {
                MessageBoxManager.Yes = "Yes";
                MessageBoxManager.No = "No";

                if (MessageBox.Show("This project doesn't have rendered music yet. Render now?", "4kampf", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    RenderMusic();
            }

            projectDirty = false;
            //mediaPlayer.Ctlcontrols.stop();
        }

        private static bool CheckDCP() {
            if (!File.Exists(Properties.Settings.Default.devCommandPromptLocation)) {
                MessageBox.Show("Hmmm, I can't find the Visual Studio Developer Command Prompt.\nPlease check \"Options->Set VS dev prompt location\"", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static void LoadShader() {
            StreamReader sr = new StreamReader("vert.glsl");
            form.edVert.Text = sr.ReadToEnd();
            form.edVert.UndoRedo.EmptyUndoBuffer();
            sr.Close();

            sr = new StreamReader("frag.glsl");
            form.edFrag.Text = sr.ReadToEnd();
            form.edFrag.UndoRedo.EmptyUndoBuffer();
            sr.Close();

            sr = new StreamReader("ppfrag.glsl");
            form.edPost.Text = sr.ReadToEnd();
            form.edPost.UndoRedo.EmptyUndoBuffer();
            sr.Close();
        }

        public static void MinifyShaders() {
            SaveProject();
            form.ShowLog("* Using shader_minify on yer shaders...\r\n");
            MinifyShader("frag.glsl");
            MinifyShader("vert.glsl");
            MinifyShader("ppfrag.glsl");
            LoadShader();
            form.ConcatLog("* Done");
        }

        private static void MinifyShader(string filename) {
            form.ConcatLog("* Minifying " + filename + "...");
            string cmd = AppDomain.CurrentDomain.BaseDirectory + "shader_minifier.exe";
            string args = "--preserve-externals --format none \"" + currentProjectDirectory + "\\" + filename + "\" -o \"" + currentProjectDirectory + "\\" + filename + "\"";

            Utils.LaunchAndLog(cmd, args);
        }


        // TODO: extract shader constant replacement stuff
        public static void BuildShader() {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            string vertText = form.edVert.Text;
            string fragText = form.edFrag.Text;
            string syncCode = TrackerCompiler.CompileSyncTrackerCode(form.timeLine.syncBars);
            string syncRest = TrackerCompiler.GetInterpolationCode(form.timeLine.syncBars, form.timeLine.camBars);
            vertText = vertText.Replace("//#SYNCCODE#", syncRest + syncCode);
            fragText = fragText.Replace("//#SYNCCODE#", syncRest + syncCode);

            vertText = vertText.Replace("CAMVARS", "uniform vec3 cp, cr;");
            
            form.ShowLog("");

            if (syncCode != "" && Properties.Settings.Default.useSyncTracker)
                form.ConcatLog("* Generated sync code:\r\n\r\n" + syncCode + "\r\n");

            string msg = "* Scene shader compilation:\r\n\r\n" + gfx.BuildShader(
                0, 
                vertText.Replace("\n", "\r\n"),
                fragText.Replace("\n", "\r\n")
            ).Replace("\n", "\r\n");
            form.ConcatLog(msg);

            if (Properties.Settings.Default.usePP) {
                string postText = form.edPost.Text;
                postText = postText.Replace("//#SYNCCODE#", syncCode);
                
                msg = "* Postprocessing shader compilation:\r\n\r\n" + gfx.BuildShader(
                    1, 
                    vertText.Replace("\n", "\r\n"), 
                    postText.Replace("\n", "\r\n")).Replace("\n", "\r\n");
                form.ConcatLog(msg);
            }
        }

        public static void RenderMusic(Boolean clinkster = false) {
            if (!CheckDCP())
                return;

            form.ShowLog("* Building wavwriter and rendering music...\r\n");

            ExportHeader();
            form.klangPlayer.Unload();

            string[] files = { "wavwriter.exe", "music.wav" };
            foreach (string s in files)
                if (File.Exists(s))
                    File.Delete(s);

            foreach (FileInfo f in new DirectoryInfo(currentProjectDirectory).GetFiles("envelope-*.dat"))
                f.Delete();
            string commandFormat = String.Format("/k \"\"{0}\" & cd \"{1}\\{2}\" & msbuild -p:configuration=\"Release\" & cd .. & wavwriter.exe\"",
                Properties.Settings.Default.devCommandPromptLocation, 
                currentProjectDirectory, 
                project.useClinkster ? "clinksterwriter" : "wavwriter");
            
            string command = "/k \"\"" + Properties.Settings.Default.devCommandPromptLocation + "\" & cd \"" + currentProjectDirectory + "\\wavwriter\" & msbuild -p:configuration=\"Release\" & cd .. & wavwriter.exe\"";
            form.ShowLog("Executing 'cmd.exe " + command + "'");
            Utils.LaunchAndLog("cmd.exe", commandFormat);

            if (!File.Exists("music.wav")) {
                form.ConcatLog("! The WAV didn't get written :O");
                return;
            }

            if (form.klangPlayer.LoadWAV(currentProjectDirectory + "/music.wav"))
                form.ConcatLog("* Tune rendered and loaded - happy days!");
            else
                form.ConcatLog("! Couldn't read WAV :(");
        }

        public static void DoBuildClean() {
            string[] paths = { "basecode/bin", "basecode/exe", "wavwriter/bin" };
            foreach (string s in paths)
                if (Directory.Exists(s))
                    Directory.Delete(s, true);

            string[] files = { "wavwriter.exe", "wavwriter/wavwriter.pdb" };
            foreach (string s in files)
                if (File.Exists(s))
                    File.Delete(s);

            form.ShowLog("Build files cleaned");
        }
        
        public static void DoExport() {
            ExportHeader();
            try {
                Process.Start("basecode\\prod.vcxproj");
            } catch (Exception) {
                MessageBox.Show("This Aggression will not stand dude, I need basecode!", "4kampfpanzer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void SaveProject(string directory = "") {
            
            StreamWriter sw = new StreamWriter("frag.glsl");
            sw.Write(form.edFrag.Text);
            sw.Close();
            sw = new StreamWriter("vert.glsl");
            sw.Write(form.edVert.Text);
            sw.Close();
            sw = new StreamWriter("ppfrag.glsl");
            sw.Write(form.edPost.Text);
            sw.Close();
            form.timeLine.SaveData("sync.dat");

            project.camBars = form.timeLine.camBars;
            project.syncBars = form.timeLine.syncBars;
            //project.settings = kampfpanzerin.Properties.Settings.Default;
            SaveProjectSettings(project);

            form.ShowLog("Saved all project assets");
            projectDirty = false;
        }

        private static void SaveProjectSettings(Project project, string directory = "") {
            try {
                using (Stream stream = File.Open(directory + "project.kml", FileMode.Create)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(Project));
                    serializer.Serialize(stream, project);
                    stream.Close();
                }
                if (Repo != null) {
                    Repo.Commit();
                }
            } catch (IOException) {
                MessageBox.Show("Something went wrong when saving.", "4kampfpanzerin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ExportHeader() {
            string syncCode = TrackerCompiler.CompileSyncTrackerCode(form.timeLine.syncBars);
            if (Properties.Settings.Default.usePP) {
                BuildUtils.DoExportHeader(
                    project,
                    form.klangPlayer.GetDuration(),
                    form.timeLine.syncBars,
                    form.edVert.Text,
                    form.edFrag.Text,
                    form.edPost.Text);
            } else {
                BuildUtils.DoExportHeader(project, form.klangPlayer.GetDuration(), form.timeLine.syncBars, form.edVert.Text, form.edFrag.Text);
            }
        }



        public static void DoRun() {
            form.klangPlayer.Stop();
            if (Properties.Settings.Default.lastBuildName != "" && File.Exists(Properties.Settings.Default.lastBuildName))
                Process.Start(Properties.Settings.Default.lastBuildName);
            else
                MessageBox.Show("Hmm, I can't see the .exe; maybe you should build it first? ;)", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void DoBuild(string buildtype) {
            if (!CheckDCP())
                return;

            form.klangPlayer.Stop();
            ExportHeader();

            form.ShowLog("* Building prod in " + buildtype + " mode...\r\n");
            Utils.LaunchAndLog("cmd.exe", "/k \"\"" + Properties.Settings.Default.devCommandPromptLocation + "\" & cd \"" + currentProjectDirectory + "\\basecode\" & msbuild -p:configuration=\"" + buildtype + "\"\"");

            string src = "basecode\\exe\\prod.exe";
            if (File.Exists(src)) {
                string dest = "prod-" + buildtype.Replace(' ', '-').ToLower() + ".exe";
                if (File.Exists(dest))
                    File.Delete(dest);
                File.Move(src, dest);
                Properties.Settings.Default.lastBuildName = dest;
                FileStream fs = new FileStream(dest, FileMode.Open, FileAccess.Read);
                long byteCount = fs.Length;
                fs.Close();
                form.ConcatLog("* Prod built! Written " + dest + ": " + byteCount + " bytes");
                if (byteCount <= 4096)
                    form.ConcatLog("NOW GO AND WIN THE COMPO! (" + (4096 - byteCount) + " bytes free)");
            } else
                form.ConcatLog("! No .exe written :(");
        }

        public static void DoProjClean() {
            MessageBoxManager.Yes = "Clean it!";
            MessageBoxManager.No = "No, Stop!!";

            if (MessageBox.Show("This will clean ALL generated project files\napart from prod executables!", "4kampf", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                DoBuildClean();  // Do a build clean

                form.klangPlayer.Unload();

                string[] files = { "music.wav", "4kampfpanzerin.h" };
                files.ToList().FindAll(File.Exists).ForEach(File.Delete);
                new DirectoryInfo(currentProjectDirectory).GetFiles("envelope-*.dat").ToList().ForEach(f => f.Delete());


                form.ShowLog("Project cleaned");
            }
        }

        public static void DoScreenshot() {
            Bitmap bmp = new Bitmap(form.preview.ClientSize.Width, form.preview.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(form.preview.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Gl.glReadPixels(0, 0, form.preview.ClientSize.Width, form.preview.ClientSize.Height, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            bmp.Save("screenshot.jpg");
            form.ShowLog("Screenshot saved \\o/");
        }

        public static void SetDirty(bool d = true) {
            projectDirty = d;
        }

        public static bool ReallyScratch() {
            if (!projectDirty)
                return true;

            MessageBoxManager.Yes = "Do it!";
            MessageBoxManager.No = "No way man";

            if (MessageBox.Show("You have unsaved changes dog! Do you really wanna proceed?", "4kampf", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                return true;

            return false;
        }

        public static void SetVSDevPromptLocation() {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Please locate VsDevCmd.bat";

            d.FileName = "vsdevcmd.bat";
            d.Filter = "Visual Studio Developer Command Prompt batch file|vsdevcmd.bat";
            d.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.devCommandPromptLocation);

            if (d.ShowDialog() == DialogResult.OK) {
                Properties.Settings.Default.devCommandPromptLocation = d.FileName;
                Properties.Settings.Default.Save();
            }
        }
    }
}
