namespace VCS_API.Models.ResponseModels
{
    public class CommitViewResponse : DiffComparisonEntity
    {
        public string? Timestamp { get; set; }
        public string? Message { get; set; }
    }
}
