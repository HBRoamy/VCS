using Microsoft.AspNetCore.Mvc;
using VCS_API.Extensions;
using VCS_API.Models;
using VCS_API.Models.RequestModels;
using VCS_API.Models.ResponseModels;
using VCS_API.Services.Interfaces;

namespace VCS_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RepositoriesController(IRepoService repoService, IBranchService branchService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<RepositoryResponse>>> GetAllRepositoriesInfo()
        {
            var repos = await repoService.GetAllRepos();

            if(repos.IsNullOrEmpty())
            {
                return NotFound("No repositories found.");
            }

            var responseList = new List<RepositoryResponse>();
            foreach(var repo in repos)
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

        [HttpGet(Constants.Constants.RepositoryName)]
        public async Task<ActionResult<RepositoryEntity>> GetRepositoryByName([FromRoute] string repoName)
        {
            //fetches name, branches (integration with branch service), etc.
            var repo = await repoService.GetRepoByNameAsync(repoName);

            if(repo is not null)
            {
                var branches = await branchService.GetBranchesByRepositoryNameAsync(repoName)!;
                repo.Branches = branches;
                return Ok(repo);
            }

            return BadRequest("No such repo exists.");
        }

        [HttpPost]
        public async Task<ActionResult> CreateRepository([FromBody] RepositoryRequest repositoryRequestBody)
        {
            //checks that the name is unique first, ordinal ignore case, also that it shouldn't contain special characters except period and underscores
            if (repositoryRequestBody == null) return BadRequest("Request body can not be null");
            try
            {
                var newRepo = new RepositoryEntity
                {
                    Id = repositoryRequestBody.Name, //in future Id will be org+repoName
                    Name = repositoryRequestBody.Name,
                    CreationTime = DateTime.Now.ToString(),
                    Description = repositoryRequestBody.Description,
                    IsPrivate = repositoryRequestBody.IsPrivate
                };
                var result = await repoService.CreateRepo(newRepo);
                
                if(result!=null)
                {
                    //Creating the default master branch
                    await branchService.CreateBranchAsync(new BranchEntity
                    {
                        Name = Constants.Constants.MasterBranchName,
                        RepoName = repositoryRequestBody.Name,
                        ParentBranchName = string.Empty,
                        CreationTime = newRepo.CreationTime
                    });

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest("Either repo name already taken or is invalid due to usage of prohibited characters or is longer than 100 characters");
        }

        [HttpPost($"{Constants.Constants.RepoAndBranchName}/")]
        public async Task<ActionResult> CreateBranchForRepository([FromRoute] string repoName, string baseBranch, [FromBody] BranchRequest branchReqBody)
        {
            if (branchReqBody == null) return BadRequest("Request body can not be null");

            try
            {
                var newBranch = new BranchEntity
                {
                    Name = branchReqBody.Name,
                    RepoName = repoName,
                    ParentBranchName = baseBranch,
                    CreationTime = DateTime.Now.ToString()
                };

                var response = await branchService.CreateBranchAsync(newBranch);

                if (!string.IsNullOrEmpty(response))
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete(Constants.Constants.RepositoryName)]
        public async Task<ActionResult> DeleteRepository(string repoName)
        {
            // delete commits under branches as well or atleast put a background worker on it. Implement a flag for marking them deleted so that the worker can delete them later. All commits for a branch in 1 file
            //deleteCommitsAsWell through the deletebranch method
            try
            {
                var totalDeletedBranches = await branchService.DeleteAllBranchesIn(repoName);
                await repoService.DeleteRepoAsync(repoName);

                return Ok(totalDeletedBranches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
