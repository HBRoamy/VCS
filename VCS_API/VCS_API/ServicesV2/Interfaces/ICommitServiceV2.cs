using VCS_API.Models;

namespace VCS_API.ServicesV2.Interfaces
{
    public interface ICommitServiceV2
    {
        public Task<CommitEntity?> CommitChanges(CommitEntity? commitEntity);
        public Task<CommitEntity?> GetLatestCommitAsync(string? repoName, string? branchName, bool includeContent = true);
        public Task<CommitEntity?> GetCommitAsync(string? repoName, string? branchName, string? commitHash);
    }
}
