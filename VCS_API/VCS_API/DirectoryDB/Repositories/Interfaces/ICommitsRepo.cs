using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories.Interfaces
{
    public interface ICommitsRepo
    {
        public Task<CommitEntity?> CreateCommitAsync(CommitEntity? commitEntity);
        public Task<CommitEntity?> GetCommitAsync(string? repoName, string? branchName, string? commitHash, bool includeContent = true);
        public Task<CommitEntity?> GetLatestCommitAsync(string? repoName, string? branchName, bool includeContent = true);
        public Task<List<CommitEntity>?> GetAllCommitsContentless(string? repoName, string? branchName);
        public Task<CommitEntity?> GetOldestCommitAsync(string? repoName, string? branchName, bool includeContent = true);
    }
}
