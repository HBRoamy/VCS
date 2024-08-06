using VCS_API.Models;

namespace VCS_API.Services.Interfaces
{
    public interface IBranchService
    {
        public Task<List<BranchEntity>>? GetBranchesByRepositoryNameAsync(string? repoName);
        public Task<string?> CreateBranchAsync(BranchEntity newBranch);
        public Task<bool> IsBranchPresentInRepo(string branch, string repo);
        public Task<int> DeleteAllBranchesIn(string? repoName);
    }
}
