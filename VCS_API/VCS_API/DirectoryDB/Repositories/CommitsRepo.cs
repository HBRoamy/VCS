using VCS_API.DirectoryDB.Helpers;
using VCS_API.DirectoryDB.Repositories.Interfaces;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories
{
    public class CommitsRepo : ICommitsRepo
    {
        public async Task<CommitEntity?> CreateCommitAsync(CommitEntity? commitEntity)
        {
			try
			{
				//base commit address is branchname#commihash
				Validations.ThrowIfNullOrWhiteSpace(commitEntity?.BranchName, commitEntity?.RepoName, commitEntity?.Message);

                var creationTime = DateTime.Now.ToString();
                var commitHash = Guid.NewGuid().ToString().Replace("-", string.Empty);

                var commitRowEntry = DBHelper.AppendDelimited(
                    commitHash,
                    commitEntity?.Message,
                    commitEntity?.BaseCommitAddress,
                    creationTime
                    );

				await DirectoryDB.WriteToFileAsync(DBPaths.CommitsStorePath(commitEntity?.RepoName!, commitEntity?.BranchName!), commitRowEntry, canCreateDirectory: true);
				await DirectoryDB.WriteToFileAsync(DBPaths.ChangesetsHeadPath(commitEntity?.RepoName!, commitEntity?.BranchName!), commitRowEntry, append: false, canCreateDirectory: true); //overwrite the last commit info
				await DirectoryDB.WriteToFileAsync(DBPaths.CommitLOBPath(commitEntity?.RepoName!, commitEntity?.BranchName!, commitHash), commitEntity?.Content, canCreateDirectory: true);
                AuditLogsRepo.Log(commitEntity?.RepoName, $"Created a commit \'{commitHash}\' (based on \'{commitEntity?.BaseCommitAddress}\') in the branch \'{commitEntity?.BranchName}\' in the repository \'{commitEntity?.RepoName}\'.");

                return DeserializeRowEntry(commitRowEntry);
            }
            catch (Exception ex)
			{
                Console.WriteLine($"An error occured in the method \'{nameof(CreateCommitAsync)}\' " + ex.Message);
            }

			return null;
        }

        public async Task<CommitEntity?> GetCommitAsync(string? repoName, string? branchName, string? commitHash, bool includeContent = true)
        {
            try
            {
                //base commit address is branchname#commihash
                Validations.ThrowIfNullOrWhiteSpace(commitHash, branchName, repoName);
                var searchTerm = commitHash + Constants.Constants.StandardColumnDelimiter; // this helps us eliminate the case when there are terms present with common prefix
                var commitEntryRow = await DirectoryDB.FirstOrDefaultRowAsync(DBPaths.CommitsStorePath(repoName!, branchName!), x => x.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(commitEntryRow))
                {
                    var commitObj = DeserializeRowEntry(commitEntryRow);

                    if(commitObj is not null)
                    {
                        commitObj.RepoName = repoName;
                        commitObj.BranchName = branchName;

                        if(includeContent)
                        {
                            commitObj.Content = await DirectoryDB.ReadAllTextAsync(DBPaths.CommitLOBPath(repoName, branchName, commitHash));
                        }
                    }

                    return commitObj;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetCommitAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<CommitEntity?> GetLatestCommitAsync(string? repoName, string? branchName, bool includeContent = true)
        {
            try
            {
                //base commit address is branchname#commihash
                Validations.ThrowIfNullOrWhiteSpace(branchName, repoName);

                var commitEntryRow = await DirectoryDB.FirstOrDefaultRowAsync(DBPaths.ChangesetsHeadPath(repoName!, branchName!));

                if (!string.IsNullOrWhiteSpace(commitEntryRow))
                {
                    var commitObj = DeserializeRowEntry(commitEntryRow);

                    if(commitObj is not null)
                    {
                        commitObj.RepoName = repoName;
                        commitObj.BranchName = branchName;

                        if(includeContent)
                        {
                            commitObj.Content = await DirectoryDB.ReadAllTextAsync(DBPaths.CommitLOBPath(repoName, branchName, commitObj.Hash));
                        }
                    }

                    return commitObj;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetLatestCommitAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<CommitEntity?> GetOldestCommitAsync(string? repoName, string? branchName, bool includeContent = true)
        {
            try
            {
                //base commit address is branchname#commihash
                Validations.ThrowIfNullOrWhiteSpace(branchName, repoName);

                var commitEntryRow = await DirectoryDB.FirstOrDefaultRowAsync(DBPaths.CommitsStorePath(repoName!, branchName!));

                if (!string.IsNullOrWhiteSpace(commitEntryRow))
                {
                    var commitObj = DeserializeRowEntry(commitEntryRow);

                    if (commitObj is not null)
                    {
                        commitObj.RepoName = repoName;
                        commitObj.BranchName = branchName;

                        if (includeContent)
                        {
                            commitObj.Content = await DirectoryDB.ReadAllTextAsync(DBPaths.CommitLOBPath(repoName, branchName, commitObj.Hash));
                        }
                    }

                    return commitObj;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetLatestCommitAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<CommitEntity?> GetLatestMergeCommitAsync(string repoName, string branchName, string mergedBranchName, bool includeContent = true)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(branchName, repoName, mergedBranchName);

                var searchTerm = $"{Constants.Constants.ItemAddressDelimiter}{mergedBranchName}{Constants.Constants.ItemAddressDelimiter}";
                var commitEntryRow = await DirectoryDB.LastOrDefaultRowAsync(DBPaths.CommitsStorePath(repoName!, branchName!), row => row.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                /*
                 use DirectoryDB.LastOrDefaultRowAsync2 for a supposedly memory optimized approach.
                 */
                if (!string.IsNullOrWhiteSpace(commitEntryRow))
                {
                    var currentCommitObj = DeserializeRowEntry(commitEntryRow);
                    if (currentCommitObj is not null)
                    {
                        var mergeCommitHash = currentCommitObj.BaseCommitAddress?.Split(Constants.Constants.ItemAddressDelimiter)[^1];//currentBranch#currentBranchHash#MergedBranch#MergedBranchHash


                        if (string.IsNullOrWhiteSpace(mergeCommitHash))
                        {
                            throw new InvalidDataException("MergeCommitHash can't be null or empty");   
                        }

                        var mergeCommitObj = await GetCommitAsync(repoName, mergedBranchName, mergeCommitHash, includeContent);

                        mergeCommitObj.RepoName = repoName;
                        mergeCommitObj.BranchName = mergedBranchName;

                        return mergeCommitObj;
                    }
                    else
                    {
                        throw new InvalidDataException("Current Commit was not found.");
                    }
                }

                return await GetOldestCommitAsync(repoName, branchName);// If no merge commit present, return the first commit (which was created using the parent branch).This helps us in the comparison operation.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetLatestMergeCommitAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<List<CommitEntity>?> GetAllCommitsContentless(string? repoName, string? branchName)//not supporting lists of contentful commits since there is no use case I can think of. Also It would be a crazy heavy overhead.
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName, branchName);

                var matchingCommitEntryRows = await DirectoryDB.GetAllRowsAsync(DBPaths.CommitsStorePath(repoName, branchName));
                if (matchingCommitEntryRows != null && matchingCommitEntryRows.Length != 0)
                {
                    List<CommitEntity>? commitEntities = [];

                    foreach (var row in matchingCommitEntryRows)
                    {
                        var commitObj = DeserializeRowEntry(row);

                        if (commitObj is not null)
                        {
                            commitObj.RepoName = repoName;
                            commitObj.BranchName = branchName;

                            commitEntities.Add(commitObj);
                        }
                    }

                    return commitEntities;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllCommitsContentless)}\' " + ex.Message);
            }

            return null;
        }

        private static CommitEntity? DeserializeRowEntry(string? csvRowEntry)
        {
            if (string.IsNullOrWhiteSpace(csvRowEntry))
            {
                return null;
            }

            var columns = csvRowEntry.GetColumns();
            return new CommitEntity
            {
                Hash = columns[0],
                Message = columns[1],
                BaseCommitAddress = columns[2],
                Timestamp = columns[3]
            };
        }
    }
}
