namespace VCS_API.Models
{
    public class BranchEntity
    {
        public string? RepoName;
        public string? ParentBranchName { get; set; } = Constants.Constants.MasterBranchName;
        public string? Name { get; set; }
        public string? CreationTime { get; set; }
        public List<CommitEntity>? Commits { get; set; }
    }
}
