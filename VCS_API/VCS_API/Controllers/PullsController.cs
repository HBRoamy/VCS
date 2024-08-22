using Microsoft.AspNetCore.Mvc;

namespace VCS_API.Controllers
{
    public class PullsController : ControllerBase
    {
        [HttpGet("/")]
        public ActionResult CreatePullRequest([FromBody] string? repoName, [FromBody] string? branchName)
        {
            return Ok();
        }
        //get pull by id
    }
}
