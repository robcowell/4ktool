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
    
    [Serializable]
    public class TimelineBarEvent : IComparable {
        public float time;
        public float value;
        public BarEventType type;

        public Vector3f vecValue;

        public int CompareTo(Object obj) {
            TimelineBarEvent be = (TimelineBarEvent)obj;
            if (time == be.time)
                return 0;
            if (time < be.time)
                return -1;
            return 1;
        }
    }
}
