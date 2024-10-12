using Microsoft.AspNetCore.Mvc;
using VCS_API.Helpers;
using VCS_API.Models;
using VCS_API.Models.RequestModels;
using VCS_API.Models.ResponseModels;
using VCS_API.ServicesV2.Interfaces;
using static VCS_API.ServicesV2.BranchServiceV2;

namespace VCS_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RepositoriesController(IRepoServiceV2 repoServiceV2, IBranchServiceV2 branchServiceV2) : ControllerBase
    {
        [HttpGet("v2")]
        public async Task<ActionResult<List<RepositoryResponse>>> GetAllRepositoriesInfoV2()
        {
            var repos = await repoServiceV2.GetAllRepos();

            if (repos is null || repos.Count == 0)
            {
                return NotFound("No repositories found.");
            }

            var responseList = new List<RepositoryResponse>();
            foreach (var repo in repos)
            {
                responseList.Add(new RepositoryResponse
                {
                    Name = repo.Name,
                    CreationTime = repo.CreationTime,
                    Description = repo.Description,
                    IsPrivate = repo.IsPrivate
                });
            }

            return Ok(responseList);
        }

        [HttpGet($"{Constants.Constants.RepositoryName}/v2")]
        public async Task<ActionResult<RepositoryEntity>> GetRepositoryV2([FromRoute] string repoName)
        {
            //fetches name, branches (integration with branch service), etc.
            var repo = await repoServiceV2.GetRepoAsync(repoName);

            if (repo is not null)
            {
                var branches = await branchServiceV2.GetAllBranchesInRepoAsync(repoName)!;
                repo.Branches = branches;
                return Ok(repo);
            }

            return BadRequest("No such repo exists.");
        }

        [HttpGet($"{Constants.Constants.RepositoryName}/History")]
        public async Task<ActionResult<List<HistoryFragment>>> GetRepositoryHistoryV2([FromRoute] string repoName)
        {
            //fetches name, branches (integration with branch service), etc.
            var repoHistory = await repoServiceV2.GetRepoHistoryAsync(repoName);

            if (repoHistory is not null)
            {
                return Ok(repoHistory);
            }
            else
            {
                return NotFound("The history could not be found. Please verify that its a valid repository.");
            }
        }

        [HttpPost("v2")]
        public async Task<ActionResult> CreateRepositoryV2([FromBody] RepositoryRequest repositoryRequestBody)
        {
            //checks that the name is unique first, ordinal ignore case, also that it shouldn't contain special characters except period and underscores
            Validations.ThrowIfNull(repositoryRequestBody);

            try
            {
                var newRepo = new RepositoryEntity
                {
                    //In the future, the Id will be org+repoName
                    Name = repositoryRequestBody.Name,
                    Description = repositoryRequestBody.Description,
                    IsPrivate = repositoryRequestBody.IsPrivate
                };
                var result = await repoServiceV2.CreateRepo(newRepo);

                if (result != null)
                {
                    var createBranchResult = await branchServiceV2.CreateBranchAsync(new BranchEntity
                    {
                        Name = Constants.Constants.MasterBranchName,
                        RepoName = result.Name,
                        ParentBranchName = Constants.Constants.NullPlaceholder
                    });

                    Validations.ThrowIfNull(createBranchResult);

                    result.Branches = [createBranchResult];

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest("Either repo name already taken or is invalid due to usage of prohibited characters or is longer than 100 characters");
        }

        [HttpPost($"{Constants.Constants.RepoAndBranchName}/v2/")]
        public async Task<ActionResult> CreateBranchInRepositoryV2([FromRoute] string repoName, string baseBranch, [FromBody] BranchRequest branchReqBody)
        {
            if (branchReqBody == null) return BadRequest("Request body can not be null");

            try
            {
                var newBranch = new BranchEntity
                {
                    Name = branchReqBody.Name,
                    RepoName = repoName,
                    ParentBranchName = baseBranch,
                };

                var response = await branchServiceV2.CreateBranchAsync(newBranch);
                Validations.ThrowIfNull(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost($"{Constants.Constants.RepositoryName}/v2/ReadMe")]
        public async Task<ActionResult<string>> UpdateRepoReadme([FromRoute] string repoName, [FromBody] string readmeBody)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName);

                var response = await repoServiceV2.UpdateRepoReadMe(repoName, readmeBody);
                Validations.ThrowIfNull(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{repoName}/{branchName}/v2")]
        public async Task<ActionResult<BranchEntity?>> GetBranchV2(string repoName, string branchName, [FromQuery] string? commitHash)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(branchName, repoName);

                return await branchServiceV2.GetBranchAsync(branchName, repoName, commitHash);
            }
            catch (Exception ex)
            {
                return BadRequest($"An unexpected error occurred when getting the branch. {ex.Message}");
            }
        }

        [HttpDelete(Constants.Constants.RepositoryName)]
        public Task<ActionResult> DeleteRepository(string repoName)
        {
           throw new NotImplementedException();
        }

        [HttpGet($"{Constants.Constants.RepositoryName}/BranchTree")]
        public async Task<ActionResult<RawNodeDatum?>> GetBranchTree(string repoName)
        {
            Validations.ThrowIfNullOrWhiteSpace(repoName);

            var treeRoot = await branchServiceV2.GetBranchTreeForRepoAsync(repoName);

            if (treeRoot != null)
            {
                return Ok(treeRoot);
            }

            return BadRequest("The tree couldn't be formed.");
        }
    }
}
