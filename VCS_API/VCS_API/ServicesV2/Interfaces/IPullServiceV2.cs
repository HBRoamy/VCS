using DiffPlex.DiffBuilder.Model;

namespace VCS_API.ServicesV2.Interfaces
{
    public interface IPullServiceV2
    {
        public Task<List<(DiffPiece, DiffPiece)>?> GetSideBySideComparisonForCommit(string? repoName, string? branchName, string? parentBranchName);
    }
}
