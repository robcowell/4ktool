using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace kampfpanzerin {
    class EyedropColorPicker : Control {
        private Bitmap snapshot;
        private Bitmap icon;
        private Color selectedCol;
        private float zoom = 4;
        private bool iscapturing = false;
        
        public EyedropColorPicker() {
            this.DoubleBuffered = true;
            icon = new Bitmap(Properties.Resources.Eyedropper);
        }

        RectangleF ImageRect {
            get {
                return Utils.Rect(ClientRectangle);
            }
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            RecalcSnapshotSize();
        }

        void RecalcSnapshotSize() {
            if (snapshot != null)
                snapshot.Dispose();
            RectangleF r = ImageRect;
            int w = (int)(Math.Floor(r.Width / zoom));
            int h = (int)(Math.Floor(r.Height / zoom));
            snapshot = new Bitmap(w, h);
        }

        void GetSnapshot() {
            Point p = Control.MousePosition;
            p.X -= snapshot.Width / 2;
            p.Y -= snapshot.Height / 2;

            using (Graphics dc = Graphics.FromImage(snapshot)) {
                dc.CopyFromScreen(p, new Point(0, 0), snapshot.Size);
                Refresh(); //Invalidate();

                PointF center = Utils.Center(new RectangleF(0, 0, snapshot.Size.Width, snapshot.Size.Height));
                Color c = snapshot.GetPixel((int)Math.Round(center.X), (int)Math.Round(center.Y));
                if (c != selectedCol) {
                    selectedCol = c;
                    ColorChooser cc = (ColorChooser)Parent;
                    cc.Color = selectedCol;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Rectangle rr = ClientRectangle;

            if (snapshot != null) {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                RectangleF r = RectangleF.Empty;
                r.Width = (snapshot.Size.Width + 1) * zoom + 1;
                r.Height = (snapshot.Size.Height + 1) * zoom + 1;
                r.X = 0;
                r.Y = 0;
                e.Graphics.DrawImage(snapshot, r);

                if (iscapturing) {
                    PointF center = Utils.Center(r);
                    Rectangle centerrect = new Rectangle(Utils.Point(center), new Size(0, 0));
                    centerrect.X -= ((int)zoom / 2 - 1);
                    centerrect.Y -= ((int)zoom / 2 - 1);
                    centerrect.Width = (int)zoom;
                    centerrect.Height = (int)zoom;
                    e.Graphics.DrawRectangle(Pens.Black, centerrect);
                } else
                    e.Graphics.DrawImage(icon, 0, 0);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                Cursor = Cursors.Cross;
                iscapturing = true;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                GetSnapshot();
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            Cursor = Cursors.Arrow;
            iscapturing = false;
            Invalidate();
        }
    }
}
