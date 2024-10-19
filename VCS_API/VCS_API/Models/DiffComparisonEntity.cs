using DiffPlex.DiffBuilder.Model;

namespace VCS_API.Models
{
    public class DiffComparisonEntity
    {
        public string? RepoName { get; set; }
        public string? BranchName { get; set; }
        public string? BranchCommitHash { get; set; }
        public string? BaseBranchName { get; set; }
        public string? BaseBranchCommitHash { get; set; }
        public bool? IsMergeable { get; set; }
        public List<DiffPiece>? OldChanges { get; set; }
        public List<DiffPiece>? NewChanges { get; set; }
    }
}
