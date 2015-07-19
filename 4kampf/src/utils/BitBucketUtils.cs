using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using Simple.CredentialManager;
using kampfpanzerin.core.Serialization;

namespace kampfpanzerin.utils {
    public static class BitBucketUtils {

        public static string GenerateSlug(this string phrase) {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt) {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static NetworkCredential GetCredentials(BitBucketData BitBucketConfig) {
            NetworkCredential credentials;
            Credential cred = new Credential(BitBucketConfig.UserName, "", BitBucketConfig.Team + ".bitbucket");
            if (!cred.Load()) {
                kampfpanzerin.core.UI.KampfCredentialDescriptor desc = kampfpanzerin.core.UI.CredentialPrompt.GetCredentialsVistaAndUp(cred.Target);
                credentials = desc.Credentials;
                if (desc.Remember) {
                    cred.Username = credentials.UserName;
                    cred.Password = credentials.Password;
                    cred.PersistenceType = PersistenceType.Enterprise;
                    cred.Save();
                }
                cred.Dispose();
            } else {
                credentials = new NetworkCredential();
                credentials.UserName = cred.Username;
                credentials.Password = cred.Password;
                cred.Dispose();
            }
            return credentials;
        }

        internal static void ClearCredentials(BitBucketData BitBucketConfig) {
            Credential cred = new Credential(BitBucketConfig.UserName, "", BitBucketConfig.Team + ".bitbucket");
            if (cred.Load()) {
                cred.Delete();
            }
        }

        public static string GetParentUriString(this string uri) {
            var uriP = new Uri(uri);
            return uriP.Segments.Last();
        }
    }
}
