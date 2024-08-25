using VCS_API.DirectoryDB.Helpers;
using VCS_API.DirectoryDB.Repositories.Interfaces;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories
{
    public class RepositoryRepo : IRepositoryRepo
    {
        public async Task<RepositoryEntity?> CreateRepository(RepositoryEntity? repositoryEntity)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repositoryEntity?.Name); //since it will be a key used for searching, we can't allow it to be empty.

                var creationTime = DateTime.Now.ToString();
                var repoEntryRow = DBHelper.AppendDelimited(repositoryEntity?.Name, repositoryEntity?.Description, repositoryEntity?.IsPrivate.ToString(), creationTime);

                await DirectoryDB.WriteToFileAsync(DBPaths.RepoStorePath(), repoEntryRow, canCreateDirectory: true);
                AuditLogsRepo.Log(repositoryEntity?.Name, $"Created the repository \'{repositoryEntity?.Name}\' at {creationTime}.");

                return DeserializeRowEntry(repoEntryRow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(CreateRepository)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<RepositoryEntity?> GetRepoByNameAsync(string? repoName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

                var searchTerm = repoName + Constants.Constants.StandardColumnDelimiter; // this helps us eliminate the case when there are repos present with common prefix
                var repoEntryRow = await DirectoryDB.FirstOrDefaultRowAsync(DBPaths.RepoStorePath(), x => x.StartsWith(searchTerm));

                if(!string.IsNullOrWhiteSpace(repoEntryRow))
                {
                    return DeserializeRowEntry(repoEntryRow);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetRepoByNameAsync)}\' "+ex.Message);
            }

            return null;
        }

        public async Task<List<RepositoryEntity>?> GetAllReposAsync()
        {
            try
            {
                var matchingRepoEntryRows = await DirectoryDB.GetAllRowsAsync(DBPaths.RepoStorePath());

                if (matchingRepoEntryRows != null && matchingRepoEntryRows.Length != 0)
                {
                    return matchingRepoEntryRows.Select(row => DeserializeRowEntry(row)!).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllReposAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task DeleteRepoAsync(string? repoName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

                var searchTerm = repoName + Constants.Constants.StandardColumnDelimiter; // this helps us eliminate the case when there are repos present with common prefix.
                await DirectoryDB.DeleteRowAsync(DBPaths.RepoStorePath(), x => x.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
                AuditLogsRepo.Log(repoName, $"Deleted the repository \'{repoName}\' at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{DeleteRepoAsync}\' " + ex.Message);
            }
        }

        private static RepositoryEntity? DeserializeRowEntry(string? csvRowEntry)
        {
            if(string.IsNullOrWhiteSpace(csvRowEntry))
            {
                return null;
            }

            var columns = csvRowEntry.GetColumns();
            return new RepositoryEntity
            {
                Name = columns[0],
                Description = columns[1],
                IsPrivate = bool.Parse(columns[2]),
                CreationTime = columns[3]
            };
        }
    }
}
