using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin.core.Serialization {
    [Serializable]
    public class Project {
        
        public List<TimelineBar> camBars { get; set; }
        public List<TimelineBar> syncBars { get; set; }
        public bool enableStandardUniforms;

        public bool useClinkster = false;

        public bool use4klangEnv = false;
        public bool usePP = false;
        public bool useSoundThread = false;
        public bool useBitBucket = false;

        public BitBucketData bitBucketSettings;

        public string gitRemote;
        public string name;
    }
}
