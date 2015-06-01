using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin {
    class CameraEvent: TimelineBarEvent {
        public Vector3f position;
        public Vector3f up;
        public Vector3f forward;

        public CameraEvent() {
            this.type = BarEventType.CAMERA;
        }
    }
}
