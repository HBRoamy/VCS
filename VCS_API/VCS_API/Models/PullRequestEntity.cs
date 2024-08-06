namespace VCS_API.Models
{
    public class PullRequestEntity
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? SourceBranch { get; set; }
        public string? TargetBranch { get; set; }
        public bool IsMerged { get; set; }
    }
}
