using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories.Interfaces
{
    public interface IPullsRepo
    {
        public Task<PullRequestEntity?> CreatePullRequest(PullRequestEntity? pullRequestEntity);
        public Task<List<PullRequestEntity>?> GetAllPullsAsync(string? repoName, PullStatus? pullStatus = null);
        public Task<PullRequestEntity?> ChangePullStatus(string? repoName, int pullSerialId, PullStatus pullStatus);
        public Task<PullRequestEntity?> GetPullBySerialIdAsync(int serialId, string? repoName);
        public Task<PullRequestEntity?> GetPullByBranchName(string? branchName, string? repoName);
    }
}
