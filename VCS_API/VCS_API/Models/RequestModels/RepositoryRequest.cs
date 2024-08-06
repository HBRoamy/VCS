namespace VCS_API.Models.RequestModels
{
    public class RepositoryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}
