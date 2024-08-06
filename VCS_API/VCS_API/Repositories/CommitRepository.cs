using System.Text;
using VCS_API.Extensions;
using VCS_API.Models;

namespace VCS_API.Repositories
{
    public class CommitRepository
    {
        // get by id
        // get all
        // get all by name
        // Add
        // DeleteById
        // DeleteAll
        // metadata file contains simply the commit info
        private readonly string storagePath; //repo+branch commits' storage path
        private readonly string changesStoragePath;//contains files with complete changes
        private readonly string metadataFilePath;//metadata file name which contains commit info (not content)
        private readonly string repoName;
        private readonly string branchName;
        public CommitRepository(string repoName, string branchName)
        {
            this.repoName = repoName;
            this.branchName = branchName;

            storagePath = $@"DataWarehouse\Commits\" + repoName + Constants.Constants.ItemAddressDelimiter + branchName;
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
                Console.WriteLine($"Directory created: {storagePath}");
            }

            metadataFilePath = Path.Combine(storagePath, "metadata.txt");
            if (!File.Exists(metadataFilePath))
            {
                File.Create(metadataFilePath).Close(); // Create and immediately close the file to make it empty
                Console.WriteLine($"File created: {metadataFilePath}");
            }

            changesStoragePath = Path.Combine(storagePath, "Changes");
            if (!Directory.Exists(changesStoragePath))
            {
                Directory.CreateDirectory(changesStoragePath);
                Console.WriteLine($"Directory created: {changesStoragePath}");
            }
        }

        public async Task<string> AddCommitAsync(CommitEntity commitEntity)
        {
            string commitChangeStoragePath = Path.Combine(storagePath, "Changes", commitEntity.Hash + ".txt");
            StringBuilder commitMetadataCSVSerialized = new();
            commitMetadataCSVSerialized
                .Append($"{commitEntity.Hash}").Append(Constants.Constants.StandardColumnDelimiter)
                .Append($"{commitEntity.Message}").Append(Constants.Constants.StandardColumnDelimiter)
                .Append($"{commitEntity.Timestamp}").Append(Constants.Constants.StandardColumnDelimiter)
                .Append($"{commitEntity.BaseCommitAddress}").Append(Constants.Constants.StandardColumnDelimiter)
                .Append($"{commitChangeStoragePath}");

            await WriteToFileAsync(metadataFilePath, commitMetadataCSVSerialized.ToString());//adding the commit metadata
            await WriteToFileAsync(Path.Combine(changesStoragePath, commitEntity.Hash! + ".txt"), commitEntity.Content?.Trim('\r').Trim('\n'), append: false);//adding the actual changes to a file, in future we must support different file extensions and multiple file changes under a single commit
            await WriteToFileAsync(Path.Combine(storagePath, "HEAD.txt"), commitMetadataCSVSerialized.ToString(), append: false);
            return commitChangeStoragePath;
        }

        /// <summary>
        /// Returns the latest commit on a branch.
        /// </summary>
        /// <param name="repoName"></param>
        /// <param name="branchName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<string?> FetchHead(string? repoName, string? branchName)
        {
            if (string.IsNullOrWhiteSpace(repoName)) throw new ArgumentNullException(nameof(repoName));
            if (string.IsNullOrWhiteSpace(branchName)) throw new ArgumentNullException(nameof(branchName));

            var headPath = Path.Combine("DataWarehouse", "Commits", $"{repoName}{Constants.Constants.ItemAddressDelimiter}{branchName}", "HEAD.txt")!;
            if (!File.Exists(headPath)) return null;
            var head = (await File.ReadAllLinesAsync(headPath)).FirstOrDefault();

            return head;
        }

        public CommitEntity? FindCommit(string repoName, string branchName, string? commitHash)
        {
            if (string.IsNullOrWhiteSpace(commitHash)) return null;
            var metaDataFile = Path.Combine("DataWarehouse", "Commits", $"{repoName}{Constants.Constants.ItemAddressDelimiter}{branchName}", "metadata.txt");
            var commit = File.ReadLines(metaDataFile).First(x => x.StartsWith(commitHash + Constants.Constants.StandardColumnDelimiter));

            if (string.IsNullOrWhiteSpace(commitHash)) return null;

            var columns = commit.GetColumns();

            return new CommitEntity
            {
                Hash = columns[0],
                Message = columns[1],
                Timestamp = columns[2],
                BaseCommitAddress = columns[3],
                ChangeStorageAddress = columns[4],
                RepoName = repoName,
                BranchName = branchName
            };
        }

        public async Task<CommitEntity?> GetCommittedContentByHash(string repoName, string branchName, string? commitHash)
        {
            if (string.IsNullOrWhiteSpace(commitHash)) return null;
            var changePath = Path.Combine("DataWarehouse", "Commits", $"{repoName}{Constants.Constants.ItemAddressDelimiter}{branchName}", "Changes", commitHash + ".txt");
            var foundCommit = FindCommit(repoName, branchName, commitHash);

            if (foundCommit is null) return null;

            var commitedContent = await File.ReadAllTextAsync(changePath);

            if (string.IsNullOrWhiteSpace(commitedContent)) return null;
            foundCommit.Content = commitedContent;

            return foundCommit;
        }

        public static async Task<string?> GetCommittedContent(string repoName, string branchName, string? commitHash)
        {
            if (string.IsNullOrWhiteSpace(commitHash)) return null;
            var changePath = Path.Combine("DataWarehouse", "Commits", $"{repoName}{Constants.Constants.ItemAddressDelimiter}{branchName}", "Changes", commitHash + ".txt");

            var commitedContent = await File.ReadAllTextAsync(changePath);

            return commitedContent;
        }

        public static async Task<string?> GetCommittedContentThroughCommitPath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                throw new ArgumentException("Commit content {path} not found",nameof(path));
            }

            return await File.ReadAllTextAsync(path);
        }

        public async Task<string?> GetLatestCommitHashOfBranch()
        {
            var lastRow = (await File.ReadAllLinesAsync(metadataFilePath))[^1];//fetching the last row
            return lastRow;
        }

        private static async Task WriteToFileAsync(string filePath, string? content, bool append = true)// enter false for overwriting the file
        {
            using var sw = new StreamWriter(filePath, append: append);
            await sw.WriteLineAsync(content);
        }
    }
}
