using DiffPlex.DiffBuilder;
using DiffPlex;
using DiffPlex.DiffBuilder.Model;
using VCS_API.Extensions;
using VCS_API.Models;
using VCS_API.Repositories;
using VCS_API.Services.Interfaces;

namespace VCS_API.Services
{
    public class ComparisonService : IComparisonService
    {
        //if base commit is old as the parent branch has more changes, simply dont allow comparison
        // get head of parent branch
        // get head of current branch
        // check if the base commit for current branch is the latest commit on parent branch, if not send some message that the branch is behind, render it unmergeable.
        // if all is consistent, fetch the contentful commits from both the parent and current branch
        //compare them and send side by side comparison or the other one.
        
        public async Task<List<(DiffPiece, DiffPiece)>> GetSideBySideComparisonForCommit(BranchEntity branchEntity)
        {
            // think about comparing master branch when it is the only branch
            var parentHead = await CommitRepository.FetchHead(branchEntity.RepoName, branchEntity.ParentBranchName);
            var parentCommitContentPath = parentHead?.GetColumns()[^1];
            var currentBranchHead = await CommitRepository.FetchHead(branchEntity.RepoName, branchEntity.Name);

            ///var currentBranchBaseCommit = currentBranchHead?.GetColumns()[^2].GetColumns("\\")[^1].Replace(".txt", "");
            // get commit hash of the base commit from the first commit row of this branch, then fetch the commit hash of the head of parent branch
            ///if (!string.Equals(parentHead?.GetColumns()[0],currentBranchBaseCommit, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("Current branch is not based on its parent's latest commit. Update the current branch.");
            // create cache to store the diff and a service to update if a commit has been made to the related branch.
            var currentCommitContentPath = currentBranchHead?.GetColumns()[^1];

            var parentCommitContent = await CommitRepository.GetCommittedContentThroughCommitPath(parentCommitContentPath);
            var currentCommitContent = await CommitRepository.GetCommittedContentThroughCommitPath(currentCommitContentPath);

            var diffResult = GenerateDiff(parentCommitContent, currentCommitContent);

            return diffResult.OldText.Lines.Zip(diffResult.NewText.Lines, (oldLine, newLine) => (oldLine, newLine)).ToList();
        }

        private static SideBySideDiffModel GenerateDiff(string? oldString, string? newString)
        {
            var diffBuilder = new SideBySideDiffBuilder(new Differ());
            return diffBuilder.BuildDiffModel(oldString, newString);
        }
    }
}
