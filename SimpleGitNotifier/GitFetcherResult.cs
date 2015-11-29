namespace SimpleGitNotifier
{
    public class GitFetcherResult
    {
        public bool AnyChanges { get; set; }
        public string[] RelevantBranches { get; set; }
    }
}