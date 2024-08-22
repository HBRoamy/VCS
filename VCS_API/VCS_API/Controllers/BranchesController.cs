﻿using DiffPlex.DiffBuilder.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VCS_API.Extensions;
using VCS_API.Models;
using VCS_API.Models.RequestModels;
using VCS_API.Models.ResponseModels;
using VCS_API.Services;
using VCS_API.Services.Interfaces;

namespace VCS_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BranchesController(IBranchService branchService, IComparisonService comparisonService, ICommitService commitService) : ControllerBase
    {
        //[HttpGet(Constants.Constants.RepositoryName)]
        //public ActionResult GetBranchesByRepoName(string repoName)
        //{
        //    return null;
        //}

        [HttpGet(Constants.Constants.RepoAndBranchCompare)]
        public async Task<ActionResult<string>> GetComparisonWithBase(string repoName, string branchName)
        {
            var branchEntity = (await branchService?.GetBranchesByRepositoryNameAsync(repoName)!).Find(branch => string.Equals(branch.Name, branchName, StringComparison.OrdinalIgnoreCase))!;
            var lineByLineComparison = await comparisonService.GetSideBySideComparisonForCommit(branchEntity);

            if (lineByLineComparison.IsNullOrEmpty())
            {
                return NotFound("No comparison could be done.");
            }

            var serializedDiff = JsonConvert.SerializeObject(lineByLineComparison);

            return Ok(serializedDiff);
        }

        [HttpPost("{repoName}/{branchName}/Commit")]
        public async Task<ActionResult> CommitChangesToBranch(string repoName, string branchName,CommitRequest commitEntity)
        {
            var baseCommit = await commitService.FetchHead(repoName, branchName);
            var newCommitPath = await commitService.CommitChanges(new CommitEntity
            {
                Hash = Guid.NewGuid().ToString().Replace("-", string.Empty),
                BranchName = branchName,
                RepoName = repoName,
                Message = commitEntity.Message,
                Timestamp = DateTime.Now.ToString(),
                BaseCommitAddress = !string.IsNullOrWhiteSpace(baseCommit) ? baseCommit.GetColumns()[^1] : string.Empty,
                Content = commitEntity.Content
            });

            return Ok(newCommitPath);//TODO: WRONG since we are giving away our schema structure in this
        }
    }
}