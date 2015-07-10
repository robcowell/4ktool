using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin.log {
    class Logger {
        public static void log(string msg) {
            AppForm.GetInstance().ConcatLog(msg);
        }
    }
}
