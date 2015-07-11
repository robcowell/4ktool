using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin {
    [Serializable]
    public class Project {
        
        public List<TimelineBar> camBars { get; set; }
        public List<TimelineBar> syncBars { get; set; }
        public bool enableStandardUniforms;

        public bool useClinkster = false;

        public bool use4klangEnv = false;
        public bool usePP = false;
        public bool useSoundThread = false;
        public bool useButBucket;
    }
}
