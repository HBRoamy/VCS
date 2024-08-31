using System.Text.RegularExpressions;
using VCS_API.DirectoryDB.Helpers;
using VCS_API.DirectoryDB.Repositories.Interfaces;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories
{
    public class PullsRepo : IPullsRepo
    {
        public async Task<PullRequestEntity?> CreatePullRequest(PullRequestEntity? pullRequestEntity)
        {
            try
            {
                Validations.ThrowIfNull(pullRequestEntity);
                Validations.ThrowIfNullOrWhiteSpace(
                    pullRequestEntity?.BranchName,
                    pullRequestEntity?.RepoName,
                    pullRequestEntity?.TargetBranchName,
                    pullRequestEntity?.Title); //since it will be a key used for searching, we can't allow it to be empty.

                var lastPullEntryRow = await DirectoryDB.LastOrDefaultRowAsync(DBPaths.PullsStorePath(pullRequestEntity?.RepoName));
                int lastPullSerialId = !string.IsNullOrWhiteSpace(lastPullEntryRow) ? int.Parse(lastPullEntryRow.GetColumns()[0]): 0;
                pullRequestEntity!.PullSerialId = (lastPullSerialId + 1).ToString();
                var creationTime = DateTime.Now.ToString();

                var pullEntryRow = DBHelper.AppendDelimited(
                    pullRequestEntity.PullSerialId,
                    pullRequestEntity.BranchName,
                    pullRequestEntity.RepoName,
                    pullRequestEntity.TargetBranchName,
                    pullRequestEntity.Title,
                    ((int)PullStatus.Open).ToString(),
                    creationTime, /*this is last status changed timestamp*/
                    string.Join("|", pullRequestEntity!.Labels ?? [string.Empty]),
                    creationTime);

                await DirectoryDB.WriteToFileAsync(DBPaths.PullsStorePath(pullRequestEntity.RepoName), pullEntryRow, canCreateDirectory: true);
                await DirectoryDB.WriteToFileAsync(DBPaths.PullDescriptionLOBPath(pullRequestEntity.RepoName, pullRequestEntity.PullSerialId), pullRequestEntity.Description, append: false, canCreateDirectory: true);

                AuditLogsRepo.Log(pullRequestEntity.RepoName, $"Created a pull request \'#{pullRequestEntity.PullSerialId}\' in {pullRequestEntity.RepoName}.");

                return DeserializeRowEntry(pullEntryRow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(CreatePullRequest)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<List<PullRequestEntity>?> GetAllPullsAsync(string? repoName, PullStatus? pullStatus = null)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

                string?[]? matchingPullEntryRows = default;

                if(pullStatus is null)
                {
                   matchingPullEntryRows = await DirectoryDB.GetAllRowsAsync(DBPaths.PullsStorePath(repoName));
                }
                else
                {
                    string pattern = DBHelper.AppendDelimited("^.*?", ".*?", Regex.Escape(repoName!), ".*?", ".*?", ((int)pullStatus).ToString(), ".*?", ".*?", ".*?$");
                    matchingPullEntryRows = (await DirectoryDB.FilterRowsAsync(DBPaths.PullsStorePath(repoName), x => Regex.IsMatch(x, pattern, RegexOptions.IgnoreCase)));
                }

                if (matchingPullEntryRows != null && matchingPullEntryRows.Length != 0)
                {
                    return matchingPullEntryRows.Select(row => DeserializeRowEntry(row)!).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllPullsAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<PullRequestEntity?> ChangePullStatus(string? repoName, int pullSerialId, PullStatus pullStatus)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName);

            if (pullStatus == PullStatus.Open) throw new InvalidOperationException(); // this should go in service as its a business requirement

            //delete the existing pull and update the received pull and save it as a new row

            var searchTerm = pullSerialId + Constants.Constants.StandardColumnDelimiter;
            var deletedRow = await DirectoryDB.DeleteRowAsync(DBPaths.PullsStorePath(repoName), row => row.StartsWith(searchTerm));
            if (!string.IsNullOrWhiteSpace(deletedRow))
            {
                try
                {
                    var pullObj = DeserializeRowEntry(deletedRow)!;
                    if (pullObj.Status != PullStatus.Open) throw new InvalidOperationException();

                    pullObj.Status = pullStatus;
                    pullObj.LastStatusChangeTimestamp = DateTime.Now.ToString();

                    var pullEntryRow = DBHelper.AppendDelimited(
                        pullObj.PullSerialId,
                        pullObj.BranchName,
                        pullObj.RepoName,
                        pullObj.TargetBranchName,
                        pullObj.Title,
                        $"{(int)pullObj.Status}",
                        pullObj.LastStatusChangeTimestamp,
                        string.Join("|", pullObj!.Labels ?? [string.Empty]),
                        pullObj.CreationTime);

                    await DirectoryDB.WriteToFileAsync(DBPaths.PullsStorePath(repoName), pullEntryRow);

                    var statusAsString = string.Empty;

                    if(pullStatus.Equals(PullStatus.Merged))
                    {
                        statusAsString = "Merged";
                    }
                    else if(pullStatus.Equals(PullStatus.Closed))
                    {
                        statusAsString = "Closed";
                    }
                    else
                    {
                        statusAsString = "BAD STATE DETECTED";
                    }
                        
                    AuditLogsRepo.Log(repoName, $"Updated status to \'{statusAsString}\' for the pull request \'#{pullSerialId}\' in {repoName}.");

                    return pullObj;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured in the method \'{nameof(ChangePullStatus)}\' " + ex.Message);
                }
            }

            return null;
        }

        public async Task<PullRequestEntity?> GetPullBySerialIdAsync(int serialId, string? repoName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

                var searchTerm = serialId + Constants.Constants.StandardColumnDelimiter;
                var matchingPullEntryRow = await DirectoryDB.FirstOrDefaultRowAsync(DBPaths.PullsStorePath(repoName), row => row.StartsWith(searchTerm));

                return DeserializeRowEntry(matchingPullEntryRow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllPullsAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<PullRequestEntity?> GetPullByBranchName(string? branchName, string? repoName)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName, branchName);

            string pattern = DBHelper.AppendDelimited("^.*?", Regex.Escape(branchName!), ".*?", ".*?", ".*?", ".*?", ".*?", ".*?", ".*?$"); //item should be in the second column only

            var matchingRow = await DirectoryDB.FirstOrDefaultRowAsync(DBPaths.PullsStorePath(repoName), (x) => Regex.IsMatch(x, pattern, RegexOptions.IgnoreCase));

            return DeserializeRowEntry(matchingRow);
        }

        private static PullRequestEntity? DeserializeRowEntry(string? csvRowEntry)
        {
            if (string.IsNullOrWhiteSpace(csvRowEntry))
            {
                return null;
            }

            var columns = csvRowEntry.GetColumns();

            PullStatus status = default;
            if (!string.IsNullOrWhiteSpace(columns[5]))
            {
                status = (PullStatus)int.Parse(columns[5]);
            }

            return new PullRequestEntity
            {
                PullSerialId = columns[0],
                BranchName = columns[1],
                RepoName = columns[2],
                TargetBranchName = columns[3],
                Title = columns[4],
                Status = status,
                LastStatusChangeTimestamp = columns[6],
                Labels = [.. columns[7].Split("|")],
                CreationTime = columns[8]
            };
        }
    }
}
