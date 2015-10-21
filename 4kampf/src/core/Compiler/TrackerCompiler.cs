using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kampfpanzerin.src.core.Compiler {
    class TrackerCompiler {
        public static string CompileSyncTrackerCode(List<TimelineBar> bars) {
            if (bars.Count == 0) {
                return "";
            }

            string res = "";
            int i = 0;
            foreach (TimelineBar bar in bars) {
                if (bar.events.Count == 0)
                    continue;

                if (bars.Count == 1)
                    res += "sn=";
                else
                    res += "sn[" + (i++) + "]=";

                string current = CompileTrack(bar);
                res += current;
            }
            return res;
        }

        public static string SyncVars(List<TimelineBar> bars) {
            if (bars.Count == 0)
                return "";

            if (bars.Count == 1)
                return "\r\nvec3 sn;";
            
            return "\r\nvec3 sn[" + bars.Count + "];";
        }

        public static string CompileTrack(TimelineBar bar) {
            string current = "{0};";

            for (int i = 0; i < bar.events.Count; i++) {
                TimelineBarEvent be = bar.events[i];
                string startVal = ToOptimisedString(be.value);
                string startTime = ToOptimisedString(be.time);
                if (i == bar.events.Count - 1) {
                    current = string.Format(current, startVal);
                    break;
                } else {
                    string stopVal = ToOptimisedString(bar.events[i + 1].value);
                    string stopTime = ToOptimisedString(bar.events[i + 1].time);
                    switch (be.type) {
                        case BarEventType.HOLD:
                            current = string.Format(current, "fx(" + stopTime + "," + startVal + ",{0})");
                            break;
                        case BarEventType.LERP: {
                                string interp = string.Format("mx({0},{1},{2},{3})", startTime, stopTime, startVal, "{0}");
                                current = string.Format(current, "mx(" + startTime + "," + stopTime + "," + startVal + ",{0})");
                                break;
                            }
                        case BarEventType.SMOOTH: {
                                string interp = string.Format("sx({0},{1},{2},{3})", startTime, stopTime, startVal, "{0}");
                                current = string.Format(current, interp);
                                break;
                            }
                    }
                }
            }
            return current;
        }

        public static string CompileCamTrackerCode(List<TimelineBar> bars) {
            if (bars.Count == 0) {
                return "";
            }

            var cp = "cp=" + CompileTrack(bars[0]);
            var rot = "or=" + CompileTrack(bars[1]);
            return cp + rot;
        }

        public static string GetInterpolationCode(List<TimelineBar> bars0, List<TimelineBar> bars1) {
            List<TimelineBar>[] bars = { bars0, bars1 };
            IEnumerable<BarEventType> types = bars.SelectMany(b => b).SelectMany(b => b.events).GroupBy(e => e.type).Select(Enumerable.First).Select(e => e.type);
            IEnumerable <string> strings = types.Select(t => {
                switch (t) {
                    case BarEventType.HOLD: return "#define fx(b,bb,bbb) u.z<b?bb:bbb\r\n";
                    case BarEventType.LERP: return "#define mx(b,bb,bbb,bbbb) u.z<bb?mix(bbb,bbbb,(u.z-b)/(bb-b)):bbbb\r\n";
                    case BarEventType.SMOOTH: return "#define sx(b,bb,bbb,bbbb) u.z<bb?bbb+(bbbb-bbb)*smoothstep(b,bb,u.z):bbbb\r\n";
                    default: throw new NotImplementedException();
                }
            });
            return string.Concat(strings.ToArray());
        }


        public static string ToOptimisedString(float f, bool intAllowed = false) {
            string s = f.ToString(".000", Kampfpanzerin.culture);
            if (s.StartsWith("-0."))
                s = s.Substring(1);
            if (s.StartsWith("0."))
                s = s.Substring(1);
            if (s.Contains(".")) {
                while (s.EndsWith("0") && s.Length > 2)
                    s = s.Substring(0, s.Length - 1);
            }
            if (s.Equals(".0")
                && intAllowed)
                s = "0";
            else if (intAllowed && s.EndsWith(".")) {
                s = s.Substring(0, s.Length - 1);
            }

            return s;
        }

        public static string ToOptimisedString(Vector3f v, bool intAllowed = true) {
            return String.Format("vec3({0},{1},{2})", ToOptimisedString(v.x, intAllowed), ToOptimisedString(v.y, intAllowed), ToOptimisedString(v.z, intAllowed));
        }

    }

            
}
