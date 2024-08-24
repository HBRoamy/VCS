using VCS_API.Helpers;

namespace VCS_API.DirectoryDB.Repositories
{
    public static class AuditLogsRepo
    {
        public static void Log(string? repoName, string logStatement)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName);
            DirectoryDB.WriteToFile(DBPaths.RepoAuditLogsPath(repoName, DateTime.Now), logStatement); // Not making it
        }
    }
}
