using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin {
    public class CamAutomationProxy {

        public Vector3f Pos {
            get {
                return GetCamAutoValue(TimelineBar.TimeLineMode.CAMERA_POS);
            }
        }
        
        public Vector3f Rot {
            get {
                return GetCamAutoValue(TimelineBar.TimeLineMode.CAMERA_ROT);
            }
        }

        private Dictionary<TimelineBar.TimeLineMode, TimelineBar> lanes;
        private MusicPlayer p;

        public CamAutomationProxy(Dictionary<TimelineBar.TimeLineMode, TimelineBar> lanes, MusicPlayer p) {
            this.lanes = lanes;
            this.p = p;
        }

        private Vector3f GetCamAutoValue(TimelineBar.TimeLineMode forTrack) {
            return lanes[forTrack].GetVectorValueAtTime(p.GetPosition());
        }
    }
}
