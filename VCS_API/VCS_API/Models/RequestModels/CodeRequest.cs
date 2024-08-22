namespace VCS_API.Models.RequestModels
{
    public class CodeRequest
    {
        public string? RepoName { get; set; }
        public string? BranchName { get; set; }
        public string? CommitHash { get; set; } // if null or empty, will return head commit
    }
}
