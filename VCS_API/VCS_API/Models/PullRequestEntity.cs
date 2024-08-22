namespace VCS_API.Models
{
    public class PullRequestEntity
    {
        public string? PullRequestId { get; set; }
        public string? RepoName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? BranchName { get; set; }
        public string? TargetBranchName { get; set; }
        public string? CreationTime { get; set; }
        public string? LastUpdated { get; set; }
        public bool IsMerged { get; set; } = false;
        public string? MergeTimestamp { get; set; }
        public List<string>? CommitHistory { get; set; }
        public string? Status { get; set; } // e.g., "Open", "Closed", "Merged"
        public List<string>? Reviewers { get; set; }
        public string? ReviewStatus { get; set; } // e.g., "Pending", "Approved", "Changes Requested"
        public List<string>? Labels { get; set; }
        public List<string>? Comments { get; set; }
        public string? SerializedDiff { get; set; }
    }

    /*
     // Method to merge the pull request
    public void Merge()
    {
        IsMerged = true;
        MergeTimestamp = DateTime.Now;
        Status = "Merged";
        LastUpdated = DateTime.Now;
    }
    
    // Method to close the pull request without merging
    public void Close()
    {
        Status = "Closed";
        LastUpdated = DateTime.Now;
    }
     */
}
