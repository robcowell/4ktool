using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Tao.OpenGl;
using System.Xml.Serialization;
using System.Globalization;
using System.Runtime.InteropServices;
using kampfpanzerin.git;
using kampfpanzerin.components;
using kampfpanzerin.core.Serialization;
using kampfpanzerin.log;
using kampfpanzerin.utils;

namespace kampfpanzerin {
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

            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!File.Exists(path + "\\.gitconfig")) {
                MessageBox.Show("Hmm, you need a .gitconfig in " + path + "\n\nThere's a sample.gitconfig in the 4kampfpanzerin directory.", "Major bummer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            form = AppForm.GetInstance();
            ApplySettings();
            form.SetFullscreen();
            form.Show();    // Note: Not done in AppForm constructor to prevent SetFullscreen() etc causing ugly resizes
            form.DoForce16To9();

            MessageBoxManager.Register();

            WelcomeDialog wd = new WelcomeDialog();
            wd.StartPosition = FormStartPosition.CenterScreen;
            WelcomeDialogResult d;
            do {
                wd.ShowDialog();
                d = wd.result;
                switch (d) {
                    case WelcomeDialogResult.CREATE:
                        CreateProject();
                        break;
                    case WelcomeDialogResult.OPEN:
                        OpenProject();
                        break;
                    case WelcomeDialogResult.IMPORT:
                        ImportProject();
                        break;
                    case WelcomeDialogResult.QUIT:
                        Application.Exit();
                        Environment.Exit(0);
                        break; // C# is gay
                }
            } while (string.IsNullOrEmpty(currentProjectDirectory));

            GraphicsManager gfx = GraphicsManager.GetInstance();
            Application.EnableVisualStyles();
            while (!form.IsDisposed) {
                Application.DoEvents();
                if (!gfx.GetRenderEnabled())
                    Thread.Sleep(50);

                if (!string.IsNullOrEmpty(currentProjectDirectory))
                    gfx.Render();
                form.FrameUpdate();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Application.Exit();
            Environment.Exit(0);
        }

        public static void ImportProject() {
            ImportDialog d = new ImportDialog();
            d.ShowDialog();
            if (d.DialogResult == DialogResult.OK) {
                var bbd = new kampfpanzerin.core.Serialization.BitBucketData();
                bbd.RepoSlug = d.Repo.Slug;
                bbd.Team = d.Team;
                bbd.UserName = d.UserName;
                var targetPath = d.ProjectLocation + @"\" + d.Repo.Slug;
                GitHandler.Clone(d.Repo, targetPath, BitBucketUtils.GetCredentials(bbd));
                OpenProject(targetPath);
            }
        }

        public static void GoToProjectFolder() {
            if (currentProjectDirectory != "")
                Process.Start(currentProjectDirectory);
        }

        public static void ApplySettings() {
            form.pnlToolbar.Visible = Properties.Settings.Default.showToolBar;
            form.pnlCam.Visible = Properties.Settings.Default.enableCamControls;

            if (project.usePP && !form.tabControl1.TabPages.Contains(form.tabPP))
                form.tabControl1.TabPages.Add(form.tabPP);
            else if (!project.usePP && form.tabControl1.TabPages.Contains(form.tabPP))
                form.tabControl1.TabPages.Remove(form.tabPP);

            if (project.useVertShader && !form.tabControl1.TabPages.Contains(form.tabVS))
                form.tabControl1.TabPages.Add(form.tabVS);
            else if (!project.useVertShader && form.tabControl1.TabPages.Contains(form.tabVS))
                form.tabControl1.TabPages.Remove(form.tabVS);

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
            form.useExtraPPShaderToolStripMenuItem.Checked = project.usePP;
            form.useVertShaderToolStripMenuItem.Checked = project.useVertShader;
            form.enableMultithread4klangInProdToolStripMenuItem.Checked = Properties.Settings.Default.useSoundThread;
            form.showLinenumbersToolStripMenuItem.Checked = Properties.Settings.Default.showLineNumbers;
            form.showToolbarToolStripMenuItem.Checked = Properties.Settings.Default.showToolBar;

            form.btnCamToggle.BackColor = Properties.Settings.Default.enableCamControls ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnLoop.BackColor = Properties.Settings.Default.enableLooping ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnFullscreen.BackColor = Properties.Settings.Default.fullscreen ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnLineNumbers.BackColor = Properties.Settings.Default.showLineNumbers ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnEnvelopes.BackColor = project.use4klangEnv ? Color.FromArgb(100, 100, 100) : form.BackColor;
            form.btnStandardUniforms.BackColor = Properties.Settings.Default.enableStandardUniforms ? Color.FromArgb(100, 100, 100) : form.BackColor;

            form.musicPlayer.ApplySettings(project);

            Properties.Settings.Default.Save();
        }

        public static void ProcessStreamHandler(object sender, DataReceivedEventArgs e) {
            Logger.log(e.Data);
        }

        public static void CreateProject() {
            Logger.clear();
            form.musicPlayer.Stop();

            NewProjectWizard wzd = new NewProjectWizard();
            wzd.StartPosition = FormStartPosition.CenterParent;
            wzd.ShowDialog();
            if (wzd.DialogResult == DialogResult.OK) {
                form.musicPlayer.Unload();

                string dest = wzd.ProjectLocation + "/" + wzd.ProjectName;
                Project p = wzd.Project;
                p.useBitBucket = wzd.UseBitBucket;

                p.useClinkster = wzd.UseClinkster;
                p.synth = wzd.Synth;

                string src = AppDomain.CurrentDomain.BaseDirectory + "skel";
                Utils.CopyFolderContents(src, dest);

                SaveProjectSettingsAndCommit(p, dest + "/");

                Repo = GitHandler.Init(dest, p);

                string msg = "* Project created. Now drop your ";
                // TODO: add the real messages
                msg += p.useClinkster ? "music.asm" : "4klang.obj and 4klang.h";
                msg += " in there and run Build->Render Music.\r\n\r\n(Or just run Build->Render Music now to render the example tune!)\r\n";
                Logger.logf(msg);

                OpenProject(dest, true);
            } else {
                return;
            }
        }

        public static void OpenProject(string dir = null, bool inhibitMusicRenderPrompt = false) {
            form.musicPlayer.Stop();
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

            Logger.clear();

            if (!File.Exists(dir + "/project.kml")) {
                MessageBox.Show("That doesn't seem to be a project directory;\nI couldn't find project.kml :(", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else {
                using (Stream stream = File.Open(dir + "/project.kml", FileMode.Open)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(Project));
                    project = (Project)serializer.Deserialize(stream);
                    stream.Close();
                    if (project.FixLegacy())
                        SaveProjectSettingsAndCommit(project, dir);
                    if (project.name == null) {
                        project.name = dir.Substring(dir.LastIndexOf(@"\") + 1);
                        SaveProjectSettingsAndCommit(project, dir);
                    }
                }
                GraphicsManager.GetInstance().updateProject(project);
                Repo = new GitHandler(dir);
                ApplySettings();
            }

            currentProjectDirectory = dir;
            Directory.SetCurrentDirectory(currentProjectDirectory);
            form.Text = "4kampf | TRSI" + " " + currentProjectDirectory;
            Properties.Settings.Default.lastProjectLocation = dir;
            Properties.Settings.Default.Save();

            LoadShader();

            if (File.Exists("music.wav"))
                form.musicPlayer.LoadWAV(currentProjectDirectory + "\\music.wav");
            else if (!inhibitMusicRenderPrompt) {
                MessageBoxManager.Yes = "Yes";
                MessageBoxManager.No = "No";

                if (MessageBox.Show("This project doesn't have rendered music yet. Render now?", "4kampf", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    RenderMusic();
            }

            form.musicPlayer.PlayPause();   // Required to set prod length; AxWindowsMediaPlayer doesn't fully load it it till we play it, and we need duration...
            projectDirty = false;
        }

        private static bool CheckDCP() {
            if (!File.Exists(Properties.Settings.Default.devCommandPromptLocation)) {
                MessageBox.Show("Hmmm, I can't find the Visual Studio Developer Command Prompt.\nPlease check \"Options->Set VS dev prompt location\"", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static void LoadShader() {
            ReloadShaders();

            form.edVert.UndoRedo.EmptyUndoBuffer();
            form.edFrag.UndoRedo.EmptyUndoBuffer();
            form.edPost.UndoRedo.EmptyUndoBuffer();
        }

        // TODO: extract shader constant replacement stuff
        public static void BuildShader() {
            Logger.clear();     // Controversial, but really required IMO (Fell). Sorry, it's back!
            Logger.log("* Building shaders...");

            GraphicsManager gfx = GraphicsManager.GetInstance();
            string vertText = form.edVert.Text;
            string fragText = form.edFrag.Text;

            string msg = "Scene shader compilation:\r\n" + gfx.BuildShader(
                0,
                project.useVertShader,
                vertText.Replace("\n", "\r\n"),
                fragText.Replace("\n", "\r\n"),
                form.edVert,
                form.edFrag
            ).Replace("\n", "\r\n");
            Logger.log(msg);

            if (project.usePP) {
                string postText = form.edPost.Text;
                msg = "Postprocessing shader compilation:\r\n" + gfx.BuildShader(
                    1,
                    project.useVertShader,
                    vertText.Replace("\n", "\r\n"),
                    postText.Replace("\n", "\r\n"),
                form.edVert,
                form.edPost
                ).Replace("\n", "\r\n");
                Logger.log(msg);
            }
        }

        public static void RenderMusic(Boolean clinkster = false) {
            if (!CheckDCP())
                return;

            Logger.log("* Building wavwriter and rendering music...");

            ExportHeader();
            form.musicPlayer.Unload();

            string[] files = { "wavwriter.exe", "music.wav" };
            foreach (string s in files)
                if (File.Exists(s))
                    File.Delete(s);

            foreach (FileInfo f in new DirectoryInfo(currentProjectDirectory).GetFiles("envelope-*.dat"))
                f.Delete();

            string commandFormat = SoundCommandLine(project.synth);



            Utils.LaunchAndLog("cmd.exe", commandFormat);

            if (!File.Exists("music.wav")) {
                Logger.log("! The WAV didn't get written :O");
                return;
            }

            if (form.musicPlayer.LoadWAV(currentProjectDirectory + "/music.wav"))
                Logger.log("Tune rendered and loaded - happy days!\r\n");
            else
                Logger.log("! Couldn't read the WAV :(\r\n");
        }

        public static string SoundCommandLine(Synth synth) {
            switch (synth) {
                case Synth.clinkster:
                case Synth.vierklang:
                    return String.Format("/k \"\"{0}\" & cd \"{1}\\{2}\" & msbuild -p:configuration=\"Release\" & cd .. & wavwriter.exe & exit\"",
                            Properties.Settings.Default.devCommandPromptLocation,
                            currentProjectDirectory,
                            project.useClinkster ? "clinksterwriter" : "wavwriter");
                case Synth.oidos:
                    return String.Format("/k oidos\\easy_exe\\build.bat && music_wav.exe");

            }

            return "";
        }

        public static void DoBuildClean() {
            Logger.log("* Cleaning build files...");

            string[] paths = { "basecode/bin", "basecode/exe", "wavwriter/bin" };
            foreach (string s in paths)
                if (Directory.Exists(s))
                    Directory.Delete(s, true);

            string[] files = { "wavwriter.exe", "wavwriter/wavwriter.pdb" };
            foreach (string s in files)
                if (File.Exists(s))
                    File.Delete(s);

            Logger.log("Build files cleaned\r\n");
        }

        public static void DoExport() {
            ExportHeader();
            try {
                Process.Start("basecode\\prod.vcxproj");
            } catch (Exception) {
                MessageBox.Show("This Aggression will not stand dude, I need basecode!", "4kampfpanzer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void SaveProject(string directory = "", bool withCommitMessage = false) {
            StreamWriter sw = new StreamWriter("frag.glsl");
            sw.Write(form.edFrag.Text);
            sw.Close();
            sw = new StreamWriter("vert.glsl");
            sw.Write(form.edVert.Text);
            sw.Close();
            sw = new StreamWriter("ppfrag.glsl");
            sw.Write(form.edPost.Text);
            sw.Close();

            BuildShader();

            //project.settings = kampfpanzerin.Properties.Settings.Default;
            SaveProjectSettingsAndCommit(project, "", withCommitMessage);

            projectDirty = false;
        }

        private static void SaveProjectSettingsAndCommit(Project project, string directory = "", bool withCommitMessage = false) {
            if (Repo != null && !Repo.CheckCommitNeeded()) {
                Logger.logf("! Work harder, there is nothing changed since the last save, boss...");
                return;
            }
            try {
                using (Stream stream = File.Open(directory + "project.kml", FileMode.Create)) {
                    XmlSerializer serializer = new XmlSerializer(typeof(Project));
                    serializer.Serialize(stream, project);
                    stream.Close();
                }
                if (Repo != null) {
                    if (Repo.Conflicts.Count() != 0) {
                        ConflictsDialog dialog = new ConflictsDialog(Repo.Conflicts);
                        dialog.ShowDialog();
                        if (dialog.DialogResult == DialogResult.OK) {
                            Repo.Resolve(dialog.Resolved);
                            if (Repo.Conflicts.Count() > 0) {
                                Logger.logf("! Not all conflicts are resolved, boss...");
                            } else {
                                DoRepoCommit(withCommitMessage);
                            }
                        }
                    } else {
                        DoRepoCommit(withCommitMessage);
                    }
                }
            } catch (IOException) {
                Logger.log("! Something went wrong when saving!");
            }
        }

        private static void DoRepoCommit(bool withCommitMessage) {
            if (withCommitMessage) {
                SaveWithCommitMessage dialog = new SaveWithCommitMessage();
                dialog.ShowDialog();
                if (dialog.DialogResult == DialogResult.OK) {
                    string commitMessage = dialog.GetCommitMessage();
                    Repo.Commit(commitMessage);
                }
            } else
                Repo.Commit();
        }

        private static void ExportHeader() {
            string vertText = form.edVert.Text;
            string fragText = form.edFrag.Text;
            if (project.usePP) {
                BuildUtils.DoExportHeader(
                    project,
                    form.musicPlayer.GetDuration(),
                    vertText,
                    fragText,
                    form.edPost.Text);
            } else {
                BuildUtils.DoExportHeader(project, form.musicPlayer.GetDuration(), vertText, fragText);
            }
        }

        public static void DoRun() {
            form.musicPlayer.Stop();
            if (Properties.Settings.Default.lastBuildName != "" && File.Exists(Properties.Settings.Default.lastBuildName))
                Process.Start(Properties.Settings.Default.lastBuildName);
            else
                MessageBox.Show("Hmm, I can't see the .exe; maybe you should build it first? ;)", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void DoBuild(string buildtype) {
            if (!CheckDCP())
                return;

            form.musicPlayer.Stop();
            ExportHeader();

            Logger.clear();     // Controversial, but really required IMO (Fell). Sorry, it's back!
            Logger.log("* Building prod in " + buildtype + " mode...");
            Utils.LaunchAndLog("cmd.exe", "/k \"\"" + Properties.Settings.Default.devCommandPromptLocation + "\" & cd \"" + currentProjectDirectory + "\\basecode\" & msbuild -p:configuration=\"" + buildtype + "\" & exit \"");

            string src = "basecode\\exe\\prod.exe";
            if (File.Exists(src)) {
                try {
                    string dest = "prod-" + buildtype.Replace(' ', '-').ToLower() + ".exe";
                    if (File.Exists(dest))
                        File.Delete(dest);
                    File.Move(src, dest);
                    Properties.Settings.Default.lastBuildName = dest;
                    FileStream fs = new FileStream(dest, FileMode.Open, FileAccess.Read);
                    long byteCount = fs.Length;
                    fs.Close();
                    if (byteCount > 0) {
                        Logger.log("* Prod built! Written " + dest + ": " + byteCount + " bytes\r\n");
                        if (byteCount <= 4096)
                            Logger.log("NOW GO AND WIN THE COMPO! (" + (4096 - byteCount) + " bytes free)\r\n");
                        else if (buildtype != "Debug")
                            Logger.log("TIME FOR A SHAVE... (" + (byteCount - 4096) + " bytes to lose)\r\n");
                    } else
                        Logger.log("! Build failed/cancelled :(\r\n");
                } catch (Exception) {
                    Logger.log("! Couldn't copy the exe :(\r\n");
                }
            } else
                Logger.log("! No .exe written :(\r\n");
        }

        public static void DoProjClean() {
            MessageBoxManager.Yes = "Clean it!";
            MessageBoxManager.No = "No, Stop!!";

            if (MessageBox.Show("This will clean ALL generated project files\napart from prod executables!", "4kampf", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                DoBuildClean();  // Do a build clean

                form.musicPlayer.Unload();

                Logger.log("* Cleaning runtime files...");

                string[] files = { "music.wav", "4kampfpanzerin.h" };
                files.ToList().FindAll(File.Exists).ForEach(File.Delete);
                new DirectoryInfo(currentProjectDirectory).GetFiles("envelope-*.dat").ToList().ForEach(f => f.Delete());

                Logger.log("Runtime files cleaned");
            }
        }

        public static void DoScreenshot() {
            Bitmap bmp = new Bitmap(form.preview.ClientSize.Width, form.preview.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(form.preview.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Gl.glReadPixels(0, 0, form.preview.ClientSize.Width, form.preview.ClientSize.Height, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) {
                Utils.SaveJpeg(bmp, currentProjectDirectory + "/screenshot.jpg", 80);
                Logger.log("* Screenshot jpeg saved \\o/");
            } else {
                bmp.Save(currentProjectDirectory + "/screenshot.png", System.Drawing.Imaging.ImageFormat.Png);
                Logger.log("* Screenshot png saved \\o/");
            }
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

        internal static void ReloadShaders() {
            StreamReader sr = new StreamReader("vert.glsl");
            form.edVert.Text = sr.ReadToEnd();
            sr.Close();

            sr = new StreamReader("frag.glsl");
            form.edFrag.Text = sr.ReadToEnd();
            sr.Close();

            sr = new StreamReader("ppfrag.glsl");
            form.edPost.Text = sr.ReadToEnd();
            sr.Close();

            BuildShader();
        }
    }
}
// monolith line!
