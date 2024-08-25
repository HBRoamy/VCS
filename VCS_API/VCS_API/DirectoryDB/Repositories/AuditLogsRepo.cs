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
    }
}
