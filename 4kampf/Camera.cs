using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

using Tao.OpenGl;

namespace kampfpanzerin
{
    public enum CameraMode
    {
        WALK,
        LOCKFLY,
        FREEFLY
    }

    public class Camera
    {
        private const float PI_OVER_180 = 0.0174532925f;

        public Vector3f position;
        public Vector3f rotation;
        public Vector3f forward, right, up;
        public CameraMode mode = CameraMode.FREEFLY;
        
        private bool dirty;

        public Camera() {
            Reset();
        }

        public bool CheckAndResetDirty() {
            bool d = dirty;
            dirty = false;
            return d;
        }

        public void Reset() {
            rotation = new Vector3f(0, 180.0f, 0);
            position = new Vector3f(0, 0, 0);
            forward = new Vector3f();
            right = new Vector3f();
            up = new Vector3f();
            dirty = true;
        }
        
        public void UpdateVectors() {
            if (rotation.x < -90) rotation.x = -90;
            if (rotation.x > 90) rotation.x = 90;

            Gl.glMatrixMode(Gl.GL_MODELVIEW); 
            Gl.glLoadIdentity();
            Gl.glRotatef(rotation.x, 1, 0, 0);
            Gl.glRotatef(360 - rotation.y, 0, 1, 0);

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
                position.x -= (float)Math.Sin(rotation.y * PI_OVER_180) * amount;
                position.z -= (float)Math.Cos(rotation.y * PI_OVER_180) * amount;
            } else if (mode == CameraMode.FREEFLY)
                position -= (forward * amount);

            dirty = true;
        }

        public void Strafe(float amount) {
            GraphicsManager gfx = GraphicsManager.GetInstance();

            if (mode == CameraMode.WALK || mode == CameraMode.LOCKFLY) {
                position.x -= (float)Math.Sin((rotation.y + 90) * PI_OVER_180) * amount;
                position.z -= (float)Math.Cos((rotation.y + 90) * PI_OVER_180) * amount;
            } else if (mode == CameraMode.FREEFLY)
                position -= (right * amount);

            dirty = true;
        }

        public void Crane(float amount) {
            GraphicsManager gfx = GraphicsManager.GetInstance();

            if (mode == CameraMode.LOCKFLY || mode == CameraMode.FREEFLY)
                position.y += amount;

            dirty = true;
        }

        public void Mouselook(PointF mouseMovement) {
            Yaw(-mouseMovement.X);
            Pitch(-mouseMovement.Y);
        }

        public void Yaw(float amount) {
            rotation.y = (rotation.y + amount) % 360;
            dirty = true;
        }
        
        public void Pitch(float amount) {
            rotation.x += amount;
            dirty = true;
        }
    }
}
