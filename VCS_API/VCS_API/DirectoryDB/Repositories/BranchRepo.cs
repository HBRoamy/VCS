using System.Text.RegularExpressions;
using VCS_API.DirectoryDB.Helpers;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories
{
    public class BranchRepo
    {
        //add a GetBranchNamesByRepoName Async should go in service and will be built on top of GetBranchesByRepoName here which returns a list of branch objects
        //Move the logging to the services

        public async Task<BranchEntity?> CreateBranchAsync(BranchEntity newBranch)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(newBranch?.Name);

#pragma warning disable CS8604 // Null name is already getting validated above which is all that matters
                var branchEntryRow = DBHelper.AppendDelimited(newBranch?.Name, newBranch?.RepoName, newBranch?.ParentBranchName, newBranch?.CreationTime);
#pragma warning restore CS8604
                await DirectoryDB.WriteToFileAsync(DBPaths.BranchStorePath(newBranch?.RepoName), branchEntryRow);
                await DirectoryDB.WriteToFileAsync(DBPaths.ChangesetsHeadPath(newBranch?.RepoName, newBranch?.Name), null);
                await DirectoryDB.WriteToFileAsync(DBPaths.CommitsStorePath(newBranch?.RepoName, newBranch?.Name), null);
                AuditLogsRepo.Log(newBranch?.RepoName, $"Created the branch \'{newBranch?.Name}\' (based on \'{newBranch?.ParentBranchName}\') in the repository \'{newBranch?.RepoName}\' at {newBranch?.CreationTime}.");

                return DeserializeRowEntry(branchEntryRow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(CreateBranchAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<BranchEntity?> GetBranchByName(string? branchName, string? repoName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(branchName, repoName);

                var searchTerm = branchName + Constants.Constants.StandardColumnDelimiter; // this helps us eliminate the case when there are repos present with common prefix
                var branchEntryRow = await DirectoryDB.FirstOrDefaultRowAsync(DBPaths.BranchStorePath(repoName), x => x.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(branchEntryRow))
                {
                    return DeserializeRowEntry(branchEntryRow);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetBranchByName)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<List<BranchEntity>?> GetBranchesByRepoName(string repoName)
        {
            ///string pattern = $@"^.*?, {Regex.Escape(repoName)}, .*?, .*?$"; //item should be in the second column only
            ///bool isMatch1 = Regex.IsMatch(input1, pattern);

            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

                var matchingBranchEntryRows = await DirectoryDB.GetAllRowsAsync(DBPaths.BranchStorePath(repoName));
                if (matchingBranchEntryRows != null && matchingBranchEntryRows.Length != 0)
                {
                    return matchingBranchEntryRows.Select(x => DeserializeRowEntry(x)!).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetBranchesByRepoName)}\' " + ex.Message);
            }

            return null;
        }

        public async Task DeleteBranchByName(string? branchName, string repoName)
        {
            // Service shouldn't allow the deletion of the master branch
            // after development, create a way to ONLY MARK THE BRANCH DELETED and dont delete it, it will be used to restore it later.

            try
            {
                Validations.ThrowIfNullOrWhiteSpace(branchName, repoName);

                var searchTerm = branchName + Constants.Constants.StandardColumnDelimiter; // this helps us eliminate the case when there are repos present with common prefix.
                var deletedRow = await DirectoryDB.DeleteRowAsync(DBPaths.BranchStorePath(repoName), x => x.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
                var deletedObject = DeserializeRowEntry(deletedRow);
                AuditLogsRepo.Log(repoName, $"Deleted the branch \'{branchName}\' (based on \'{deletedObject?.ParentBranchName}\') in the repository \'{repoName}\' at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(DeleteBranchByName)}\' " + ex.Message);
            }
        }

        public async Task DeleteAllBranchesByRepoName(string repoName)
        {
            // Service shouldn't allow the deletion of the master branch
            // after development, create a way to ONLY MARK THE BRANCH DELETED and dont delete it, it will be used to restore it later.

            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

                await DirectoryDB.RemoveAll(DBPaths.BranchStorePath(repoName));
                AuditLogsRepo.Log(repoName, $"Deleted all the branches in the repository \'{repoName}\' at {DateTime.Now}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(DeleteBranchByName)}\' " + ex.Message);
            }
        }

        private static BranchEntity? DeserializeRowEntry(string? csvRowEntry)
        {
            if (string.IsNullOrWhiteSpace(csvRowEntry))
            {
                return null;
            }

            var columns = csvRowEntry.GetColumns();
            return new BranchEntity
            {
                Name = columns[0],
                RepoName = columns[1],
                ParentBranchName = columns[2],
                CreationTime = columns[3]
            };
        }
    }
}
