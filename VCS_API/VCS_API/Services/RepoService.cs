using VCS_API.Extensions;
using VCS_API.Models;
using VCS_API.Repositories.Interfaces;
using VCS_API.Services.Interfaces;

namespace VCS_API.Services
{
    public class RepoService(IRepository<RepositoryEntity> repoRepository) : IRepoService
    {
		private static readonly HashSet<char> prohibitedCharacters = ['-','#', '@', '$', '%', '^', '&', '*', '(', ')', '!', '~', '\"', '/', '\\', '|', '`', '=', '+', '?', ',', '>', '<', '[', ']','{', '}', ';', ':', '\'', ' ', '\n', '\t'];
        
        public async Task<List<RepositoryEntity>> GetAllRepos()
        {
            var rows = await repoRepository.AllRows();
            List<RepositoryEntity> repositoryEntities = [];

            if (rows==null || rows.Length==0) return repositoryEntities;
            
            foreach (var row in rows)
            {
                var columns = row.GetColumns();

                repositoryEntities.Add(new RepositoryEntity
                {
                    Name = columns[0],
                    CreationTime = columns[1],
                    Description = columns[2],
                    IsPrivate = bool.Parse(columns[3])
                });
            }

            return repositoryEntities;
        }

        public async Task<string?> CreateRepo(RepositoryEntity repoEntity)
        {
			try
			{
                if (repoEntity.Name.IsValidName(prohibitedCharacters, 100, 5) && await GetRepoByNameAsync(repoEntity.Name!)==null)
                {
                    var repoDataStorePath = await repoRepository.Create($"{repoEntity.Name}{Constants.Constants.StandardColumnDelimiter}{repoEntity.CreationTime}{Constants.Constants.StandardColumnDelimiter}{repoEntity.Description}{Constants.Constants.StandardColumnDelimiter}{repoEntity.IsPrivate}");
                    return repoDataStorePath;
                }
                else 
                {
                    throw new Exception("Item name is either invalid or is already taken.");
                }
			}
			catch (Exception)
			{
				throw;
			}
        }

        public async Task<RepositoryEntity?> GetRepoByNameAsync(string repoName)
        {
            var searchResult = await repoRepository.FindAsync(row => row.StartsWith(repoName+Constants.Constants.StandardColumnDelimiter));

            if(!string.IsNullOrWhiteSpace(searchResult))
            {
                var columns = searchResult.GetColumns();
                return new RepositoryEntity
                {
                    Name= columns[0],
                    CreationTime = columns[1],
                    Description = columns[2],
                    IsPrivate = bool.Parse(columns[3])
                };
            }

            return null;
        }

        public async Task<bool> IsRepoPresent(string repo)
        {
            if (string.IsNullOrWhiteSpace(repo)) return false;

            return !string.IsNullOrWhiteSpace(await repoRepository.FindAsync(row => row.StartsWith(repo + Constants.Constants.StandardColumnDelimiter)));
        }

        public async Task<int> DeleteRepoAsync(string repoName)
        {
            var searchResult = await repoRepository.FindAsync(row => row.StartsWith(repoName + Constants.Constants.StandardColumnDelimiter));

            if (string.IsNullOrWhiteSpace(searchResult))
            {
                throw new ArgumentException("{repoName} doesn't exist.", repoName);
            }

            var totalDeletedRepos = repoRepository.Delete(row => row.StartsWith(repoName + Constants.Constants.StandardColumnDelimiter));

            if (totalDeletedRepos > 1) throw new Exception("More than one repos match the name.");

            return totalDeletedRepos;
        }

        
    }
}
