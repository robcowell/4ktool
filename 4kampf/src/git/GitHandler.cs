using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibGit2Sharp;
using kampfpanzerin.log;
using kampfpanzerin.core.Serialization;
using Simple.CredentialManager;
using System.Net;
using RestSharp;

namespace kampfpanzerin.git {
    class GitHandler {

        private Repository repo;

        public GitHandler(string folder) {
            repo = new Repository(folder);
        }

        public static GitHandler Init(string folder, Project p) {

            Repository.Init(folder, folder);
            var gitRepo = new Repository(folder);
            // Call EnumerateFiles in a foreach-loop.

            foreach (string file in Directory.EnumerateFiles(folder,
                "*.*",
                SearchOption.AllDirectories)) {
                var relative = MakeRelativePath(folder + "/", file);
                if (!relative.StartsWith(".git")) {
                    gitRepo.Index.Add(relative);
                }
            }

            

            //Signature s = new Signature(Properties.Settings.Default.gitAuthor, Properties.Settings.Default.gitEmail, DateTime.Now);
            Commit c = gitRepo.Commit(@"Created a new intro \o/");
            Logger.log("commited " + c.ToString());
            return new GitHandler(folder);
        }
        
        public static String MakeRelativePath(String fromPath, String toPath) {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);


            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.ToUpperInvariant() == "FILE") {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativeUri.ToString().Replace('/',Path.DirectorySeparatorChar);
        }

        public static bool CheckForCredentials(BitBucketData data) {
            Credential cred = new Credential(data.UserName, "", data.UserName + "@" + "bitbucket/" + data.Team);
            try {
                bool exist = cred.Load();
                return exist;
            } finally {
                cred.Dispose();
            }
        }
        

        public static string CreateBitBucketRepo(BitBucketData data, NetworkCredential credentials) {

            {
                RestClient r = new RestClient("https://bitbucket.org/");
                r.Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password);
                RestRequest request = new RestRequest("api/2.0/repositories/" + data.Team + "/" + data.RepoSlug, Method.POST);
                request.AddParameter("name", data.RepoSlug);
                request.AddParameter("is_private", "true");
                request.AddParameter("scm", "git");
                string t = request.ToString();
                IRestResponse response = r.Post(request);
                if (response.StatusCode != HttpStatusCode.OK) {
                    return response.StatusDescription;
                }
            }
            return "Success";
        }
    }

}
