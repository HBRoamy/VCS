using VCS_API.DirectoryDB.Helpers;
using VCS_API.Helpers;

namespace VCS_API.DirectoryDB.Repositories
{
    public static class AuditLogsRepo
    {
        public static void Log(string? repoName, string logStatement)
        {
			try
			{
                Validations.ThrowIfNullOrWhiteSpace(repoName);
                DirectoryDB.WriteToFile(DBPaths.RepoAuditLogsPath(repoName, DateTime.Now), logStatement, canCreateDirectory: true);
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
                var logStatement = DBHelper.AppendDelimited(endpointHttpMethod, endpoint, requestLifetimeDurationInMilliseconds.ToString(), DateTime.Now.ToString());
                DirectoryDB.WriteToFile(DBPaths.GlobalStatsLogsPath(endpointHttpMethod, DateTime.Now), logStatement, canCreateDirectory: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to write logs due to an unexpected error: {ex.Message}");
            }
        }
    }
}
