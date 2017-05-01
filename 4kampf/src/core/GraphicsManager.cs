using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using kampfpanzerin.core.Serialization;

using Tao.OpenGl;
using Tao.Platform.Windows;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;

namespace kampfpanzerin
{
	public class GraphicsManager
	{
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out ulong lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out ulong lpFrequency);
        
        private const float MOVE_SPEED = 4.0f;      // q/sec
        private const float MOUSE_LOOK_SPEED = 0.5f;
   
        private static GraphicsManager instance;    // Singleton blad

        private Camera camera = new Camera();
        private Control control;
		private IntPtr hDC;
		private IntPtr hRC;
		private bool[] keys = new bool[256];
        private Point mouseStartPos; 
        private Point mouseCurrentPos; 
        private bool rmp, mouseRDown;
        private float editStep = 0.1f;
		private uint numFrames = 0;
		private float fpsUpdateInterval = 1.0f, lastUpdate = 0, fps = 0, frameTime, lastFrameTime;
		private ulong ticksPerSecond;
        private int[] shaderProg = new int[] { -1, -1 };
        private int rtt,rtt2,fbo;
        private bool renderEnabled = true;

        private BitmapFont debugLabeller;
        private Project project;

		public static GraphicsManager GetInstance() {
			if(instance == null)
				instance = new GraphicsManager();
			return instance;
		}

        private void BuildRenderTarget() {
            Gl.glGenTextures(1, out rtt);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rtt);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA16F_ARB, control.Width, control.Height, 0, Gl.GL_RGBA, Gl.GL_HALF_FLOAT_ARB, IntPtr.Zero);

            Gl.glGenTextures(1, out rtt2);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rtt2);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA16F_ARB, control.Width, control.Height, 0, Gl.GL_RGBA, Gl.GL_HALF_FLOAT_ARB, IntPtr.Zero);

            Gl.glGenFramebuffersEXT(1, out fbo);
            Gl.glBindFramebufferEXT(Gl.GL_FRAMEBUFFER_EXT, fbo);
            Gl.glFramebufferTexture2DEXT(Gl.GL_FRAMEBUFFER_EXT, Gl.GL_COLOR_ATTACHMENT0_EXT, Gl.GL_TEXTURE_2D, rtt, 0);
            Gl.glFramebufferTexture2DEXT(Gl.GL_FRAMEBUFFER_EXT, Gl.GL_COLOR_ATTACHMENT1_EXT, Gl.GL_TEXTURE_2D, rtt2, 0);

            int[] myBuffers = { Gl.GL_COLOR_ATTACHMENT0_EXT, Gl.GL_COLOR_ATTACHMENT1_EXT };
            Gl.glDrawBuffers(2,myBuffers);
            Gl.glBindFramebufferEXT(Gl.GL_FRAMEBUFFER_EXT, 0);
        }

        public string BuildShader(int progIndex, bool useVertShader, string vertSource, string fragSource, ScintillaNET.Scintilla editorVert, ScintillaNET.Scintilla editorFrag) {
            string[] fragShaderSource = { fragSource };

            if (shaderProg[progIndex] > -1) {
                Gl.glUseProgram(0);
                Gl.glDeleteShader(shaderProg[progIndex]);
            }

            shaderProg[progIndex] = Gl.glCreateProgram();

            StringBuilder vsResult = new StringBuilder(60000);
            if (useVertShader) {
                string[] vertShaderSource = { vertSource };
                int vs = Gl.glCreateShader(Gl.GL_VERTEX_SHADER);
                Gl.glShaderSource(vs, 1, vertShaderSource, IntPtr.Zero);
                Gl.glCompileShader(vs);
                Gl.glAttachShader(shaderProg[progIndex], vs);                
                Gl.glGetInfoLogARB(vs, 60000, IntPtr.Zero, vsResult);
            }

            int fs = Gl.glCreateShader(Gl.GL_FRAGMENT_SHADER);
            Gl.glShaderSource(fs, 1, fragShaderSource, IntPtr.Zero);
            Gl.glCompileShader(fs);
            Gl.glAttachShader(shaderProg[progIndex], fs);
            StringBuilder fsResult = new StringBuilder(60000);
            Gl.glGetInfoLogARB(fs, 60000, IntPtr.Zero, fsResult);

            Gl.glLinkProgram(shaderProg[progIndex]);
            StringBuilder linkResult = new StringBuilder(60000);
            Gl.glGetInfoLogARB(shaderProg[progIndex], 60000, IntPtr.Zero, linkResult);

            string result = "";
            bool noLog = true;
            
            if (useVertShader && vsResult.ToString().Length > 0) {
                noLog = false;
                result += "\nVert shader compile log:\n" + vsResult.ToString() + "\n";
                /*
                int lineNum = int.Parse(vsResult.ToString().Split('(', ')')[1])-1;
                MessageBox.Show(lineNum.ToString()); 
                editorVert.CurrentPos = lineNum;
                */
            }
            if (fsResult.ToString().Length > 0) {
                noLog = false;
                result += "\nFrag shader compile log:\n" + fsResult.ToString() + "\n";
                /*
                int lineNum = int.Parse(fsResult.ToString().Split('(', ')')[1]) - 1;
                MessageBox.Show(lineNum.ToString());
                editorFrag.CurrentPos = lineNum;
                 */
            }

            if (linkResult.ToString().Length > 0) {
                noLog = false;
                result += "\nShader prog link log:\n" + linkResult.ToString() + "\n";
            }
            if (noLog)
                result = "No shader compilation errors, homeboy \\o/\n";

            return result;
        }

		public void Init (Control c) {
			control = c;

			control.KeyDown += new KeyEventHandler(this.KeyDown);
			control.KeyUp += new KeyEventHandler(this.KeyUp);
            control.MouseDown += new MouseEventHandler(this.MouseDown);
            control.MouseUp += new MouseEventHandler(this.MouseUp);
			control.MouseMove += new MouseEventHandler(this.MouseMove);
            control.LostFocus += new EventHandler(this.ResetKeys);
            control.Resize += new EventHandler(this.Resize);

			GC.Collect();
			Kernel.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);	// Force a swap

            if (!CreateGLSurface()) {
                MessageBox.Show("Couldn't create GL surface!");
                Application.Exit();
            }

            debugLabeller = new BitmapFont("Small Fonts", hDC, 8);
            
            BuildRenderTarget();
            
            if (!QueryPerformanceFrequency(out ticksPerSecond))
                ticksPerSecond = 1000;  // Assuming 1000 ticks per second
		}

		public GraphicsManager() {
			mouseCurrentPos = new Point(0, 0);
            ResetKeys(null, null);
		}

		private void ResetKeys(Object o, EventArgs args) {
			for (int i=0; i<256; i++)
				keys[i]=false;
		}

		private void KeyDown(object sender, KeyEventArgs e) {
			keys[e.KeyValue] = true;
		}

		private void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)  {
			switch (e.Button) {
				case MouseButtons.Right:
					mouseRDown = true;
					break;
			}
		}
		
		private void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			mouseRDown = false;
		}

		private void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            mouseCurrentPos.X = e.X;
            mouseCurrentPos.Y = e.Y;
		}

		private void KeyUp(object sender, KeyEventArgs e) {
			keys[e.KeyValue] = false;
		}

		private void KillGLWindow() {
			if(hRC != IntPtr.Zero) {
				if(!Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero))
					MessageBox.Show("Release Of DC And RC Failed.", "SHUTDOWN ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

				if(!Wgl.wglDeleteContext(hRC))
					MessageBox.Show("Release Rendering Context Failed.", "SHUTDOWN ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

				hRC = IntPtr.Zero;
			}

			if(hDC != IntPtr.Zero) {   
				if(control != null && !control.IsDisposed) 
					if(control.Handle != IntPtr.Zero)
                        if(!User.ReleaseDC(control.Handle, hDC))
							MessageBox.Show("Release Device Context Failed.", "SHUTDOWN ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

				hDC = IntPtr.Zero;
			}

			if(control != null) control = null;
		}
        
		private bool CreateGLSurface() {
			int pixelFormat = 0;

			hDC = User.GetDC(control.Handle);
			if(hDC == IntPtr.Zero) { 
				KillGLWindow();
				return false;
			}
            
			Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();

			pfd.nSize = (short) Marshal.SizeOf(pfd);
			pfd.nVersion = 1;
			pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DOUBLEBUFFER;
			pfd.iPixelType = (byte) Gdi.PFD_TYPE_RGBA;  // RGBA
			pfd.cColorBits = (byte) 32;                 // Color depth
			pfd.cRedBits = 0;
			pfd.cRedShift = 0;
			pfd.cGreenBits = 0;
			pfd.cGreenShift = 0;
			pfd.cBlueBits = 0;
			pfd.cBlueShift = 0;
			pfd.cAlphaBits = 0;
			pfd.cAlphaShift = 0;
			pfd.cAccumBits = 0;
			pfd.cAccumRedBits = 0;
			pfd.cAccumGreenBits = 0;
			pfd.cAccumBlueBits = 0;
			pfd.cAccumAlphaBits = 0;
			pfd.cDepthBits = 32;                        // 32 bit Z
			pfd.cStencilBits = 0;
			pfd.cAuxBuffers = 0;
			pfd.iLayerType = (byte) Gdi.PFD_MAIN_PLANE;
			pfd.bReserved = 0;
			pfd.dwLayerMask = 0;
			pfd.dwVisibleMask = 0;
			pfd.dwDamageMask = 0;

			pixelFormat = Gdi.ChoosePixelFormat(hDC, ref pfd);
			
			if(pixelFormat == 0) {
				KillGLWindow();
				return false;
			}

			if(!Gdi.SetPixelFormat(hDC, pixelFormat, ref pfd)) {
				KillGLWindow();
				return false;
			}

			hRC = Wgl.wglCreateContext(hDC);
			if(hRC == IntPtr.Zero) {
				KillGLWindow();
				return false;
			}

			if(!Wgl.wglMakeCurrent(hDC, hRC)) {
				KillGLWindow();
				return false;
			}
            
			return true;
		}

        public void Render(bool forceRender = false) {
            if (!renderEnabled && !forceRender)
                return;

            UpdateFPS();

            Gl.glUseProgram(shaderProg[0]);         // Use scene prog

            if (project.usePP)  // Using PP? Render to the FBO then
                Gl.glBindFramebufferEXT(Gl.GL_FRAMEBUFFER_EXT, fbo);

            bool shiftPressed = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            float speedMove = MOVE_SPEED * (shiftPressed ? 0.02f : 1.0f);
            float speedLook = MOUSE_LOOK_SPEED * (shiftPressed ? 0.02f : 1.0f);

            if (Properties.Settings.Default.enableCamControls) {
                if (keys[(int)Keys.W] || keys[(int)Keys.Up])
                    camera.Move(-speedMove * editStep);
                if (keys[(int)Keys.S] || keys[(int)Keys.Down])
                    camera.Move(speedMove * editStep);
                if (keys[(int)Keys.A] || keys[(int)Keys.Left])
                    camera.Strafe(-speedMove * editStep);
                if (keys[(int)Keys.D] || keys[(int)Keys.Right])
                    camera.Strafe(speedMove * editStep);
                if (keys[(int)Keys.PageUp] || keys[(int)Keys.R])
                    camera.Crane(speedMove * editStep);
                if (keys[(int)Keys.PageDown] || keys[(int)Keys.F])
                    camera.Crane(-speedMove * editStep);
                if (mouseRDown) {
                    if (!rmp) {
                        mouseStartPos = mouseCurrentPos;
                        rmp = true;
                    }

                    PointF mouseMovement = new PointF();

                    mouseMovement.X = (mouseCurrentPos.X - mouseStartPos.X) * speedLook;
                    mouseMovement.Y = (mouseCurrentPos.Y - mouseStartPos.Y) * speedLook;

                    camera.Mouselook(mouseMovement);
                    mouseStartPos = mouseCurrentPos;
                } else
                    rmp = false;

                camera.UpdateVectors();

                //float[] forward = { camera.Forward.x, camera.Forward.y, camera.Forward.z };
                float[] angles = { camera.Rotation.x, camera.Rotation.y, camera.Rotation.z };
                //float[] up = { camera.Up.x, camera.Up.y, camera.Up.z };
                float[] campos = { camera.Position.x, camera.Position.y, camera.Position.z };
                Gl.glUniform3fv(Gl.glGetUniformLocation(shaderProg[0], "cp"), 1, campos);
                //AppForm.GetInstance().ConcatLog(angles.ToString());
                Gl.glUniform3fv(Gl.glGetUniformLocation(shaderProg[0], "cr"), 1, angles);
                //Gl.glUniform3fv(Gl.glGetUniformLocation(shaderProg[0], "up"), 1, up);
            }

            if (Properties.Settings.Default.enableStandardUniforms) {
                float[] standardUniforms = {control.Width, control.Height, GetTrackTime()};
                Gl.glUniform3fv(Gl.glGetUniformLocation(shaderProg[0], "u"), 1, standardUniforms);
            }

            if (project.use4klangEnv) {
                float[] syncVals = AppForm.GetInstance().musicPlayer.Get4klangSync();
                if (syncVals!=null)
                    Gl.glUniform1fv(Gl.glGetUniformLocation(shaderProg[0], "ev"), syncVals.Length, syncVals);
            }

            Gl.glRects(-1, -1, 1, 1);   // Render scene to current target (screen if no PP, or FBO if using PP)

            if (project.usePP) {
                Gl.glBindFramebufferEXT(Gl.GL_FRAMEBUFFER_EXT, 0);  // Unbind the fbo
                Gl.glUseProgram(shaderProg[1]);                     // Use the PP prog
                Gl.glActiveTexture(Gl.GL_TEXTURE0);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, rtt);
                Gl.glActiveTexture(Gl.GL_TEXTURE1);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, rtt2);
                //Gl.glUniform1i(Gl.glGetUniformLocation(shaderProg[1], "fr"), 0);
                //Gl.glUniform1i(Gl.glGetUniformLocation(shaderProg[1], "fr"), 1);
                if (Properties.Settings.Default.enableStandardUniforms) {
                    float[] standardUniforms = { control.Width, control.Height, GetTrackTime() };
                    Gl.glUniform3fv(Gl.glGetUniformLocation(shaderProg[1], "u"), 1, standardUniforms);
                }
                Gl.glRects(-1, -1, 1, 1);   // Render PP to screen
            }

            if (Properties.Settings.Default.enableCamControls) {
                Gl.glUseProgram(0);
                RenderAxes();
            }
            
            Gdi.SwapBuffers(hDC);
		}

		private void UpdateFPS() {
			numFrames++;
			float currentUpdate = GetSystemTime();
			if(currentUpdate - lastUpdate > fpsUpdateInterval) {
				fps = numFrames / (currentUpdate - lastUpdate);
				lastUpdate = currentUpdate;
				numFrames = 0;
			}

			frameTime = currentUpdate-lastFrameTime;
			lastFrameTime = currentUpdate;
		}

        private void Resize(object sender, EventArgs e) {
            if (control != null) {
                int height = control.Height;

                if (height == 0)
                    height = 1;

                Gl.glViewport(0, 0, control.Width, height);
                BuildRenderTarget();
            }
        }

        // Lol so fixed pipeline
        public void RenderAxes() {
            GraphicsManager gfx = GraphicsManager.GetInstance();
            
            Gl.glLineStipple(1, 0x0101);

            Gl.glPushAttrib(Gl.GL_ENABLE_BIT | Gl.GL_LIGHTING_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glDisable(Gl.GL_LIGHTING);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();
            Glu.gluOrtho2D(0, control.Width, 0, control.Height);
            Gl.glTranslated(0, 0, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPushMatrix();

            double lineLength = 50.0f;
            double lineOriginPosX = 60.0;
            double lineOriginPosY = 60.0;
            double xx, xy, yx, yy, zx, zy;

            float[] fvViewMatrix = new float[16];
            Gl.glGetFloatv(Gl.GL_MODELVIEW_MATRIX, fvViewMatrix);
            Gl.glLoadIdentity();

            xx = lineLength * fvViewMatrix[0];
            xy = lineLength * fvViewMatrix[1];
            yx = lineLength * fvViewMatrix[4];
            yy = lineLength * fvViewMatrix[5];
            zx = lineLength * fvViewMatrix[8];
            zy = lineLength * fvViewMatrix[9];

            Gl.glEnable(Gl.GL_LINE_STIPPLE);

            Gl.glBegin(Gl.GL_LINES);

            Gl.glColor3f(1.0f, 0, 0);
            Gl.glVertex2d(lineOriginPosX - xx, lineOriginPosY - xy);
            Gl.glVertex2d(lineOriginPosX, lineOriginPosY);

            Gl.glColor3f(0, 1.0f, 0);
            Gl.glVertex2d(lineOriginPosX - yx, lineOriginPosY - yy);
            Gl.glVertex2d(lineOriginPosX, lineOriginPosY);

            Gl.glColor3f(0, 0, 1.0f);
            Gl.glVertex2d(lineOriginPosX - zx, lineOriginPosY - zy);
            Gl.glVertex2d(lineOriginPosX, lineOriginPosY);

            Gl.glEnd();

            Gl.glDisable(Gl.GL_LINE_STIPPLE);

            Gl.glBegin(Gl.GL_LINES);

            Gl.glColor3f(1.0f, 0, 0);
            Gl.glVertex2d(lineOriginPosX, lineOriginPosY);
            Gl.glVertex2d(lineOriginPosX + xx, lineOriginPosY + xy);

            Gl.glColor3f(0, 1.0f, 0);
            Gl.glVertex2d(lineOriginPosX, lineOriginPosY);
            Gl.glVertex2d(lineOriginPosX + yx, lineOriginPosY + yy);

            Gl.glColor3f(0, 0, 1.0f);
            Gl.glVertex2d(lineOriginPosX, lineOriginPosY);
            Gl.glVertex2d(lineOriginPosX + zx, lineOriginPosY + zy);

            Gl.glEnd();

            Gl.glColor3f(1.0f, 1.0f, 1.0f);

            Gl.glPushMatrix();
            Gl.glTranslated(lineOriginPosX + xx, lineOriginPosY + xy, 0);
            debugLabeller.Print("X");
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(lineOriginPosX + yx, lineOriginPosY + yy, 0);
            debugLabeller.Print("Y");
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(lineOriginPosX + zx, lineOriginPosY + zy, 0);
            debugLabeller.Print("Z");
            Gl.glPopMatrix();

            Gl.glPopAttrib();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPopMatrix();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPopMatrix();
        }

        public float GetSystemTime() {
            ulong ticks;
            float time;
            QueryPerformanceCounter(out ticks);
            time = (float)ticks / (float)ticksPerSecond;

            return time;
        }

        public float GetTrackTime() {
            AppForm af = AppForm.GetInstance();
            float t = 0;
            try {
                t = (float)af.musicPlayer.GetPosition();
            } catch (Exception) { }
            return t;
        }

		public float GetFPS() {
			return fps;
		}

        public void SetEditStep(float newEditStep) {
            editStep = newEditStep;
        }

        public float GetEditStep() {
            return editStep;
        }
        
        public void SetCameraMode(CameraMode newCamMode) {
            camera.Mode = newCamMode;
        }

        public Camera GetCamera() {
            return camera;
        }

        public bool GetRenderEnabled() {
            return renderEnabled;
        }

        public void SetRenderEnabled(bool b) {
            renderEnabled = b;
        }
        public void updateProject(Project project) {
            this.project = project;
        }
	}
}

