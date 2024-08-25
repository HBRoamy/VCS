using VCS_API.DirectoryDB.Repositories.Interfaces;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;
using VCS_API.ServicesV2.Interfaces;

namespace VCS_API.ServicesV2
{
    public class BranchServiceV2(IBranchRepo branchRepo, IRepoServiceV2 repoService, ICommitServiceV2 commitsService) : IBranchServiceV2
    {
        public async Task<BranchEntity?> CreateBranchAsync(BranchEntity? newBranch)
        {
            try
            {
                Validations.ThrowIfNull(newBranch);
                Validations.ThrowIfNullOrWhiteSpace(newBranch?.Name, newBranch?.RepoName);

                // validate the branch name
                Validations.ThrowIfInvalidName(newBranch?.Name, maxLength: 50);

                // validate that the repo exists
                Validations.ThrowIfNull(await repoService.GetRepoAsync(newBranch?.RepoName));

                // validate that the branch doesn't already exists
                var getBranchResult = await GetBranchAsync(newBranch?.Name, newBranch?.RepoName);// this will also prevent the user from creating a 2nd master branch

                if(getBranchResult != null)
                {
                    throw new InvalidOperationException("The branch name already exists.");
                }

                // validate that parent branch exists (not for the master branch)
                var isRootBranch = newBranch!.Name!.IsRootBranch();
                if(!isRootBranch)
                {
                    Validations.ThrowIfNullOrWhiteSpace(newBranch.ParentBranchName);

                    getBranchResult = await GetBranchAsync(newBranch.ParentBranchName, newBranch.RepoName);

                    if (getBranchResult is null)
                    {
                        throw new InvalidOperationException("The parent branch doesn't exist.");
                    }
                }

                var createBranchResult = await branchRepo.CreateBranchAsync(newBranch) ?? throw new Exception($"An error occured while creating the branch \'{newBranch.Name}\'.");
                
                var baseCommit = !isRootBranch ? await commitsService.GetLatestCommitAsync(newBranch.RepoName, newBranch.ParentBranchName) : null;
                var baseCommitAddress = (baseCommit!=null) ? $"{baseCommit.BranchName}{Constants.Constants.ItemAddressDelimiter}{baseCommit.Hash}" : Constants.Constants.NullPlaceholder;

                var firstCommit = await commitsService.CommitChanges(new CommitEntity
                {
                    BranchName = createBranchResult.Name,
                    RepoName = createBranchResult.RepoName,
                    Message = "Branch Initialized",
                    BaseCommitAddress = baseCommitAddress,
                    Content = (baseCommit != null) ? baseCommit.Content : string.Empty
                }) 
                ?? throw new Exception($"An error occured while creating the first commit for the branch \'{createBranchResult.Name}\'.");

                createBranchResult.Commits = [firstCommit];

                return createBranchResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(CreateBranchAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<List<BranchEntity>?> GetAllBranchesInRepoAsync(string? repoName)
        {
            try
            {
                return await branchRepo.GetBranchesByRepoNameAsync(repoName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllBranchesInRepoAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<BranchEntity?> GetBranchAsync(string? branchName, string? repoName, string? commitHash = null)
        {
            try
            {
                var branch = await branchRepo.GetBranchByNameAsync(branchName, repoName);

                if( branch != null)
                {
                    var commitObj = default(CommitEntity?);
                    if(!string.IsNullOrWhiteSpace(commitHash))
                    {
                        commitObj = await commitsService.GetCommitAsync(repoName, branchName, commitHash);
                    }
                    else
                    {
                        commitObj = await commitsService.GetLatestCommitAsync(repoName, branchName, includeContent: true);
                    }

                    branch.Commits = [commitObj];
                }

                return branch;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetBranchAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<BranchEntity?> DeleteBranchAsync(string? repoName, string? branchName)
        {
            try
            {
                if(branchName!=null && branchName.IsRootBranch())
                {
                    throw new InvalidOperationException("Can not delete the Master branch unless the whole repository is getting deleted.");
                }

                return await branchRepo.DeleteBranchByNameAsync(branchName, repoName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(DeleteBranchAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task DeleteAllBranchesInRepoAsync(string? repoName)
        {
            try
            {
                await branchRepo.DeleteAllBranchesByRepoNameAsync(repoName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(DeleteAllBranchesInRepoAsync)}\' " + ex.Message);
            }
        }

        public async Task GetBranchTreeForRepoAsync(string? repoName)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetBranchTreeForRepoAsync)}\' " + ex.Message);
            }
        }
    }
}
