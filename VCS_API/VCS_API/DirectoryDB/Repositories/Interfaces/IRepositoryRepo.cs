using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories.Interfaces
{
    public interface IRepositoryRepo
    {
        public Task<RepositoryEntity?> CreateRepository(RepositoryEntity? repositoryEntity);
        public Task<RepositoryEntity?> GetRepoByNameAsync(string? repoName);
        public Task<List<RepositoryEntity>?> GetAllReposAsync();
        public Task DeleteRepoAsync(string? repoName);
    }
}
