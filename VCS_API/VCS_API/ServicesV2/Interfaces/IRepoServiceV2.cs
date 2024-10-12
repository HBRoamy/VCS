using VCS_API.Models;

namespace VCS_API.ServicesV2.Interfaces
{
    public interface IRepoServiceV2
    {
        public Task<RepositoryEntity?> CreateRepo(RepositoryEntity? repoEntity);
        public Task<List<RepositoryEntity>?> GetAllRepos();
        public Task<RepositoryEntity?> GetRepoAsync(string? repoName);
        public Task<List<HistoryFragment>?> GetRepoHistoryAsync(string repoName);
        public Task<string> UpdateRepoReadMe(string? repoName, string content);
        public Task DeleteRepoAsync(string? repoName);
    }
}
