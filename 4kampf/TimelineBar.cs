using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin {
    public class TimelineBar {
        public string name;
        public bool selected = false;
        public List<TimelineBarEvent> events = new List<TimelineBarEvent>();
        public float maxVal, minVal;

        public TimeLineMode mode { get; set; }

        public enum TimeLineMode {
            SYNC, CAMERA
        };

        public TimelineBar(string name)
            : this(name, TimeLineMode.SYNC) {
        }

        public TimelineBar(string name, TimeLineMode mode) {
            this.name = name;
            this.mode = mode;
        }

        public void Recalc() {
            maxVal = 0;
            minVal = 9999999;
            foreach (TimelineBarEvent be in events) {
                if (be.value > maxVal)
                    maxVal = be.value;
                if (be.value < minVal)
                    minVal = be.value;
            }

            events.Sort();
        }

        public float GetValueAtTime(float t) {
            TimelineBarEvent be1 = null, be2 = null;

            foreach (TimelineBarEvent be in events) {
                if (be.time > t) {
                    be2 = be;
                    break;
                } else
                    be1 = be;
            }

            if (be1 == null)
                return -666666.0f;

            if (be1.type == BarEventType.HOLD)
                return be1.value;

            if (be2 == null)
                return -666666.0f;

            float amount = (t - be1.time) / (be2.time - be1.time);
            if (be1.type == BarEventType.SMOOTH)
                amount = (amount * amount) * (3.0f - (2.0f * amount));
            return be1.value + (be2.value - be1.value) * amount;
        }

    }
}
