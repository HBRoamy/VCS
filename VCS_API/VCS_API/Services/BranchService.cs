using System.Text;
using VCS_API.Extensions;
using VCS_API.Models;
using VCS_API.Repositories.Interfaces;
using VCS_API.Services.Interfaces;

namespace VCS_API.Services
{
    public class BranchService(IRepository<BranchEntity> branchRepo, IRepoService repoService, ICommitService commitService) : IBranchService
    {
        private static readonly HashSet<char> prohibitedCharacters = ['#', '@', '$', '%', '^', '&', '*', '(', ')', '!', '~', '\"', '/', '\\', '|', '`', '=', '+', '?', ',', '>', '<', '[', ']', '{', '}', ';', ':', '\'', ' ', '\n', '\t'];
        public async Task<string?> CreateBranchAsync(BranchEntity newBranch)
        {
            try
            {
                if (await IsCreatableBranchAsync(newBranch))
                {
                    var baseCommit = newBranch.IsRootBranch() ? string.Empty : await commitService.GetLatestCommitHashFromBranch(newBranch.ParentBranchName, newBranch.RepoName);
                    _ = await commitService.CommitChanges(new CommitEntity
                    {
                        Hash = Guid.NewGuid().ToString().Replace("-", string.Empty),
                        BranchName = newBranch.Name,
                        RepoName = newBranch.RepoName,
                        Message = "Branch Initialized",
                        Timestamp = newBranch.CreationTime,
                        BaseCommitAddress = !string.IsNullOrWhiteSpace(baseCommit) ? baseCommit.GetColumns()[^1] : string.Empty,
                        Content = !string.IsNullOrWhiteSpace(baseCommit) ? await commitService.GetCommittedContentThroughContentPath(baseCommit.GetColumns()[^1]) : string.Empty
                    });

                    var branchData = new StringBuilder()
                        .Append(newBranch.Name).Append(Constants.Constants.StandardColumnDelimiter)
                        .Append(newBranch.RepoName).Append(Constants.Constants.StandardColumnDelimiter)
                        .Append(newBranch.ParentBranchName).Append(Constants.Constants.StandardColumnDelimiter)
                        .Append(newBranch.CreationTime)
                        .ToString();

                    var branchPath = await branchRepo.Create(branchData);
                    
                    return branchPath;
                }
            }
            catch (ArgumentException)
            {
                throw; // Re-throw known exceptions without wrapping them.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error was encountered while creating the branch: {ex.Message}", ex);
            }

            return null;
        }

        private async Task<bool> IsCreatableBranchAsync(BranchEntity newBranch)
        {
            ArgumentNullException.ThrowIfNull(newBranch);

            if (!newBranch.Name.IsValidName(prohibitedCharacters, 50))
            {
                throw new ArgumentException("Invalid Branch Name", newBranch.Name);
            }

            _ = await repoService.GetRepoByNameAsync(newBranch.RepoName ?? "---") ?? throw new ArgumentException("Repo does not exist.", newBranch.RepoName);
            var isRootBranch = string.Equals(newBranch.Name, Constants.Constants.MasterBranchName, StringComparison.OrdinalIgnoreCase);
            if (!isRootBranch)
            {
                var parentBranchExists = await IsBranchPresentInRepo(newBranch.ParentBranchName, newBranch.RepoName);
                if (!parentBranchExists)
                {
                    throw new ArgumentException("Parent Branch does not exist.", newBranch.ParentBranchName);
                }

                var branchAlreadyExists = !string.IsNullOrWhiteSpace(await branchRepo.FindAsync(row => row.StartsWith(newBranch.Name + Constants.Constants.StandardColumnDelimiter ?? "---")));
                if (branchAlreadyExists)
                {
                    throw new ArgumentException("Branch already exists", newBranch.Name);
                }
            }

            return true;
        }

        public async Task<bool> IsBranchPresentInRepo(string branch, string repo)
        {
            if (string.IsNullOrWhiteSpace(repo) || string.IsNullOrWhiteSpace(branch)) return false;

            return !string.IsNullOrWhiteSpace(await branchRepo.FindAsync(row => row.StartsWith(branch + Constants.Constants.StandardColumnDelimiter + repo + Constants.Constants.StandardColumnDelimiter ?? "---", StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<List<BranchEntity>>? GetBranchesByRepositoryNameAsync(string? repoName)
        {
            var searchResults = await branchRepo.FindModifiedAsync(Constants.Constants.StandardColumnDelimiter + repoName + Constants.Constants.StandardColumnDelimiter);
            List<BranchEntity> result = [];

            if (!searchResults?.IsNullOrEmpty() ?? false)
            {
                foreach (var branch in searchResults)
                {
                    var columns = branch.GetColumns();
                    result.Add(new BranchEntity
                    {
                        Name = columns[0],
                        ParentBranchName = columns[2],
                        RepoName = columns[1],
                        CreationTime = columns[3]
                    });
                }
            }

            return result;
        }

        public async Task<int> DeleteAllBranchesIn(string? repoName)
        {
            _ = await repoService.GetRepoByNameAsync(repoName ?? "---") ?? throw new ArgumentException("Repo does not exist.", repoName);
            return branchRepo.Delete(row => row.Contains($"{Constants.Constants.StandardColumnDelimiter}{repoName}{Constants.Constants.StandardColumnDelimiter}", StringComparison.OrdinalIgnoreCase));
        }

        //public static string AppendValues<TEntity>(TEntity entity, string[] propertiesToSkip)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException(nameof(entity));

        //    var type = typeof(TEntity);
        //    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    var sb = new StringBuilder();

        //    foreach (var property in properties)
        //    {
        //        if (!propertiesToSkip.Contains(property.Name))
        //        {
        //            var value = property.GetValue(entity, null);
        //            if (value != null)
        //            {
        //                sb.Append(value.ToString());
        //                sb.Append(", ");
        //            }
        //        }
        //    }

        //    // Remove the last comma and space
        //    if (sb.Length > 0)
        //        sb.Length -= 2;

        //    return sb.ToString();
        //}
    }
}
