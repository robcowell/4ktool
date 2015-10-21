using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin.log {
    class Logger {
        public static void clear() {
            AppForm.GetInstance().ClearLog(); 
        }
        public static void log(string msg) {
            AppForm.GetInstance().ConcatLog(msg);
        }
        public static void logf(string format, params object[] args) {
            log(string.Format(format, args));
        }
    }
}
