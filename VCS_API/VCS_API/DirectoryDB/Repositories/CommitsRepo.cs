using VCS_API.DirectoryDB.Helpers;
using VCS_API.Helpers;
using VCS_API.Models;

namespace VCS_API.DirectoryDB.Repositories
{
    public class CommitsRepo
    {
        public async Task<CommitEntity?> CreateCommit(CommitEntity? commitEntity)
        {
			try
			{
				//base commit address is branchname#commihash
				Validations.ThrowIfNullOrWhiteSpace(commitEntity?.Hash ,commitEntity?.BranchName, commitEntity?.BaseCommitAddress, commitEntity?.RepoName, commitEntity?.Timestamp, commitEntity?.Message, commitEntity?.Content);

				var commitRowEntry = DBHelper.AppendDelimited(commitEntity?.Hash, commitEntity?.Message, commitEntity?.Timestamp, commitEntity?.BaseCommitAddress);

				await DirectoryDB.WriteToFileAsync(DBPaths.CommitsStorePath(commitEntity?.RepoName!, commitEntity?.BranchName!), commitRowEntry);
				await DirectoryDB.WriteToFileAsync(DBPaths.CommitLOBPath(commitEntity?.RepoName!, commitEntity?.BranchName!, commitEntity?.Hash), commitEntity?.Content, canCreateDirectory: true);
                AuditLogsRepo.Log(commitEntity?.RepoName, $"Created a commit \'{commitEntity?.Hash}\' (based on \'{commitEntity?.BaseCommitAddress}\') in the branch \'{commitEntity?.BranchName}\' in the repository \'{commitEntity?.RepoName}\' at {DateTime.Now}.");

            }
            catch (Exception ex)
			{
                Console.WriteLine($"An error occured in the method \'{nameof(CreateCommit)}\' " + ex.Message);
            }

			return null;
        }
    }
}
