using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin {
    [Serializable]
    public class TimelineBar {
        public string name;

        [NonSerialized]
        public List<TimelineBarEvent> events = new List<TimelineBarEvent>();

        [NonSerialized]
        public float maxVal, minVal;

        public TimeLineMode mode { get; set; }

        public enum TimeLineMode {
            SYNC, CAMERA_POS, CAMERA_ROT
        };

        public TimelineBar() { }

        public TimelineBar(string name)
            : this(name, TimeLineMode.SYNC) {
        }

        public TimelineBar(string name, TimeLineMode mode) {
            this.name = name;
            this.mode = mode;
        }

        public void Recalc() {
            maxVal = -9999999;
            minVal = 9999999;
            foreach (TimelineBarEvent be in events) {
                maxVal = Math.Max(be.value.Max(), maxVal);
                minVal = Math.Min(be.value.Min(), minVal);
            }
            

            events.Sort();
        }

        public Vector3f GetValueAtTime(float t) {
            TimelineBarEvent be1 = null, be2 = null;

            foreach (TimelineBarEvent be in events) {
                if (be.time > t) {
                    be2 = be;
                    break;
                } else
                    be1 = be;
            }

            if (be1 == null) {
                if (be2 != null) {
                    return be2.value;
                }
                return Vector3f.INVALID;
            }
            if (be1.type == BarEventType.HOLD)
                return be1.value;

            if (be2 == null)
                return be1.value;

            float amount = (t - be1.time) / (be2.time - be1.time);
            if (be1.type == BarEventType.SMOOTH)
                amount = (amount * amount) * (3.0f - (2.0f * amount));
            return be1.value + (be2.value - be1.value) * amount;
        }

        public Vector3f GetVectorValueAtTime(float t) {
            return GetValueAtTime(t);
        }
        //    TimelineBarEvent be1 = null, be2 = null;

        //    foreach (TimelineBarEvent be in events) {
        //        if (be.time > t) {
        //            be2 = be;
        //            break;
        //        } else
        //            be1 = be;
        //    }

        //    if (be1 == null || be2 == null)
        //        return new Vector3f(0);

        //    float amount = (t - be1.time) / (be2.time - be1.time);
        //    amount = (amount * amount) * (3.0f - (2.0f * amount));
        //    return be1.value + (be2.value - be1.value) * amount;
        //}
    }
}
