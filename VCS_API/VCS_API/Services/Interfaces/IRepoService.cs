using VCS_API.Models;

namespace VCS_API.Services.Interfaces
{
    public interface IRepoService
    {
        public Task<List<RepositoryEntity>> GetAllRepos();
        public Task<string?> CreateRepo(RepositoryEntity repoEntity);
        public Task<RepositoryEntity?> GetRepoByNameAsync(string repoName);
        public Task<bool> IsRepoPresent(string repo);
        public Task<int> DeleteRepoAsync(string repoName);
    }
}
