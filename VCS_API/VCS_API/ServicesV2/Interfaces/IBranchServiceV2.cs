using VCS_API.Models;
using static VCS_API.ServicesV2.BranchServiceV2;

namespace VCS_API.ServicesV2.Interfaces
{
    public interface IBranchServiceV2
    {
        public Task<BranchEntity?> CreateBranchAsync(BranchEntity? newBranch);
        public Task<List<BranchEntity>?> GetAllBranchesInRepoAsync(string? repoName);
        public Task<BranchEntity?> GetBranchAsync(string? branchName, string? repoName, string? commitHash = null);
        public Task<BranchEntity?> DeleteBranchAsync(string? repoName, string? branchName);
        public Task DeleteAllBranchesInRepoAsync(string? repoName);
        public Task<RawNodeDatum?> GetBranchTreeForRepoAsync(string? repoName);
    }
}
