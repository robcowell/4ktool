using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibGit2Sharp;
using kampfpanzerin.log;

namespace kampfpanzerin.git {
    class GitHandler {

        private Repository repo;

        public GitHandler(string folder) {
            repo = new Repository(folder);
        }

        public static GitHandler Init(string folder) {

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
            Signature s = new Signature(Properties.Settings.Default.gitAuthor, Properties.Settings.Default.gitEmail, DateTime.Now);
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
    }

}
