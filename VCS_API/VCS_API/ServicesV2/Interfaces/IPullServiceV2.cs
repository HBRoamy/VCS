using DiffPlex.DiffBuilder.Model;
using VCS_API.Models;

namespace VCS_API.ServicesV2.Interfaces
{
    public interface IPullServiceV2
    {
        public Task<DiffMergeEntity?> GetSideBySideComparisonForCommit(string? repoName, string? branchName, string? parentBranchName);
    }
}
