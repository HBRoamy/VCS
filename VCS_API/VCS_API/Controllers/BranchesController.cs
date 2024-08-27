using DiffPlex.DiffBuilder.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Models;
using VCS_API.Models.RequestModels;
using VCS_API.Models.ResponseModels;
using VCS_API.Services;
using VCS_API.Services.Interfaces;
using VCS_API.ServicesV2.Interfaces;

namespace VCS_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BranchesController(IBranchService branchService, IBranchServiceV2 branchServiceV2, IComparisonService comparisonService, ICommitService commitService, ICommitServiceV2 commitServiceV2, IPullServiceV2 pullServiceV2) : ControllerBase
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

        [HttpGet($"{Constants.Constants.RepoAndBranchCompare}/v2")]
        public async Task<ActionResult<DiffMergeEntity>> GetComparisonWithBaseV2(string repoName, string branchName)
        {
            var branchEntity = await branchServiceV2?.GetBranchAsync(branchName, repoName)!;
            Validations.ThrowIfNull(branchEntity);

            var diffMergeResult = await pullServiceV2.GetSideBySideComparisonForCommit(repoName,  branchName, branchEntity?.ParentBranchName);

            if (diffMergeResult == null)
            {
                return NotFound("No comparison could be done.");
            }

            return Ok(diffMergeResult);
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

        [HttpPost("{repoName}/{branchName}/CommitV2")]
        public async Task<ActionResult> CommitChangesToBranchV2(string repoName, string branchName, CommitRequest commitEntity)
        {
            var baseCommit = await commitServiceV2.GetLatestCommitAsync(repoName, branchName);
            var newCommitPath = await commitServiceV2.CommitChanges(new CommitEntity
            {
                Hash = Guid.NewGuid().ToString().Replace("-", string.Empty),
                BranchName = branchName,
                RepoName = repoName,
                Message = commitEntity.Message,
                Timestamp = DateTime.Now.ToString(),
                BaseCommitAddress = (baseCommit != null) ? $"{baseCommit.BranchName}{Constants.Constants.ItemAddressDelimiter}{baseCommit.Hash}" : Constants.Constants.NullPlaceholder,
                Content = commitEntity.Content
            });

            return Ok(newCommitPath);//TODO: WRONG since we are giving away our schema structure in this
        }
    }
}
