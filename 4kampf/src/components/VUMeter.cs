using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kampfpanzerin {
    public partial class VUMeter : UserControl {
        public float[] channelLevels = null;
            
        public VUMeter() {
            InitializeComponent();
        }

        public void Redraw() {
            if (channelLevels == null)
                return;

            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(BackColor);

            int colWidth = Width / channelLevels.Length;
            int x = 0;
            SolidBrush b = new SolidBrush(ForeColor);
            for (int i = 0; i < channelLevels.Length; i++) {
                int val = Height - (int)(channelLevels[i] * Height);
                g.FillRectangle(b, new Rectangle(x,val,colWidth-colWidth/5,Height-val));
                x += colWidth;
            }

            g.Dispose();
            pb.Image = bmp;
        }
    }
}
