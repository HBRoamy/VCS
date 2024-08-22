namespace VCS_API.Models.RequestModels
{
    public class PullRequestRequest
    {
        public string? RepoName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? BranchName { get; set; }
        public List<string>? Labels { get; set; }
    }
}
