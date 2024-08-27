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
                var logStatement = DBHelper.AppendDelimited(endpointHttpMethod, endpoint, DateTime.Now.ToString() , requestLifetimeDurationInMilliseconds.ToString());
                DirectoryDB.WriteToFile(DBPaths.GlobalStatsLogsPath(endpointHttpMethod!), logStatement, canCreateDirectory: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to write logs due to an unexpected error: {ex.Message}");
            }
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
                TimeStamp = splitResult[0],
                EventStatement = splitResult[1]
            };
        }
    }
}
