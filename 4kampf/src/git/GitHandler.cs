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

        public List<String> Conflicts {
            get {
                return new List<string>(repo.Index.Conflicts.Select(c => c.Ancestor.Path));
            }
        }

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
            Logger.log("* Committed " + c.ToString() + "\r\n\r\n");
            GitHandler ret = new GitHandler(folder);
            if (p.gitRemote != null) {
                if (credentials == null) {
                    credentials = BitBucketUtils.GetCredentials(p.bitBucketSettings);
                }
                gitRepo.Network.Remotes.Add("origin", p.gitRemote);
                ret.Push(p, credentials);
            }
            return ret;
        }

        public void Push(Project p, NetworkCredential credentials) {
            LibGit2Sharp.PushOptions options = new LibGit2Sharp.PushOptions();
            options.CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler(
                (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials() {
                        Username = credentials.UserName,
                        Password = credentials.Password
                    });

            try {
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
                Logger.logf("pushed {0} to {1}", repo.Head.Name, repo.Head.Remote.Name);

            } catch (NonFastForwardException) {
                Logger.logf("Heavy Vibes Boss, someone edited the prod already, you need a pull here!");
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
                return string.Format("https://bitbucket.org/{0}/{1}.git", data.Team, data.RepoSlug);
            }

        }

        public static IEnumerable<RepoDescriptor> GetBitBucketRepos(string team, NetworkCredential credentials) {
            RestClient r = new RestClient("https://bitbucket.org/");
            r.Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password);
            RestRequest request = new RestRequest("api/2.0/repositories/" + team, Method.GET);
            string t = request.ToString();
            IRestResponse response = r.Get(request);
            IDictionary<string, object> o = (IDictionary<string, object>)SimpleJson.DeserializeObject(response.Content);
            if (response.StatusCode != HttpStatusCode.OK) {
                return null;
            } else {
                IEnumerable<RepoDescriptor> ret = ((RestSharp.JsonArray)o["values"]).Select(e0 => {
                    var e = (JsonObject)e0;
                    var rD = new RepoDescriptor();
                    rD.Name = e["name"].ToString();
                    var links = ((IDictionary<string, object>)e["links"]);
                    var self = ((JsonObject)links["self"]);
                    rD.Slug = self["href"].ToString().GetParentUriString();
                    rD.Clone = ((JsonObject)((JsonArray)links["clone"]).Find(
                        cu => {
                            return (((JsonObject)cu)["name"]).Equals("https");
                        }))["href"].ToString();
                    rD.Description = e["description"].ToString();
                    return rD;
                });
                return ret;
            }

        }

        public static string Clone(RepoDescriptor desc, string path, NetworkCredential credentials) {
            var options = new CloneOptions();
            options.CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler(
                (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials() {
                        Username = credentials.UserName,
                        Password = credentials.Password
                    });
            var dir = Repository.Clone(desc.Clone, path, options);
            return dir;
        }
        

        internal void Pull(Project project, NetworkCredential credentials) {
            LibGit2Sharp.PullOptions options = new LibGit2Sharp.PullOptions();
            options.MergeOptions = new MergeOptions();
            options.MergeOptions.CommitOnSuccess = true;
            options.MergeOptions.FileConflictStrategy = CheckoutFileConflictStrategy.Merge;
            options.FetchOptions = new FetchOptions();
            options.FetchOptions.CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler(
                (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials() {
                        Username = credentials.UserName,
                        Password = credentials.Password
                    });
            Signature s = new Signature(project.bitBucketSettings.UserName, project.bitBucketSettings.UserName, DateTime.Now);
            MergeResult r = repo.Network.Pull(s, options);
            switch (r.Status) {
                case MergeStatus.UpToDate:
                    Logger.logf("up to date with {0}", repo.Head.Remote.Name);
                    break;
                case MergeStatus.FastForward:
                    Logger.logf("pulled all changes from {0} (fast forward)", repo.Head.Remote.Name);
                    break;
                case MergeStatus.NonFastForward:
                    Logger.logf("pulled all changes from {0} (merged, non fast forward)", repo.Head.Remote.Name);
                    break;
                case MergeStatus.Conflicts:
                    Logger.logf("Conflict while pulling from {0}", repo.Head.Remote.Name);
                    foreach (Conflict c in repo.Index.Conflicts) {
                        Logger.logf(" * {0}", c.Ancestor.Path);
                    }
                    
                    Logger.logf("Please resolve and commit", repo.Head.Remote.Name);
                    break;
            }
        }

        internal void Commit() {
            CommitOptions co = new CommitOptions();
            co.AllowEmptyCommit = false;
            foreach (StatusEntry e in repo.RetrieveStatus()) {
                if(e.State == FileStatus.Modified) {
                    repo.Stage(e.FilePath);
                }
            }
            try {
                repo.Commit(DateTime.Now.ToString(), co);
                Logger.logf("committed");
            } catch (EmptyCommitException) {
                Logger.logf("Nothing to commit");
            } catch (UnmergedIndexEntriesException) {
                Logger.logf("Bad vibes boss, can't commit while not merged!");
            }
        }

        internal void Resolve(List<string> list) {
            list.ForEach(repo.Stage);
        }
    }

}
