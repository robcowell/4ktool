using System;
using System.Collections.Generic;
using System.Linq;

namespace _4kampf.Shared.Utilities;

/// <summary>
/// Shader processing utilities.
/// Platform-agnostic shader cleaning and processing functions.
/// </summary>
public static class ShaderUtils
{
    /// <summary>
    /// Cleans shader code for export by removing comments, handling AUTOREP directives, etc.
    /// </summary>
    /// <param name="shaderCode">Raw shader code</param>
    /// <returns>Cleaned shader code suitable for embedding</returns>
    public static string CleanShader(string shaderCode)
    {
        if (string.IsNullOrEmpty(shaderCode))
        {
            return string.Empty;
        }

        Dictionary<string, string> replacements = new Dictionary<string, string>();
        string result = "";
        string[] lines = shaderCode.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
        
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            
            // Substitute AUTOREP
            if (line.IndexOf("// AUTOREP") != -1 && line.StartsWith("#define"))
            {
                string trimmed = line.Substring(8).Trim();
                int spaceIndex = trimmed.IndexOf(" ");
                if (spaceIndex > 0)
                {
                    string varName = trimmed.Substring(0, spaceIndex);
                    string varValue = trimmed.Substring(spaceIndex + 1)
                        .Replace("// AUTOREP", "")
                        .Trim();
                    replacements.Add(varName, varValue);
                }
                continue;
            }
            
            // Remove comments
            int commentPos = line.IndexOf("//");
            if (commentPos == 0)
            {
                continue; // Entire line is a comment
            }
            if (commentPos > 0)
            {
                line = line.Substring(0, commentPos).TrimEnd();
            }
            
            // Trim
            line = line.Trim();
            
            // Add newlines for lines starting with #
            if (line.StartsWith("#"))
            {
                if (i > 0 && !lines[i - 1].EndsWith("\\n"))
                {
                    line = "\\n" + line;
                }
                line += "\\n";
            }
            
            // Add spaces after braceless elses
            if (line.EndsWith("else"))
            {
                line += " ";
            }
            
            // Do replacements
            foreach (var pair in replacements)
            {
                line = line.Replace(pair.Key, pair.Value);
            }
            
            // Strip tabs from stuff like material tables
            line = line.Replace("\t", "");
            
            // Got anything left? Cool, add it!
            if (line.Length > 0)
            {
                result += line;
            }
        }
        
        return result;
    }
}

