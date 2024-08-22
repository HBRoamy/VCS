namespace VCS_API.DirectoryDB
{
    internal static class DBPaths
    {
        private const string ParentPath = "DataWarehouse";
        private const string AuditLogsPath = "AuditLogs";
        private const string RepositoriesPath = "Repositories";
        private const string Entities = "Entities";
        private const string ChangeSetsPath = "ChangeSets";

        #region Repositories
        public static string RepoStorePath() => Path.Combine(ParentPath, RepositoriesPath, "RepositoryStore.txt");
        public static string PullsStorePath(string? repoName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(repoName);
            return Path.Combine(ParentPath, RepositoriesPath, Entities, repoName, "PullsStore.txt");
        }

        public static string BranchStorePath(string? repoName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(repoName);
            return Path.Combine(ParentPath, RepositoriesPath, Entities, repoName, "BranchStore.txt");
        }
        #endregion

        #region ChangeSets
        public static string ChangesetsHeadPath(string? repoName, string? branchName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(repoName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(branchName);

            return Path.Combine(ParentPath, ChangeSetsPath, $"{repoName}#{branchName}", "Head.txt");
        }

        public static string CommitsStorePath(string? repoName, string? branchName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(repoName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(branchName);

            return Path.Combine(ParentPath, ChangeSetsPath, $"{repoName}#{branchName}", "CommitStore.txt");
        }

        public static string CommitLOBPath(string? repoName, string? branchName, string? commitHash)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(repoName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(branchName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(commitHash);

            return Path.Combine(ParentPath, ChangeSetsPath, $"{repoName}#{branchName}", "CommitLOBs", $"{commitHash}.txt");
        }
        #endregion

        #region Audit Logs
        public static string RepoAuditLogsPath(string? repoName, DateTime? dateTime)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(repoName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(dateTime?.ToString());

            return Path.Combine(ParentPath, AuditLogsPath, repoName, $"{dateTime?.Date}.txt");
        }
        #endregion
    }
}
