using Microsoft.AspNetCore.Mvc;
using VCS_API.Helpers;
using VCS_API.Models;
using VCS_API.Models.RequestModels;
using VCS_API.ServicesV2.Interfaces;
using static VCS_API.ServicesV2.BranchServiceV2;

namespace VCS_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BranchesController(IBranchServiceV2 branchServiceV2, ICommitServiceV2 commitServiceV2, IPullServiceV2 pullServiceV2) : ControllerBase
    {

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

        [HttpPost("{repoName}/{branchName}/CommitV2")]
        public async Task<ActionResult> CommitChangesToBranchV2(string repoName, string branchName, CommitRequest commitEntity)
        {
            var baseCommit = await commitServiceV2.GetLatestCommitAsync(repoName, branchName);
            var newCommitEntity = await commitServiceV2.CommitChanges(new CommitEntity
            {
                Hash = Guid.NewGuid().ToString().Replace("-", string.Empty),
                BranchName = branchName,
                RepoName = repoName,
                Message = commitEntity.Message,
                Timestamp = DateTime.Now.ToString(),
                BaseCommitAddress = (baseCommit != null) ? $"{baseCommit.BranchName}{Constants.Constants.ItemAddressDelimiter}{baseCommit.Hash}" : Constants.Constants.NullPlaceholder,
                Content = commitEntity.Content
            });

            return Ok(newCommitEntity);
        }
    }
}
