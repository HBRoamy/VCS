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
        public async Task<ActionResult> CommitChangesToBranchV2(string repoName, string branchName, [FromBody] CommitRequest commitEntity, [FromQuery] string? mergeBranchCommitHash)
        {
            var baseCommit = await commitServiceV2.GetLatestCommitAsync(repoName, branchName);

            var newBaseCommit = Constants.Constants.NullPlaceholder;
            var commitMessage = commitEntity.Message;
            if (baseCommit!=null)
            {
                newBaseCommit = string.Join(Constants.Constants.ItemAddressDelimiter, baseCommit.BranchName, baseCommit.Hash);//BaseBranch#BaseCommitHash

                if(!string.IsNullOrWhiteSpace(mergeBranchCommitHash))// For managing parent commits when a pull action happens
                {
                    var baseBranch = (await branchServiceV2.GetBranchAsync(branchName, repoName))?.ParentBranchName;
                    if( await commitServiceV2.GetCommitAsync(repoName, baseBranch, mergeBranchCommitHash) is null )
                    {
                        throw new InvalidOperationException("ERROR: Merge Commit couldn't be found in the base branch.");
                    }

                    // Here, we are relying on the frontend to send the merged code in the Content property
                    newBaseCommit = string.Join(Constants.Constants.ItemAddressDelimiter, newBaseCommit, baseBranch, mergeBranchCommitHash); //BaseBranch#BaseCommitHash#MergeBranch#MergeCommitHash
                    commitMessage = $"Merged commit {mergeBranchCommitHash} from the branch {baseCommit.BranchName}. Message: {commitEntity.Message}";
                }
            }

            var newCommitEntity = await commitServiceV2.CommitChanges(new CommitEntity
            {
                Hash = Guid.NewGuid().ToString().Replace("-", string.Empty),
                BranchName = branchName,
                RepoName = repoName,
                Message = commitMessage,
                Timestamp = DateTime.Now.ToString(),
                BaseCommitAddress = newBaseCommit,
                Content = commitEntity.Content
            });

            return Ok(newCommitEntity);
        }
    }
}
