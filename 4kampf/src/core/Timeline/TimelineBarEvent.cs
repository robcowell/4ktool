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
        //CAMERA
    }
    
    [Serializable]
    public class TimelineBarEvent : IComparable {
        public float time;

        private Vector3f Value;
        public Vector3f value {
            get{
                return Value;
            }
            set {
                this.Value = value;
            }
        }
        public BarEventType type;

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
