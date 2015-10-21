using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Tao.OpenGl;

namespace kampfpanzerin
{
    public enum CameraMode
    {
        WALK,
        LOCKFLY,
        FREEFLY,
        AUTOMATED
    }

    public class Camera
    {
        private const float PI_OVER_180 = 0.0174532925f;
        private static readonly Vector3f CAM_INVERT_X = new Vector3f(-1, 1, 1);
        private Vector3f position;
        private Vector3f rotation;
        private Vector3f forward, right, up;
        public Vector3f Forward {
            get {
                return forward;
            }
        }
        public Vector3f Up {
            get {
                return up;
            }
        }

        public Vector3f Right {
            get {
                return right;
            }
        }

        public Vector3f Rotation {
            get {
                if (mode == CameraMode.AUTOMATED) {
                    return automation.Rot;
                }
                return rotation;
            }
        }
        
        public Vector3f Position {
            get {
                if (mode == CameraMode.AUTOMATED)
                    return automation.Pos;
                return position;
            }
        }

        private CamAutomationProxy automation;
        private CameraMode mode;


        public CameraMode Mode {
                get {
                    return mode;
                }
                set {
                    if (value != mode) {
                        if (value == CameraMode.AUTOMATED) {
                            List<TimelineBar> bars = AppForm.GetInstance().timeLine.camBars;
                            Dictionary<TimelineBar.TimeLineMode, TimelineBar> d = new Dictionary<TimelineBar.TimeLineMode, TimelineBar>();
                            d.Add(TimelineBar.TimeLineMode.CAMERA_POS, bars[0]);
                            d.Add(TimelineBar.TimeLineMode.CAMERA_ROT, bars[1]);

                            automation = new CamAutomationProxy(d, AppForm.GetInstance().musicPlayer);
                        } else if(mode == CameraMode.AUTOMATED) {

                            this.position = automation.Pos.Clone();
                            this.rotation = automation.Rot.Clone();
                            UpdateVectors();
                        }
                        mode = value;
                        UpdateVectors();
                    }
                }
        }
        
        private bool dirty;

        public Camera() {
            Reset();
            mode = CameraMode.FREEFLY ;
        }

        public bool CheckAndResetDirty() {
            bool d = dirty;
            dirty = false;
            return d;
        }

        public void Reset() {
            rotation = new Vector3f(0, (float)Math.PI, 0);
            position = new Vector3f(0, 0, 0);
            forward = new Vector3f();
            right = new Vector3f();
            up = new Vector3f();
            dirty = true;
        }

        public void UpdateVectors() {
            if (Rotation.x < -90) Rotation.x = -90;
            if (Rotation.x > 90) Rotation.x = 90;

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glRotatef(Rotation.x / PI_OVER_180, 1, 0, 0);
            Gl.glRotatef(360 - Rotation.y / PI_OVER_180, 0, 1, 0);
            Gl.glRotatef(Rotation.z / PI_OVER_180, 0, 0, 1);

            // Store orientation vectors!
            float[] model = new float[16];
            Gl.glGetFloatv(Gl.GL_MODELVIEW_MATRIX, model);
            forward.Set(model[2], model[6], model[10]);
            right.Set(model[0], model[4], model[8]);
            up.Set(model[1], model[5], model[9]);
        }

        public void Move(float amount) {
            GraphicsManager gfx = GraphicsManager.GetInstance();

            if (mode == CameraMode.WALK || mode == CameraMode.LOCKFLY) {
                position.x += (float)Math.Sin(rotation.y) * amount;
                position.z += (float)Math.Cos(rotation.y) * amount;
            } else if (mode == CameraMode.FREEFLY)
                position -= (forward * CAM_INVERT_X * amount);

            dirty = true;
        }

        public void Strafe(float amount) {
            GraphicsManager gfx = GraphicsManager.GetInstance();

            if (mode == CameraMode.WALK || mode == CameraMode.LOCKFLY) {
                position.x += (float)Math.Sin((rotation.y + Math.PI / 2)) * amount;
                position.z += (float)Math.Cos((rotation.y + Math.PI / 2)) * amount;
            } else if (mode == CameraMode.FREEFLY)
                position -= (right * CAM_INVERT_X * amount);

            dirty = true;
        }

        public void Crane(float amount) {
            GraphicsManager gfx = GraphicsManager.GetInstance();

            if (mode == CameraMode.LOCKFLY || mode == CameraMode.FREEFLY)
                position.y += amount;

            dirty = true;
        }

        public void Mouselook(PointF mouseMovement) {
            if (mode != CameraMode.AUTOMATED) {
                Yaw(-mouseMovement.X);
                Pitch(-mouseMovement.Y);
            }
        }

        public void Yaw(float amount) {
            if (mode != CameraMode.AUTOMATED) {
                rotation.y = (rotation.y + amount * PI_OVER_180) % 360;
                dirty = true;
            }
        }
        
        public void Pitch(float amount) {
            if (mode != CameraMode.AUTOMATED) {
                rotation.x = (rotation.x + amount * PI_OVER_180) % 360;
                dirty = true;
            }
        }
    }
}
