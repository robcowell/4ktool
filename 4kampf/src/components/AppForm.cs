using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using kampfpanzerin.core.Serialization;

namespace kampfpanzerin {
    public partial class AppForm : Form {
        private static AppForm formInstance;
        private bool logBusy = false;

        public static AppForm GetInstance() {
            if (formInstance == null)
                formInstance = new AppForm();
            return formInstance;
        }

        private AppForm() {
            InitializeComponent();
            this.menuStrip.Renderer = new ToolStripProfessionalRenderer(new MenuColorTable());
            GraphicsManager gfx = GraphicsManager.GetInstance();
            gfx.Init(this.preview);

            InitScintillaCustomisations(edVert);
            InitScintillaCustomisations(edFrag);
            InitScintillaCustomisations(edPost);

            log.Text = " ";  // Force a textchanged to fire
            log.Text = "";
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void rUNToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoRun();
        }

        private void AppForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (!Kampfpanzerin.ReallyScratch())
                e.Cancel = true;        
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }
        
        private void freeFlyButton_Click(object sender, EventArgs e) {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            gfx.SetCameraMode(CameraMode.FREEFLY);
            freeFlyButton.BackColor = Color.FromArgb(100, 100, 100);
            camAutoButton.BackColor = Color.FromArgb(32, 32, 32);
            preview.Focus();
        }

        private void camAutoButton_Click(object sender, EventArgs e) {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            gfx.SetCameraMode(CameraMode.AUTOMATED);
            freeFlyButton.BackColor = Color.FromArgb(32, 32, 32);
            camAutoButton.BackColor = Color.FromArgb(100, 100, 100);
            preview.Focus();
        }

        private void stepDownButton_Click(object sender, EventArgs e) {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            float editStep = gfx.GetEditStep();

            if (editStep > 0.0001f)
                editStep /= 10.0f;

            //btnCamReset.Text = editStep.ToString();

            stepDownButton.Enabled = (editStep > 0.0005f);
            stepUpButton.Enabled = (editStep < 50000);

            gfx.SetEditStep(editStep);
            preview.Focus();
        }

        private void stepUpButton_Click(object sender, EventArgs e) {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            float editStep = gfx.GetEditStep();

            if (editStep < 100000f)
                editStep *= 10.0f;

            //btnCamReset.Text = editStep.ToString();

            stepDownButton.Enabled = (editStep > 0.0005f);
            stepUpButton.Enabled = (editStep < 50000);

            gfx.SetEditStep(editStep);
            preview.Focus();
        }
        
        private void enableSyncTrackerToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.useSyncTracker = !Properties.Settings.Default.useSyncTracker;
            Kampfpanzerin.ApplySettings();
        }

        private void enableTrack02EnvsInUniformSvrequiresCorrect4klangExportToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.project.use4klangEnv = !Kampfpanzerin.project.use4klangEnv;
            Kampfpanzerin.ApplySettings();
        }

        private void enableMultithread4klangInProdToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.useSoundThread = !Properties.Settings.Default.useSoundThread;
            Kampfpanzerin.ApplySettings();
        }

        private void enableCamControlToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.enableCamControls = !Properties.Settings.Default.enableCamControls;
            Kampfpanzerin.ApplySettings();
        }

        private void enableStandardUniformsToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.enableStandardUniforms = !Properties.Settings.Default.enableStandardUniforms;
            Kampfpanzerin.ApplySettings();
        }

        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e) {
            ToggleFullscreen();
        }
        
        private void resetCameraToolStripMenuItem_Click(object sender, EventArgs e) {
            GraphicsManager.GetInstance().GetCamera().Reset();
        }

        private void rebuildShadersToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.BuildShader();

            GraphicsManager gfx = GraphicsManager.GetInstance();
            if (!gfx.GetRenderEnabled())
                gfx.Render(true);
        }

        private void loopTrackToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.enableLooping = !Properties.Settings.Default.enableLooping;
            Kampfpanzerin.ApplySettings();
        }

        private void useExtraPPShaderToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.usePP = !Properties.Settings.Default.usePP;
            Kampfpanzerin.ApplySettings();
            Kampfpanzerin.BuildShader();
        }

        private void exportToVSToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoExport();
        }

        private void preview_MouseClick(object sender, MouseEventArgs e) {
            preview.Focus();
        }

        private void preview_MouseDown(object sender, MouseEventArgs e) {
            preview.Focus();
        }

        private void preview_MouseWheel(object sender, MouseEventArgs e)
        {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            float amt = -e.Delta*0.1f;
            gfx.GetCamera().Move(ModifierKeys == Keys.Shift ? amt : amt * 0.1f);
        }

        private void cleanToolStripMenuItem_Click_1(object sender, EventArgs e) {
            Kampfpanzerin.DoBuildClean();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoBuild("Debug");
        }

        private void unpackedReleaseToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoBuild("Unpacked release");
        }

        private void packedReleasefastToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoBuild("Packed release (fast)");
        }

        private void packedReleaseslowToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoBuild("Packed release");
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e) {
            DoNew();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            DoOpen();
        }

        private void rerender4klangMuskcToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.RenderMusic();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.SaveProject();
        }

        private void minifyShadersToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.MinifyShaders();
        }

        private void edVert_TextChanged(object sender, EventArgs e) {
            Kampfpanzerin.SetDirty();
        }

        private void edFrag_TextChanged(object sender, EventArgs e) {
            Kampfpanzerin.SetDirty();
        }

        private void edPost_TextChanged(object sender, EventArgs e) {
            Kampfpanzerin.SetDirty();
        }

        private void setVisualStudioDevCommandpromptLocationToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.SetVSDevPromptLocation();
        }

        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoScreenshot();
        }

        private void openProjectLocationToolStripMenuItem_Click(object sender, EventArgs e) {
            Kampfpanzerin.GoToProjectFolder();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox box = new AboutBox();
            box.ShowDialog();
        }

        public void SetFullscreen() {
            if (Properties.Settings.Default.fullscreen) {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                //statusStrip1.SizingGrip = false;
            } else {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                //statusStrip1.SizingGrip = true;
            }
        }

        public void FrameUpdate() {
            GraphicsManager gfx = GraphicsManager.GetInstance();

            float time = gfx.GetTrackTime();
            string fps = (int)gfx.GetFPS() + " fps";
            string timeStr = time.ToString("000.00");

            if (Properties.Settings.Default.enableCamControls) {
                Camera c = gfx.GetCamera();
                if (c.CheckAndResetDirty()) {
                    Vector3f camPos = c.Position, camRot = c.Rotation;
                    string camStr = camPos.x.ToString("0.00", Kampfpanzerin.culture) + "," + camPos.y.ToString("0.00", Kampfpanzerin.culture) + "," + camPos.z.ToString("0.00", Kampfpanzerin.culture) + "\n";
                    camStr += camRot.x.ToString("0.00", Kampfpanzerin.culture) + "," + camRot.y.ToString("0.00", Kampfpanzerin.culture) + "," + camRot.z.ToString("0.00", Kampfpanzerin.culture);
                    lblCam.Text = camStr;
               }
            } else
                lblCam.Text = "";

            musicPlayer.SetLabels(fps, timeStr);
            musicPlayer.UpdateStuff();

            if (Properties.Settings.Default.useSyncTracker) {
                timeLine.SetTime(time);
                timeLine.Redraw();
            }
        }

        private void colourHelperToolStripMenuItem_Click(object sender, EventArgs e) {
            DoColourHelper();
        }
        
        private void cleanToolStripMenuItem1_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoProjClean();
        }

        private void btnRefreshShaders_Click(object sender, EventArgs e) {
            Kampfpanzerin.BuildShader();
        }

        private void btnNew_Click(object sender, EventArgs e) {
            DoNew();
        }

        private void btnClearLog_Click(object sender, EventArgs e) {
            ClearLog();
        }

        private void btnOpenProj_Click(object sender, EventArgs e) {
            DoOpen();
        }

        private void DoOpen() {
            if (Kampfpanzerin.ReallyScratch())
                Kampfpanzerin.OpenProject();
        }

        private void DoNew() {
            if (Kampfpanzerin.ReallyScratch())
                Kampfpanzerin.CreateProject();
        }

        private void DoImport() {
            if (Kampfpanzerin.ReallyScratch())
                Kampfpanzerin.ImportProject();
        }

        private void btnSaveAll_Click(object sender, EventArgs e) {
            Kampfpanzerin.SaveProject();
        }

        private void btnUndo_Click(object sender, EventArgs e) {
            GetActiveScintilla().UndoRedo.Undo();
        }

        private ScintillaNET.Scintilla GetActiveScintilla() {
            if (tabControl1.SelectedIndex == 0)
                return edVert;
            
            if (tabControl1.SelectedIndex == 1)
                return edFrag;

            return edPost;
        }

        private void btnRedo_Click(object sender, EventArgs e) {
            GetActiveScintilla().UndoRedo.Redo();
        }

        private void btnFind_Click(object sender, EventArgs e) {
            GetActiveScintilla().FindReplace.ShowFind();
        }

        private void btnCamReset_Click(object sender, EventArgs e) {
            GraphicsManager.GetInstance().GetCamera().Reset();
            preview.Focus();
        }

        private void btnCamToggle_Click(object sender, EventArgs e) {
            Properties.Settings.Default.enableCamControls = !Properties.Settings.Default.enableCamControls;
            Kampfpanzerin.ApplySettings();
        }

        private void btnLoop_Click(object sender, EventArgs e) {
            Properties.Settings.Default.enableLooping = !Properties.Settings.Default.enableLooping;
            Kampfpanzerin.ApplySettings();
        }

        private void btnBuild_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoBuild("Debug");
        }

        private void btnRun_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoRun();
        }

        private void btnRenderMusic_Click(object sender, EventArgs e) {
            Kampfpanzerin.RenderMusic();
        }

        private void ToggleFullscreen() {
            Properties.Settings.Default.fullscreen = !Properties.Settings.Default.fullscreen;
            Kampfpanzerin.ApplySettings();
            SetFullscreen();
        }

        private void btnFullscreen_Click(object sender, EventArgs e) {
            ToggleFullscreen();
        }

        private void btnExportProj_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoExport();
        }

        private void InitScintillaCustomisations(ScintillaNET.Scintilla sci) {
            sci.Lexing.Lexer = ScintillaNET.Lexer.Cpp;
            sci.Lexing.Keywords[0] = "attribute const uniform varying layout centroid flat smooth noperspective break continue do for while switch case default if else in out inout true false invariant discard return lowp mediump highp precision struct radians degrees sin cos tan asin acos atan atan sinh cosh tanh asinh acosh atanh pow exp log exp2 log2 sqrt inversesqrt abs sign floor trunc round roundEven ceil fract mod modf min max clamp mix step smoothstep isnan isinf floatBitsToInt floatBitsToUint intBitsToFloat uintBitsToFloat length distance dot cross normalize faceforward reflect refract matrixCompMult outerProduct transpose determinant inverse lessThan lessThanEqual greaterThan greaterThanEqual equal notEqual any all not textureSize texture textureProj textureLod textureOffset texelFetch texelFetchOffset textureProjOffset textureLodOffset textureProjLod textureProjLodOffset textureGrad textureGradOffset textureProjGrad textureProjGradOffset texture1D texture1DProj texture1DProjLod texture2D texture2DProj texture2DLod texture2DProjLod texture3D texture3DProj texture3DLod texture3DProjLod textureCube textureCubeLod shadow1D shadow2D shadow1DProj shadow2DProj shadow1DLod shadow2DLod shadow1DProjLod shadow2DProjLod dFdx dFdy fwidth noise1 noise2 noise3 noise4 EmitVertex EndPrimitive gl_VertexID gl_InstanceID gl_Vertex gl_Position gl_PointSize gl_ClipDistance gl_PerVertex gl_Layer gl_ClipVertex gl_FragCoord gl_FrontFacing gl_ClipDistance gl_FragColor gl_FragData gl_MaxDrawBuffers gl_FragDepth gl_PointCoord gl_PrimitiveID gl_MaxVertexAttribs gl_MaxVertexUniformComponents gl_MaxVaryingFloats gl_MaxVaryingComponents gl_MaxVertexOutputComponents gl_MaxGeometryInputComponents gl_MaxGeometryOutputComponents gl_MaxFragmentInputComponents gl_MaxVertexTextureImageUnits gl_MaxCombinedTextureImageUnits gl_MaxTextureImageUnits gl_MaxFragmentUniformComponents gl_MaxDrawBuffers gl_MaxClipDistances gl_MaxGeometryTextureImageUnits gl_MaxGeometryOutputVertices gl_MaxGeometryOutputVertices gl_MaxGeometryTotalOutputComponents gl_MaxGeometryUniformComponents gl_MaxGeometryVaryingComponents gl_DepthRange";
            sci.Lexing.Keywords[1] = "float int void bool mat2 mat3 mat4 mat2x2 mat2x3 mat2x4 mat3x2 mat3x3 mat3x4 mat4x2 mat4x3 mat4x4 vec2 vec3 vec4 ivec2 ivec3 ivec4 bvec2 bvec3 bvec4 uint uvec2 uvec3 uvec4 sampler1D sampler2D sampler3D samplerCube sampler1DShadow sampler2DShadow samplerCubeShadow sampler1DArray sampler2DArray sampler1DArrayShadow sampler2DArrayShadow isampler1D isampler2D isampler3D isamplerCube isampler1DArray isampler2DArray usampler1D usampler2D usampler3D usamplerCube usampler1DArray usampler2DArray sampler2DRect sampler2DRectShadow isampler2DRect usampler2DRect samplerBuffer isamplerBuffer usamplerBuffer sampler2DMS isampler2DMS usampler2DMS sampler2DMSArray isampler2DMSArray usampler2DMSArray";
            sci.Styles[33].ForeColor = Color.DarkGray;                   // Line numbers
            sci.Styles[33].BackColor = Color.FromArgb(32,32,32);
            sci.Styles[sci.Lexing.StyleNameMap["WORD"]].ForeColor = Color.FromArgb(86, 156, 214);
            sci.Styles[sci.Lexing.StyleNameMap["WORD2"]].ForeColor = Color.FromArgb(78, 201, 176);
            sci.Styles[sci.Lexing.StyleNameMap["COMMENT"]].ForeColor = Color.FromArgb(87, 166, 74);
            sci.Styles[sci.Lexing.StyleNameMap["COMMENTLINE"]].ForeColor = Color.FromArgb(87, 166, 74);
            sci.Styles[sci.Lexing.StyleNameMap["NUMBER"]].ForeColor = Color.FromArgb(194, 58, 255);
            //sci.Styles[sci.Lexing.StyleNameMap["OPERATOR"]].ForeColor = Color.FromArgb(200, 200, 200);
            sci.Styles[sci.Lexing.StyleNameMap["PREPROCESSOR"]].ForeColor = Color.FromArgb(214, 157, 133);
            sci.Caret.Color = Color.White;
        }

        private void btnColourHelper_Click(object sender, EventArgs e) {
            DoColourHelper();
        }

        private void DoColourHelper() {
            Color c = Color.Gray;
            bool isReplacing = false;

            string s = GetActiveScintilla().Selection.Text;

            s = s.Replace("vec3", "");
            s = s.Replace(")", "");
            s = s.Replace("(", "");
            s = s.Replace(" ", "");
            string[] ss = s.Split(',');
            if (ss.Length == 3) {
                try {
                    float r = Utils.Clamp(float.Parse(ss[0]));
                    float g = Utils.Clamp(float.Parse(ss[1]));
                    float b = Utils.Clamp(float.Parse(ss[2]));
                    c = Color.FromArgb((int)(r * 255.0f), (int)(g * 255.0f), (int)(b * 255.0f));
                    isReplacing = true;
                } catch (Exception) { }
            }
            
            ColorChooser frm = new ColorChooser(isReplacing);
            frm.Color = c;
            if (isReplacing) {
                if (frm.ShowDialog() == DialogResult.OK)
                    GetActiveScintilla().Clipboard.Paste();
            } else
                frm.Show();
        }

        private void showLinenumbersToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.showLineNumbers = !Properties.Settings.Default.showLineNumbers;
            Kampfpanzerin.ApplySettings();
        }

        private void btnLineNumbers_Click(object sender, EventArgs e) {
            Properties.Settings.Default.showLineNumbers = !Properties.Settings.Default.showLineNumbers;
            Kampfpanzerin.ApplySettings();
        }

        private void btnTracker_Click(object sender, EventArgs e) {
            Properties.Settings.Default.useSyncTracker = !Properties.Settings.Default.useSyncTracker;
            Kampfpanzerin.ApplySettings();
        }

        private void btnEnvelopes_Click(object sender, EventArgs e) {
            Kampfpanzerin.project.use4klangEnv = !Kampfpanzerin.project.use4klangEnv;
            Kampfpanzerin.ApplySettings();
        }

        private void showToolbarToolStripMenuItem_Click(object sender, EventArgs e) {
            Properties.Settings.Default.showToolBar = !Properties.Settings.Default.showToolBar;
            Kampfpanzerin.ApplySettings();
        }

        private void btnCleanProj_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoProjClean();
        }

        private void btnCleanBuild_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoBuildClean();
        }

        private void btnScreenshot_Click(object sender, EventArgs e) {
            Kampfpanzerin.DoScreenshot();
        }

        private void btnStandardUniforms_Click(object sender, EventArgs e) {
            Properties.Settings.Default.enableStandardUniforms = !Properties.Settings.Default.enableStandardUniforms;
            Kampfpanzerin.ApplySettings();
        }

        private void log_TextChanged(object sender, EventArgs e) {
            if (logBusy) 
                return;  
            logBusy = true;  
            TextBox tb = sender as TextBox;  
            Size tS = TextRenderer.MeasureText(tb.Text, tb.Font);  
            bool Hsb = tb.ClientSize.Height < tS.Height + Convert.ToInt32(tb.Font.Size);
            bool Vsb = tb.ClientSize.Width < tS.Width;  
 
            if (Hsb && Vsb)  
                tb.ScrollBars = ScrollBars.Both;  
            else if (!Hsb && !Vsb)  
                tb.ScrollBars = ScrollBars.None;  
            else if (Hsb && !Vsb)  
                tb.ScrollBars = ScrollBars.Vertical;  
            else if (!Hsb && Vsb)  
                tb.ScrollBars = ScrollBars.Horizontal;  
 
            sender = tb as object;  
            logBusy = false;
        }

        public void ClearLog() {
            log.Text = "";
        }

        public void ConcatLog(string what) {
            MethodInvoker action = delegate {
                log.Text += what + "\r\n";
                log.SelectionStart = log.TextLength;
                log.ScrollToCaret();
            };
            log.BeginInvoke(action);
        }

        private void btnGotoProjectFolder_Click(object sender, EventArgs e) {
            Kampfpanzerin.GoToProjectFolder();
        }

        private void btnPush_Click(object sender, EventArgs e) {
            PushProject();
        }

        private void btnPull_Click(object sender, EventArgs e) {
            PullProject();
        }

        private bool CheckRemote() {
            if (Kampfpanzerin.project.bitBucketSettings == null) {
                MessageBox.Show("No remote configured, chief!", "4kampfpanzerin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void PullProject() {
            if (!CheckRemote()) {
                return;
            }
            Kampfpanzerin.Repo.Pull(Kampfpanzerin.project, utils.BitBucketUtils.GetCredentials(Kampfpanzerin.project.bitBucketSettings));
            Kampfpanzerin.ReloadShaders();
        }

        private void PushProject() {
            if (!CheckRemote()) {
                return;
            }
            Kampfpanzerin.Repo.Push(Kampfpanzerin.project, utils.BitBucketUtils.GetCredentials(Kampfpanzerin.project.bitBucketSettings));
        }

        private void pullFromBitBucketToolStripMenuItem_Click(object sender, EventArgs e) {
            PullProject();
        }

        private void pushToBitbucketToolStripMenuItem_Click(object sender, EventArgs e) {
            PushProject();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e) {
            DoImport();
        }

        private void forceDisplayTo169ToolStripMenuItem_Click(object sender, EventArgs e) {
            DoForce16To9();
        }

        private void lblCam_Click(object sender, EventArgs e) {
            string[] cam = lblCam.Text.Split('\n');
            Clipboard.SetText("cp=vec3(" + cam[0].ToString(Kampfpanzerin.culture) + "),cr=vec3(" + cam[1].ToString(Kampfpanzerin.culture) + ")");
        }

        public void DoForce16To9() {
            Size current = preview.Size;
            int newHeight = current.Width / 16 * 9,
                change = newHeight - current.Height;
            splitLHS.SplitterDistance += change;
        }

        private void btnRenderEnable_Click(object sender, EventArgs e) {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            gfx.SetRenderEnabled(!gfx.GetRenderEnabled());
            btnRenderEnable.BackColor = gfx.GetRenderEnabled() ? Color.FromArgb(100, 100, 100) : Color.FromArgb(64,64,64);
        }
    }
}
