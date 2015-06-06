using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin {
    [Serializable]
    public class Project {
        
        public List<TimelineBar> camBars { get; set; }
        public List<TimelineBar> syncBars { get; set; }
    }
}
