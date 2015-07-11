using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibGit2Sharp;
using kampfpanzerin.log;
using kampfpanzerin.core.Serialization;
using System.Net;
using RestSharp;
using kampfpanzerin.utils;

namespace kampfpanzerin.git {
    class GitHandler {

        private Repository repo;

        public GitHandler(string folder) {
            repo = new Repository(folder);
        }

        public static GitHandler Init(string folder, Project p, NetworkCredential credentials = null) {


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
            Logger.log("* Committed " + c.ToString() + "\n\n");
            if (p.gitRemote != null) {
                if (credentials == null) {
                    credentials = BitBucketUtils.GetCredentials(p.bitBucketSettings);
                }
                gitRepo.Network.Remotes.Add("origin", p.gitRemote);
                Push(p, gitRepo, credentials);
            }
            return new GitHandler(folder);
        }

        public static void Push(Project p, Repository repo, NetworkCredential credentials) {
            LibGit2Sharp.PushOptions options = new LibGit2Sharp.PushOptions();
            options.CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler(
                (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials() {
                        Username = credentials.UserName,
                        Password = credentials.Password
                    });
            if (!repo.Head.IsTracking) {
                string refspec = string.Format("{0}:{1}",
                         repo.Head.CanonicalName, repo.Head.CanonicalName);
                repo.Network.Push(repo.Network.Remotes["origin"], refspec, options);
                repo.Branches.Update(repo.Head, delegate(BranchUpdater updater) {
                    updater.Remote = repo.Network.Remotes["origin"].Name;
                    updater.UpstreamBranch = repo.Head.CanonicalName;
                });
            } else {
                repo.Network.Push(repo.Head, options);
            }
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

            return relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar);
        }

        public static string CreateBitBucketRepo(BitBucketData data, NetworkCredential credentials) {
            RestClient r = new RestClient("https://bitbucket.org/");
            r.Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password);
            RestRequest request = new RestRequest("api/2.0/repositories/" + data.Team + "/" + data.RepoSlug, Method.POST);
            request.AddParameter("name", data.RepoSlug);
            request.AddParameter("is_private", "true");
            request.AddParameter("scm", "git");
            string t = request.ToString();
            IRestResponse response = r.Post(request);
            if (response.StatusCode != HttpStatusCode.OK) {
                return null;
            } else {
                return string.Format("https://{0}@bitbucket.org/{1}/{2}.git", data.UserName, data.Team, data.RepoSlug);
            }

        }
    }

}
