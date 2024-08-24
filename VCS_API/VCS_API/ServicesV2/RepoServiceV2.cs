using VCS_API.Models;

namespace VCS_API.ServicesV2
{
    public class RepoServiceV2
    {
        public Task<string?> CreateRepo(RepositoryEntity repoEntity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRepoAsync(string repoName)
        {
            throw new NotImplementedException();
        }

        public Task<List<RepositoryEntity>> GetAllRepos()
        {
            throw new NotImplementedException();
        }

        public Task<RepositoryEntity?> GetRepoByNameAsync(string repoName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsRepoPresent(string repo)
        {
            throw new NotImplementedException();
        }
    }
}
