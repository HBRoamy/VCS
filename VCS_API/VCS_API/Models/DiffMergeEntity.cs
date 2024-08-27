using DiffPlex.DiffBuilder.Model;

namespace VCS_API.Models
{
    public class DiffMergeEntity
    {
        public bool? Mergeable { get; set; }
        public List<DiffPiece>? OldChanges { get; set; }
        public List<DiffPiece>? NewChanges { get; set; }
    }
}
