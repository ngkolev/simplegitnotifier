using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleGitNotifier
{
    class GitFetcher
    {
        public GitFetcherResult Fetch()
        {
            var config = new Configuration();
            var result = new GitFetcherResult();
            var processInfo = new ProcessStartInfo();
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = config.RepositoryPath;
            processInfo.FileName = config.GitFullName;
            processInfo.Arguments = "fetch";

            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            string error;
            using (var process = Process.Start(processInfo))
            {
                process.WaitForExit();
                error = process.StandardError.ReadToEnd();
            }

            if (!String.IsNullOrEmpty(error))
            {
                result.AnyChanges = true;
                result.RelevantBranches = GetRelevantBranches(error);
            }

            return result;
        }

        private string[] GetRelevantBranches(string text)
        {
            var matches = Regex.Matches(text, "-> .*/(?<branch>.*)");

            return matches.Cast<Match>().Select(m => m.Groups["branch"].Value).Distinct().ToArray();
        }
    }
}
