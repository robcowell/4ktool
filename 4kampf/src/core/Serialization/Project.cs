using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace kampfpanzerin.core.Serialization {
    public enum Synth { vierklang, clinkster, oidos };
    [Serializable]
    public class Project {
        public bool enableStandardUniforms;

        public bool useClinkster = false;

        public Synth synth = Synth.vierklang;

        public bool use4klangEnv = false;
        public bool usePP = false;
        public bool useSoundThread = false;
        public bool useBitBucket = false;
        public bool useVertShader = true;

        public BitBucketData bitBucketSettings;

        public string gitRemote;
        public string name;
    }
}
