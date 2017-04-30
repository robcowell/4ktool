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
using System.Windows.Forms;

namespace kampfpanzerin.git {
    class GitHandler {
        private Repository repo;

        public List<String> Conflicts {
            get {
                return new List<string>(repo.Index.Conflicts.Select(c => c.Ancestor.Path));
            }
        }

        public GitHandler(string folder) {
            repo = new Repository(folder.NormalizePath());
        }

        public static GitHandler Init(string folder, Project p, NetworkCredential credentials = null) {
            Logger.log("* Initialising local repo...");
            Cursor.Current = Cursors.WaitCursor;
            
            try {
                folder = folder.NormalizePath();
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
                Logger.log("Committed " + c.ToString() + "\r\n");
                GitHandler ret = new GitHandler(folder);
                if (p.gitRemote != null) {
                    if (credentials == null) {
                        credentials = BitBucketUtils.GetCredentials(p.bitBucketSettings);
                    }
                    gitRepo.Network.Remotes.Add("origin", p.gitRemote);
                    ret.Push(p, credentials);
                }
                Cursor.Current = Cursors.Default;      
                return ret;
            } catch (Exception) {
                Logger.log("! Bad juju guvnor, I couldn't create the repo!\r\n");
                Cursor.Current = Cursors.Default;
                return null;
            }
        }

        public void Push(Project p, NetworkCredential credentials) {
            Logger.log("* Pushing the repo to Bitbucket...");
            Cursor.Current = Cursors.WaitCursor;
            
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
                Logger.logf("Pushed {0} to {1}\r\n", repo.Head.Name, repo.Head.Remote.Name);
            } catch (NonFastForwardException) {
                Logger.logf("! Heavy Vibes Boss, someone edited the prod already, you need a pull here!\r\n");
            } catch (Exception) {
                Logger.logf("! Bugger, couldn't push the shizz!\r\n");
            }
            Cursor.Current = Cursors.Default;
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
            Logger.log("* Creating new Bitbucket repo...");
            Cursor.Current = Cursors.WaitCursor;
            try {
                RestClient r = new RestClient("https://bitbucket.org/");
                r.Authenticator = new HttpBasicAuthenticator(credentials.UserName, credentials.Password);
                RestRequest request = new RestRequest("api/2.0/repositories/" + data.Team + "/" + data.RepoSlug, Method.POST);
                request.AddParameter("name", data.RepoSlug);
                request.AddParameter("is_private", "true");
                request.AddParameter("scm", "git");
                string t = request.ToString();
                IRestResponse response = r.Post(request);
                if (response.StatusCode != HttpStatusCode.OK) {
                    Logger.logf("! Couldn't create the repo :(\r\n");
                    Cursor.Current = Cursors.Default;
                    return null;
                } else {
                    string s = string.Format("https://bitbucket.org/{0}/{1}.git", data.Team, data.RepoSlug);
                    Logger.logf("Created repo at "+s+"\r\n");
                    Cursor.Current = Cursors.Default;
                    return s;
                }
            } catch (Exception) {
                Logger.logf("! Couldn't create the repo :(\r\n");
                Cursor.Current = Cursors.Default;
                return null;
            }
        }

        public static IEnumerable<RepoDescriptor> GetBitBucketRepos(string team, NetworkCredential credentials) {
            try {
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
            } catch (Exception) {
                Logger.logf("! Couldn't enumerate Bitbucket repos :(\r\n");
                return null;
            }
        }

        public static string Clone(RepoDescriptor desc, string path, NetworkCredential credentials) {
            Logger.logf("* Cloning "+desc.Clone+"...");
            Cursor.Current = Cursors.WaitCursor;
            try {
                var options = new CloneOptions();
                options.CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler(
                    (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials() {
                            Username = credentials.UserName,
                            Password = credentials.Password
                        });
                var dir = Repository.Clone(desc.Clone, path, options);
                Logger.logf("Cloned\r\n");
                Cursor.Current = Cursors.Default;
                return dir;
            } catch (Exception) {
                Logger.logf("! Couldn't clone the repo :(\r\n");
                Cursor.Current = Cursors.Default;
                return null;
            }
        }

        internal void Pull(Project project, NetworkCredential credentials) {
            Logger.logf("* Pulling...");
            Cursor.Current = Cursors.WaitCursor;
            try {
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

                Configuration config = new Configuration();
                var gitUsername = config.Get<string>("user.name", ConfigurationLevel.Global).Value;

                Signature s = new Signature(gitUsername, gitUsername, DateTime.Now);
                MergeResult r = repo.Network.Pull(s, options);
                MergeResult(r);
            } catch (Exception) {
                Logger.logf("! Couldn't pull the repo :(\r\n");
            }

            Cursor.Current = Cursors.Default;
        }

        internal void Commit(string message = null) {
            Logger.logf("* Committing...");
            Cursor.Current = Cursors.WaitCursor;
            CommitOptions co = new CommitOptions();
            co.AllowEmptyCommit = false;
            foreach (StatusEntry e in repo.RetrieveStatus()) {
                if(e.State == FileStatus.Modified) {
                    repo.Stage(e.FilePath);
                }
            }
            try {
                if (message == null)
                {
                    repo.Commit(DateTime.Now.ToString(), co);
                }
                else
                {
                    repo.Commit(message);
                }
                Logger.logf("Committed\r\n");
            } catch (EmptyCommitException) {
                Logger.logf("! Nothing to commit\r\n");
            } catch (UnmergedIndexEntriesException) {
                Logger.logf("! Bad vibes boss, can't commit while not merged!\r\n");
            }
            Cursor.Current = Cursors.Default;
        }

        internal bool CheckCommitNeeded()
        {
            RepositoryStatus status = repo.RetrieveStatus();
            return status.IsDirty;
        } 

        internal void Branch(string name)
        {
            Logger.logf("* Committing...");
            Cursor.Current = Cursors.WaitCursor;

            Branch branch = repo.CreateBranch(name);
            CheckoutOptions options = new CheckoutOptions();
            repo.Checkout(branch, options);
        }

        internal void merge(string branchToMerge)
        {
            Branch b = repo.Branches[branchToMerge];
            Configuration config = new Configuration();
            var gitUsername = config.Get<string>("user.name", ConfigurationLevel.Global).Value;

            Signature s = new Signature(gitUsername, gitUsername, DateTime.Now);

            MergeResult r = repo.Merge(b, s);
            MergeResult(r);
        }

        internal void MergeResult(MergeResult r)
        {
            switch (r.Status)
            {
                case MergeStatus.UpToDate:
                    Logger.logf("Up to date with {0}\r\n", repo.Head.Remote.Name);
                    break;
                case MergeStatus.FastForward:
                    Logger.logf("Merged all changes from {0} (fast forward)\r\n", repo.Head.Remote.Name);
                    break;
                case MergeStatus.NonFastForward:
                    Logger.logf("Merged all changes from {0} (merged, non fast forward)\r\n", repo.Head.Remote.Name);
                    break;
                case MergeStatus.Conflicts:
                    Logger.logf("! Conflict while pulling from {0}\r\n", repo.Head.Remote.Name);
                    foreach (Conflict c in repo.Index.Conflicts)
                    {
                        Logger.logf(" * {0}", c.Ancestor.Path);
                    }

                    Logger.logf("! Eeep, please resolve and commit\r\n", repo.Head.Remote.Name);
                    break;
            }
        }

        internal void Resolve(List<string> list) {
            list.ForEach(repo.Stage);
        }

        internal static void SetUsername(string name) {
            Configuration c = new Configuration();
            c.Set<string>("user.name", name, ConfigurationLevel.Global);
            c.Dispose();
        }
    }
}
