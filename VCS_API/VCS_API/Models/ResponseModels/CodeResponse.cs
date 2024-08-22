namespace VCS_API.Models.ResponseModels
{
    public class CodeResponse
    {
        public string? RepoName { get; set; }
        public string? BranchName { get; set; }
        public string? Hash { get; set; }
        public string? Message { get; set; }
        public string? Timestamp { get; set; }
        public string? Content { get; set; }
    }
}
