using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace kampfpanzerin {
    public enum BarEventType {
        HOLD,
        LERP,
        SMOOTH,
        CAMERA
    }
    
    public class TimelineBarEvent : IComparable {
        public float time;
        public float value;
        public BarEventType type;

        public int CompareTo(Object obj) {
            TimelineBarEvent be = (TimelineBarEvent)obj;
            if (time == be.time)
                return 0;
            if (time < be.time)
                return -1;
            return 1;
        }

        public void Draw(Graphics g, int x, int y, int height, Color c) {
            Pen p = new Pen(c, 1.0f);
            SolidBrush b = new SolidBrush(c);

            g.DrawLine(p, x, y + height / 2, x, y - height / 2);

            if (height < 16)
                return;

            y = y + height / 2;
            if (height % 2 == 1)
                y++;

            Point[] Points = new Point[3];
            Points[0] = new Point(x, y - 4);
            Points[1] = new Point(x + 4, y);
            Points[2] = new Point(x - 4, y);
            g.FillPolygon(b, Points);

            y -= height;
            Points[0] = new Point(x, y + 4);
            Points[1] = new Point(x + 4, y);
            Points[2] = new Point(x - 4, y);
            g.FillPolygon(b, Points);
        }

    }
}
