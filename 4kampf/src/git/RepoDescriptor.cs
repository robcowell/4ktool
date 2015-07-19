using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin.git {
    class RepoDescriptor {

        public string Name {
            get;
            set;
        }
        public string Slug {
            get;
            set;
        }
        public string Clone {
            get;
            set;
        }
        public string Description {
            get;
            set;
        }

        public string ToString() {
            return Name;
        }
    }
}
