using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories.Interfaces
{
    public interface IBranchRepo
    {
        public Task<BranchEntity?> CreateBranchAsync(BranchEntity? newBranch);
        public Task<BranchEntity?> GetBranchByNameAsync(string? branchName, string? repoName);
        public Task<List<BranchEntity>?> GetBranchesByRepoNameAsync(string? repoName);
        public Task<BranchEntity?> DeleteBranchByNameAsync(string? branchName, string? repoName);
        public Task DeleteAllBranchesByRepoNameAsync(string? repoName);
    }
}
