using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace kampfpanzerin {
    public class TabControlFlat : System.Windows.Forms.TabControl {
        public TabControlFlat() {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e) {
           // base.OnPaint(e);
            if (TabCount <= 0) 
                return;

            e.Graphics.Clear(Color.FromArgb(64, 64, 64));

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            SolidBrush PaintBrush = new SolidBrush(BackColor);

            for (int index = 0; index <= TabCount - 1; index++) {
                TabPage tp = TabPages[index];
                tp.Padding = new Padding(0);
                tp.Margin = new Padding(0);
                
                Rectangle r = GetTabRect(index);
                r.X += 2;
                r.Y -= 2;
                r.Height += 4;

                if (index == SelectedIndex)
                    PaintBrush.Color = Color.FromArgb(32, 32, 32);
                else
                    PaintBrush.Color = Color.FromArgb(64,64,64);
                
                e.Graphics.FillRectangle(PaintBrush, r);

                if (index == SelectedIndex)
                    PaintBrush.Color = Color.White;
                else
                    PaintBrush.Color = Color.FromArgb(224, 224, 224);
                
                if (tp.Enabled)
                    e.Graphics.DrawString(tp.Text, Font, PaintBrush, (RectangleF)r, sf);
                else
                    ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, tp.BackColor, (RectangleF)r, sf);
            }
            PaintBrush.Dispose();
        }
    }
}
