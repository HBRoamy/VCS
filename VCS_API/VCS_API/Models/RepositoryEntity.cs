namespace VCS_API.Models
{
    public class RepositoryEntity
    {
        public string? Id { get; set; }// in future it will be org+repoName 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreationTime { get; set; }
        public bool IsPrivate { get; set; } = false;
        public List<BranchEntity>? Branches { get; set; }
    }
}
