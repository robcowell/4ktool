using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using kampfpanzerin.core.Serialization;

namespace kampfpanzerin {
    class BuildUtils {
        public static void DoExportHeader(Project p, float length, List<TimelineBar> syncBars, /*string camCode, */string vertexShader, string fragmentShader, string ppShader = null) {
            string s = "// Prod shaders, sync and config\n// Exported from 4kampf\n\n#pragma once\n\n";
            if (Properties.Settings.Default.enableStandardUniforms)
                s += "#define USE_STANDARD_UNIFORMS\n";
            if (p.use4klangEnv)
                s += "#define USE_4KLANG_ENV_SYNC\n";
            if (p.usePP)
                s += "#define USE_PP\n";
            if (Properties.Settings.Default.useSoundThread)
                s += "#define USE_SOUND_THREAD\n";
            if (p.useClinkster)
                s += "#define USE_CLINKSTER\n";
            
            //vertexShader = ReplaceShaderMacros(vertexShader, syncCode, camCode, true);
            //fragmentShader = ReplaceShaderMacros(fragmentShader, syncCode, camCode, true);
            //ppShader = ReplaceShaderMacros(ppShader, syncCode, camCode, true);
            vertexShader = vertexShader.Replace("CAMVARS", "vec3 cp, fd, up;");

            s += "#define PROD_LENGTH " + ((int)length) + "\n\n";
            s += "#pragma data_seg(\".vertShader\")\nstatic const char *vertShader[] = {\"";
            s += CleanShader(vertexShader);
            s += "\"};\n\n#pragma data_seg(\".fragShader\")\nstatic const char *fragShader[] = {\"";
            s += CleanShader(fragmentShader);
            if (ppShader != null) {
                s += "\"};\n\n#pragma data_seg(\".ppShader\")\nstatic const char *ppShader[] = {\"";
                s += CleanShader(ppShader);
            }
            s += "\"};\n";
            File.WriteAllText("4kampfpanzerin.h", s);
        }



        private static string CleanShader(string s) {
            Dictionary<string,string> replacements = new Dictionary<string, string>();
            string r = "";
            string[] lines = s.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Count(); i++) {
                // Substitute AUTOREP
                if (lines[i].IndexOf("// AUTOREP") != -1 && lines[i].StartsWith("#define")) {
                    string t = lines[i].Substring(8);
                    string varName = t.Substring(0, t.IndexOf(" "));
                    string varValue = t.Substring(t.IndexOf(" ") + 1).Replace("// AUTOREP","").Trim();
                    replacements.Add(varName,varValue);
                    continue;
                }
                // Remove comments
                int commentPos = lines[i].IndexOf("//");
                if (commentPos == 0)
                    continue;
                if (commentPos > 0)
                    lines[i] = lines[i].Substring(0, commentPos - 1);
                // Trim
                lines[i] = lines[i].Trim();
                // Add newlines for lines starting #
                if (lines[i].StartsWith("#"))
                    lines[i] += "\\n";
                // Add spaces after braceless elses
                if (lines[i].EndsWith("else"))
                    lines[i] += " ";
                // Do replacements
                foreach (var pair in replacements)
                    lines[i] = lines[i].Replace(pair.Key, pair.Value);
                // Strip tabs from stuff like material tables
                lines[i] = lines[i].Replace("\t", "");
                // Got anything left? Cool, add it!
                if (lines[i].Length > 0)
                    r += lines[i];
            }
            return r;
        }

        public static string ReplaceShaderMacros(string shader, string syncCode, string camCode, bool prodmode = true) {
            if (shader == null) {
                return null;
            }
            shader = shader.Replace("SYNCCODE", syncCode);
            //shader = shader.Replace("CAMVARS", (prodmode? "" : "uniform ") + "vec3 cp, cr;");

            if (!prodmode) {
                shader = shader.Replace("\n", "\r\n");
            }
            return shader;
        }

    }
}
