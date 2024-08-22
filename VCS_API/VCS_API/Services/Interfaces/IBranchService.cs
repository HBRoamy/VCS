using VCS_API.Models;
using VCS_API.Models.RequestModels;
using VCS_API.Models.ResponseModels;

namespace VCS_API.Services.Interfaces
{
    public interface IBranchService
    {
        public Task<List<BranchEntity>>? GetBranchesByRepositoryNameAsync(string? repoName);
        public Task<string?> CreateBranchAsync(BranchEntity newBranch);
        public Task<bool> IsBranchPresentInRepoAsync(string branch, string repo);
        public Task<CodeResponse> GetContentfulBranch(CodeRequest request);
        public Task<int> DeleteAllBranchesIn(string? repoName);
    }
}
