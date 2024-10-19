using System.Text.RegularExpressions;
using VCS_API.DirectoryDB.Helpers;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories
{
    public static class AuditLogsRepo
    {
        public static void Log(string? repoName, string logStatement)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName, logStatement);
                logStatement = DBHelper.AppendDelimited(DateTime.Now.ToString(), logStatement);
                DirectoryDB.WriteToFile(DBPaths.RepoAuditLogsPath(repoName!), logStatement, canCreateDirectory: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to write logs due to an unexpected error: {ex.Message}");
            }
        }

        public static void LogStats(string? endpoint, string? endpointHttpMethod, long requestLifetimeDurationInMilliseconds)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(endpoint, endpointHttpMethod);
                var logStatement = DBHelper.AppendDelimited(endpointHttpMethod, endpoint, DateTime.Now.ToString(), requestLifetimeDurationInMilliseconds.ToString());
                DirectoryDB.WriteToFile(DBPaths.GlobalStatsLogsPath(endpointHttpMethod!), logStatement, canCreateDirectory: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to write logs due to an unexpected error: {ex.Message}");
            }
        }

        public static async Task<Dictionary<string, List<HistoryFragment>>?> GetRepoHistoryLogsV2(string repoName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);
                Dictionary<string, List<HistoryFragment>> groupedHistoryFrags = [];

                await foreach (var frag in DirectoryDB.StreamRowsAsync(DBPaths.RepoAuditLogsPath(repoName.ToUpper())))
                {
                    if (string.IsNullOrWhiteSpace(frag)) continue;

                    var historyFrag = DeserializeHistoryFrag(frag);
                    var dateKey = historyFrag.Timestamp!.Trim()[..10];

                    if (!groupedHistoryFrags.ContainsKey(dateKey))
                    {
                        groupedHistoryFrags[dateKey] = [];
                    }

                    groupedHistoryFrags[dateKey].Add(historyFrag);
                }

                return groupedHistoryFrags;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to read history due to an unexpected error: {ex.Message}");
            }

            return null; // If something goes wrong, return null
        }

        public static async Task<List<HistoryFragment>?> GetRepoHistoryLogs(string repoName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);
                return (await DirectoryDB.GetAllRowsAsync(DBPaths.RepoAuditLogsPath(repoName.ToUpper())))?.Select(row => DeserializeHistoryFrag(row)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to read history due to an unexpected error: {ex.Message}");
            }
            return null;
        }

        private static HistoryFragment DeserializeHistoryFrag(string row)
        {
            Validations.ThrowIfNullOrWhiteSpace(row);
            var splitResult = row.GetColumns();

            return new HistoryFragment
            {
                Timestamp = splitResult[0],
                EventStatement = splitResult[1]
            };
        }
    }

    public record LogEntry(
    string? Timestamp,
    string? Operation,
    string? RepositoryName,
    string? BranchName,
    string? CommitHash,
    string? BaseCommitAddress,
    string? PullRequestId,
    string? Status
    );

    public static class LogParser
    {
        public static LogEntry ParseLogEntry(string logStatement)
        {
            Validations.ThrowIfNullOrWhiteSpace(logStatement);

            // Define the regex pattern to match the log entry components
            var pattern = @"^(?<Timestamp>\d{2}-\d{2}-\d{4} \d{2}:\d{2}:\d{2}), " +
                      @"(?<Operation>Created|Deleted|Opened|Updated) " +
                      @"(?:" +
                      @"the branch '(?<BranchName>[^']*)'(?: \(based on '(?<BaseCommitAddress>[^']*)'\))?" +
                      @"|a commit '(?<CommitHash>[^']*)'(?: \(based on '(?<BaseCommitAddress>[^']*)'\))?" +
                      @"|the repository '(?<RepositoryName>[^']*)'" +
                      @"|all the branches in the repository '(?<RepositoryName>[^']*)'" +
                      @"|status to '(?<Status>[^']*)' for the pull request '#(?<PullRequestId>\d+)'" +
                      @"|a pull request '#(?<PullRequestId>\d+)'" +
                      @")?" +
                      @"(?: in the repository '(?<RepositoryName>[^']*)')?";

            var match = Regex.Match(logStatement, pattern);

            if (match.Success)
            {
                var timestamp = match.Groups["Timestamp"]?.Value;
                var operation = match.Groups["Operation"]?.Value;
                var repoName = match.Groups["RepositoryName"]?.Value;
                var branchName = match.Groups["BranchName"]?.Value;
                var commitHash = match.Groups["CommitHash"]?.Value;
                var baseCommitAddress = match.Groups["BaseCommitAddress"]?.Value;
                var pullRequestId = match.Groups["PullRequestId"]?.Value;
                var status = match.Groups["Status"]?.Value;

                return new LogEntry(
                    timestamp,
                    operation,
                    string.IsNullOrWhiteSpace(repoName) ? null : repoName,
                    string.IsNullOrWhiteSpace(branchName) ? null : branchName,
                    string.IsNullOrWhiteSpace(commitHash) ? null : commitHash,
                    string.IsNullOrWhiteSpace(baseCommitAddress) ? null : baseCommitAddress,
                    string.IsNullOrWhiteSpace(pullRequestId) ? null : pullRequestId,
                    string.IsNullOrWhiteSpace(status) ? null : status
                );
            }
            else
            {
                throw new ArgumentException("Log line does not match the expected format.", nameof(logStatement));
            }
        }
    }
}
