namespace VCS_API.Models
{
    public class FileChangeEntity
    {
        public string? FilePath { get; set; }
        public string? OldContent { get; set; }
        public string? NewContent { get; set; }
    }
}
