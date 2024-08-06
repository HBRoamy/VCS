namespace VCS_API.Models.ResponseModels
{
    public class RepositoryResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreationTime { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}
