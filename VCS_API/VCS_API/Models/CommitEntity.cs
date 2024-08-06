namespace VCS_API.Models
{
    public class CommitEntity
    {
        public string? BaseCommitAddress { get; set; }//the commit hash of the commit on top of which this commit did the changes
        public string? RepoName { get; set; }
        public string? BranchName { get; set; }
        public string? Hash { get; set; }
        public string? Message { get; set; }
        public string? Timestamp { get; set; }
        public string? ChangeStorageAddress { get; set; } //stores the address of the file which contains the full changes introduced in the commit. Format datawarehouse\RepoName#BranchName\changes\CommitHash
        public string? Content { get; set; }
    }
}
