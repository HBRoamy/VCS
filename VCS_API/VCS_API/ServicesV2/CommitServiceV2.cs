using System.Globalization;
using VCS_API.DirectoryDB.Helpers;
using VCS_API.DirectoryDB.Repositories.Interfaces;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;
using VCS_API.ServicesV2.Interfaces;

namespace VCS_API.ServicesV2
{
    public class CommitServiceV2(ICommitsRepo commitRepo) : ICommitServiceV2
    {
        public async Task<CommitEntity?> CommitChanges(CommitEntity? commitEntity)
        {
            try
            {
                Validations.ThrowIfNull(commitEntity);
                Validations.ThrowIfNullOrWhiteSpace(commitEntity?.Message, commitEntity?.BranchName, commitEntity?.RepoName);

                var isRootBranch = commitEntity!.BranchName!.IsRootBranch();
                if (!isRootBranch)
                {
                    Validations.ThrowIfNullOrWhiteSpace(commitEntity.BaseCommitAddress);

                    // Retrieving base commit info
                    var baseCommitBranchAndHash = SplitAddress(commitEntity!.BaseCommitAddress!);
                    var baseCommitBranch = baseCommitBranchAndHash[0];
                    var baseCommitHash = baseCommitBranchAndHash[1];
                    var baseCommitObject = await commitRepo.GetCommitAsync(commitEntity.RepoName, baseCommitBranch, baseCommitHash);

                    Validations.ThrowIfNull(baseCommitObject);

                    // The first commit of a branch can have the same changes as its base branch, it enables inheritance
                    var isFirstCommit = string.IsNullOrWhiteSpace((await GetLatestCommitAsync(commitEntity.RepoName, commitEntity.BranchName))?.Hash);
                    if (!isFirstCommit)
                    {
                        var baseContent = baseCommitObject?.Content?.CleanData();
                        var newContent = commitEntity.Content?.CleanData();

                        if (!commitEntity.IsMergeCommit && baseContent == newContent)
                        {
                            throw new InvalidOperationException("Nothing new found that could be committed.");
                        }

                        commitEntity.Content = newContent; // passing clean data to the commit
                    }
                }

                return await commitRepo.CreateCommitAsync(commitEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(CommitChanges)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<CommitEntity?> GetLatestCommitAsync(string? repoName, string? branchName, bool includeContent = true)
        {
            try
            {
                return await commitRepo.GetLatestCommitAsync(repoName, branchName, includeContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetLatestCommitAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<CommitEntity?> GetOldestCommitAsync(string? repoName, string? branchName, bool includeContent = true)
        {
            try
            {
                return await commitRepo.GetOldestCommitAsync(repoName, branchName, includeContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetOldestCommitAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<CommitEntity?> GetLatestMergeCommitOrDefaultAsync(string? repoName, string? branchName, string? mergedBranchName, bool includeContent = true)
        {
            try
            {
                return await commitRepo.GetLatestMergeCommitAsync(repoName, branchName, mergedBranchName, includeContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetLatestMergeCommitOrDefaultAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<CommitEntity?> GetCommitAsync(string? repoName, string? branchName, string? commitHash)
        {
            try
            {
                return await commitRepo.GetCommitAsync(repoName, branchName, commitHash);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetCommitAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<List<CommitEntity>?> GetAllCommitsAsync(string? repoName, string? branchName)
        {
            try
            {
                return await commitRepo.GetAllCommitsContentless(repoName, branchName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllCommitsAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<Dictionary<string,List<CommitEntity>>?> GetCommitsGroupedByDateAsync(string? repoName, string? branchName)
        {
            try
            {
                var allCommits = await GetAllCommitsAsync(repoName, branchName) ?? [];

                Dictionary<string, List<CommitEntity>> groupedCommits = [];

                foreach (var commit in allCommits)
                {
                    var date = commit.Timestamp!.Trim()[..10];

                    if(!groupedCommits.ContainsKey(date))
                    {
                        groupedCommits[date] = [];
                    }

                    groupedCommits[date].Add(commit);
                }

                return groupedCommits;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllCommitsAsync)}\' " + ex.Message);
            }

            return null;
        }

        private static string[] SplitAddress(string address)
        {
            return address.Split(Constants.Constants.ItemAddressDelimiter);
        }
    }
}
