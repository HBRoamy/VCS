using VCS_API.Extensions;
using VCS_API.Models;
using VCS_API.Repositories;
using VCS_API.Services.Interfaces;

namespace VCS_API.Services
{
    public class CommitService : ICommitService
    {
        // Implement Change detection service as well in future
        public async Task<string> CommitChanges(CommitEntity commitEntity)
        {
            var repoName = commitEntity.RepoName;
            var branchName = commitEntity.BranchName;
            if(string.IsNullOrWhiteSpace(commitEntity.Message)) throw new InvalidOperationException(nameof(commitEntity.Message));

            // check repo exists, check branch exists, check base commit hash exists, check hasChanges
#pragma warning disable CS8604 // Null repo, branch names and base commit hash are handled inside their Find() methods

            var commitRepo = new CommitRepository(repoName, branchName);

            var isRootBranch = string.Equals(commitEntity.BranchName, Constants.Constants.MasterBranchName, StringComparison.OrdinalIgnoreCase);
            if(!isRootBranch)
            {
                var baseCommit = !string.IsNullOrWhiteSpace(commitEntity.BaseCommitAddress) ? commitEntity.BaseCommitAddress.GetColumns("\\") : throw new InvalidOperationException("Commit Should be based on another commit.");
                var baseCommitHash = baseCommit[4].Replace(".txt", "");//Check if fetch head can do it
                var parentBranch = baseCommit[2].Split(Constants.Constants.ItemAddressDelimiter)[1];
                var baseCommitWithContent = await GetContentfulCommitAsync(repoName,parentBranch, baseCommitHash, commitRepo);
                // Fetch head, if its empty, dont compare texts
                var isFirstCommit = string.IsNullOrEmpty(await FetchHead(repoName, branchName));
                if(!isFirstCommit && baseCommitWithContent?.Content?.Trim('\r').Trim('\n') == commitEntity.Content) throw new InvalidOperationException("Nothing new found that could be committed.");
            }
#pragma warning restore CS8604 // Possible null reference argument.

            var changesFileAddress = await commitRepo.AddCommitAsync(commitEntity);
            return changesFileAddress;
        }

        public async Task<CommitEntity?> GetContentlessCommitAsync(string repoName, string branchName, string commitHash, CommitRepository commitRepo)
        {
            return commitRepo.FindCommit(repoName, branchName, commitHash);
        }

        public async Task<CommitEntity?> GetContentlessCommitAsync(string repoName, string branchName, string commitHash)
        {
            return new CommitRepository(repoName, branchName).FindCommit(repoName, branchName, commitHash);
        }

        public async Task<CommitEntity?> GetContentfulCommitAsync(string repoName, string branchName,string commitHash, CommitRepository commitRepo)
        {
            return await commitRepo.GetCommittedContentByHash(repoName, branchName,commitHash);
        }

        public async Task<CommitEntity?> GetContentfulCommitAsync(string repoName, string branchName, string commitHash)
        {
            return await new CommitRepository(repoName, branchName).GetCommittedContentByHash(repoName, branchName, commitHash);
        }

        public async Task<string?> GetLatestCommitHashFromBranch(string branchName, string repoName)
        {
            return await (new CommitRepository(repoName,branchName)).GetLatestCommitHashOfBranch();
        }

        public async Task<string?> FetchHead(string repoName, string branchName)
        {
            return await CommitRepository.FetchHead(repoName, branchName);
        }

        public async Task<string?> GetCommittedContentThroughContentPath(string commitContentPath)
        {
            return await CommitRepository.GetCommittedContentThroughCommitPath(commitContentPath);
        }
    }
}
