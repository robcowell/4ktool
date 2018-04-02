using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace kampfpanzerin.core.Serialization {
    public enum Synth { undefined, vierklang, clinkster, oidos };
    [Serializable]
    public class Project {
        public bool enableStandardUniforms;

        public bool useClinkster = false;

        public Synth synth = Synth.undefined;

        public bool use4klangEnv = false;
        public bool usePP = false;
        public bool useSoundThread = false;
        public bool useBitBucket = false;
        public bool useVertShader = true;

        public BitBucketData bitBucketSettings;

        public string gitRemote;
        public string name;

        internal bool FixLegacy() {
            if (synth == Synth.undefined) {
                synth = useClinkster ? Synth.clinkster : Synth.vierklang;
                return true;
            }
            return false;
        }
    }
}
