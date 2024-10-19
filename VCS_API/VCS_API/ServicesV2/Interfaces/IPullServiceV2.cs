using DiffPlex.DiffBuilder.Model;
using VCS_API.Models;
using VCS_API.Models.ResponseModels;

namespace VCS_API.ServicesV2.Interfaces
{
    public interface IPullServiceV2
    {
        public Task<DiffComparisonEntity?> GetSideBySideComparisonForCommit(string? repoName, string? branchName, string? parentBranchName);
        public Task<CommitViewResponse?> GetCommitedDiffWithParentCommit(string repoName, string branchName, string commitHash);
    }
}
