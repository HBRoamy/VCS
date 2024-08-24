using VCS_API.Models;

namespace VCS_API.ServicesV2.Interfaces
{
    public interface IBranchServiceV2
    {
        public Task<BranchEntity?> CreateBranchAsync(BranchEntity? newBranch);
        public Task<List<BranchEntity>?>? GetAllBranchesInRepoAsync(string? repoName);
        public Task<BranchEntity?> GetBranchAsync(string? branchName, string? repoName);
        public Task<BranchEntity?> DeleteBranchAsync(string? repoName, string? branchName);
        public Task DeleteAllBranchesInRepoAsync(string? repoName);
        public Task GetBranchTreeForRepoAsync(string? repoName);
    }
}
