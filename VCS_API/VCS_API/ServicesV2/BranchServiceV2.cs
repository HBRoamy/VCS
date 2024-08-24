﻿using VCS_API.DirectoryDB.Repositories.Interfaces;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;
using VCS_API.ServicesV2.Interfaces;

namespace VCS_API.ServicesV2
{
    public class BranchServiceV2(IBranchRepo branchRepo, ICommitServiceV2 commitsService) : IBranchServiceV2
    {
        public async Task<BranchEntity?> CreateBranchAsync(BranchEntity? newBranch)
        {
            try
            {
                Validations.ThrowIfNull(newBranch);
                Validations.ThrowIfNullOrWhiteSpace(newBranch?.Name, newBranch?.RepoName);

                // validate branch name
                Validations.ThrowIfInvalidName(newBranch?.Name, maxLength: 50);

                // validate that repo exists
                throw new NotImplementedException("repo validation missing");

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
                var baseCommitAddress = (baseCommit!=null) ? $"{baseCommit.RepoName}{Constants.Constants.ItemAddressDelimiter}{baseCommit.BranchName}" : Constants.Constants.NullPlaceholder;

                _ = await commitsService.CommitChanges(new CommitEntity
                {
                    BranchName = createBranchResult.Name,
                    RepoName = createBranchResult.RepoName,
                    Message = "Branch Initialized",
                    BaseCommitAddress = baseCommitAddress,
                    Content = (baseCommit != null) ? baseCommit.Content : string.Empty
                }) 
                ?? throw new Exception($"An error occured while creating the first commit for the branch \'{createBranchResult.Name}\'.");
                
                return createBranchResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(CreateBranchAsync)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<List<BranchEntity>?>? GetAllBranchesInRepoAsync(string? repoName)
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

        public async Task<BranchEntity?> GetBranchAsync(string? branchName, string? repoName)
        {
            try
            {
                return await branchRepo.GetBranchByNameAsync(branchName, repoName);
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
