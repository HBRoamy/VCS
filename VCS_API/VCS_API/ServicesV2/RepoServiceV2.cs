﻿using VCS_API.DirectoryDB.Repositories.Interfaces;
using VCS_API.Helpers;
using VCS_API.Models;
using VCS_API.ServicesV2.Interfaces;

namespace VCS_API.ServicesV2
{
    public class RepoServiceV2(IRepositoryRepo repositoryRepo) : IRepoServiceV2
    {
        public async Task<RepositoryEntity?> CreateRepo(RepositoryEntity? repoEntity)
        {
            try
            {
                Validations.ThrowIfNull(repoEntity);

                // Validate the name
                Validations.ThrowIfInvalidName(repoEntity?.Name, maxLength: 100, minLength: 5);

                // Validate that the repo doesn't already exist
                if ((await GetRepoAsync(repoEntity?.Name)) != null)
                {
                    throw new InvalidOperationException("The repo name already exists.");
                }

                var createResult = await repositoryRepo.CreateRepository(repoEntity);

                Validations.ThrowIfNull(createResult);

                return createResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(CreateRepo)}\' " + ex.Message);
            }

            return null;
        }

        public async Task DeleteRepoAsync(string? repoName)
        {
            try
            {
                await repositoryRepo.DeleteRepoAsync(repoName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(DeleteRepoAsync)}\' " + ex.Message);
            }
        }

        public async Task<List<RepositoryEntity>?> GetAllRepos()
        {
            try
            {
                return await repositoryRepo.GetAllReposAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetAllRepos)}\' " + ex.Message);
            }

            return null;
        }

        public async Task<RepositoryEntity?> GetRepoAsync(string? repoName)
        {
            try
            {
                var result = await repositoryRepo.GetRepoByNameAsync(repoName);

                //Will go in the controller
                ///var branches = await branchService.GetAllBranchesInRepoAsync(repoName);
                ///result.Branches = branches;

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetRepoAsync)}\' " + ex.Message);
            }

            return null;
        }

        public Task<RepositoryEntity?> UpdateRepoReadMe(string? repoName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(UpdateRepoReadMe)}\' " + ex.Message);
            }

            throw new NotImplementedException();
        }
    }
}