using VCS_API.Helpers;

namespace VCS_API.DirectoryDB
{
    internal static class DBPaths
    {
        private const string ParentPath = "DataWarehouseV2";
        private const string AuditLogsPath = "AuditLogs";
        private const string RepositoriesPath = "Repositories";
        private const string Entities = "Entities";
        private const string LOBs = "LOBs";
        private const string Stats = "Stats";

        #region Repositories
        public static string RepoStorePath() => Path.Combine(ParentPath, RepositoriesPath, "RepositoryStore.txt");
        public static string PullsStorePath(string? repoName)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName);
            return Path.Combine(ParentPath, RepositoriesPath, Entities, repoName!, "PullsStore.txt");
        }

        public static string BranchStorePath(string? repoName)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName);
            return Path.Combine(ParentPath, RepositoriesPath, Entities, repoName!, "BranchStore.txt");
        }
        #endregion

        #region ChangeSets
        public static string ChangesetsHeadPath(string? repoName, string? branchName)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName, branchName);

            return Path.Combine(ParentPath, LOBs, $"{repoName}#{branchName}", "Head.txt");
        }

        public static string CommitsStorePath(string? repoName, string? branchName)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName, branchName);

            return Path.Combine(ParentPath, LOBs, $"{repoName}#{branchName}", "CommitStore.txt");
        }

        public static string CommitLOBPath(string? repoName, string? branchName, string? commitHash)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName, branchName, commitHash);

            return Path.Combine(ParentPath, LOBs, $"{repoName}#{branchName}", "CommitLOBs", $"{commitHash}.txt");
        }

        private static string MarkdownLOBPath(string? id)
        {
            Validations.ThrowIfNullOrWhiteSpace(id);

            return Path.Combine(ParentPath, LOBs, "MarkdownLOBs", $"{id}.md");
        }

        public static string ReadMeLOBPath(string? repoName)
        {
            Validations.ThrowIfNullOrWhiteSpace( repoName);

            return MarkdownLOBPath($"{repoName}#ReadMe");
        }

        public static string PullDescriptionLOBPath(string? repoName, string? pullSerialId)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName, pullSerialId);

            return MarkdownLOBPath($"{repoName}#{pullSerialId}");
        }
        #endregion

        #region Audit Logs
        public static string RepoAuditLogsPath(string repoName)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName);

            return Path.Combine(ParentPath, AuditLogsPath, $"{repoName.ToUpper()}.txt");
        }

        public static string GlobalStatsLogsPath(string endpointHttpMethod)
        {
            Validations.ThrowIfNullOrWhiteSpace(endpointHttpMethod);

            return Path.Combine(ParentPath, Stats, $"{endpointHttpMethod?.ToUpper()}.txt");
        }
        #endregion
    }
}
