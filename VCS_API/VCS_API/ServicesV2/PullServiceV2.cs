using DiffPlex.DiffBuilder;
using DiffPlex;
using DiffPlex.DiffBuilder.Model;
using VCS_API.DirectoryDB.Helpers;
using VCS_API.Helpers;
using VCS_API.ServicesV2.Interfaces;
using VCS_API.Models;

namespace VCS_API.ServicesV2
{
    public class PullServiceV2(ICommitServiceV2 commitServiceV2) : IPullServiceV2
    {
        public async Task<DiffMergeEntity?> GetSideBySideComparisonForCommit(string? repoName, string? branchName, string? parentBranchName)
        {
            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName, parentBranchName, branchName);

                // Prevent comparison of master since it will always be the final destination and will not be merged in to anything
                if (string.Equals(parentBranchName, Constants.Constants.NullPlaceholder))
                {
                    return null;
                }

                var parentBranchLatestCommitTask = commitServiceV2.GetLatestCommitAsync(repoName, parentBranchName);
                var currentBranchFirstCommitTask = commitServiceV2.GetOldestCommitAsync(repoName, branchName);
                var currentBranchLatestCommitTask = commitServiceV2.GetLatestCommitAsync(repoName, branchName);

                await Task.WhenAll(parentBranchLatestCommitTask, currentBranchFirstCommitTask, currentBranchLatestCommitTask);

                var parentBranchLatestCommit = await parentBranchLatestCommitTask;
                var currentBranchFirstCommit = await currentBranchFirstCommitTask;
                var currentBranchLatestCommit = await currentBranchLatestCommitTask;

                Validations.ThrowIfNull(parentBranchLatestCommit, currentBranchFirstCommit, currentBranchLatestCommit);


                var comparisonResult = new DiffMergeEntity
                {
                    RepoName = repoName,
                    BranchName = branchName,
                    BaseBranchName = parentBranchName,
                    BranchCommitHash = currentBranchLatestCommit?.Hash,
                    BaseBranchCommitHash = parentBranchLatestCommit?.Hash
                };

                var parentBranchLatestCommitContent = parentBranchLatestCommit?.Content?.CleanData();
                var currentBranchFirstCommitContent = currentBranchFirstCommit?.Content.CleanData();
                var currentBranchLatestCommitContent = currentBranchLatestCommit?.Content.CleanData();

                var currentBranchFirstCommitBaseHash = SplitAddress(currentBranchFirstCommit!.BaseCommitAddress!)[1];
                // Potential merge conflict if parent branch is ahead of the current branch
                if (!string.Equals(parentBranchLatestCommit?.Hash, currentBranchFirstCommitBaseHash, StringComparison.OrdinalIgnoreCase))
                {
                    //check how much the parent branch has changed since we created a branch from it. The current branch's first commit can be used as a base.
                    var parentNewChanges = GenerateDiff(currentBranchFirstCommitContent, parentBranchLatestCommitContent).NewText.Lines;

                    //check how much the current branch has changed since we created it.
                    var currentNewChanges = GenerateDiff(currentBranchFirstCommitContent, currentBranchLatestCommitContent).NewText.Lines;

                    comparisonResult.IsMergeable = false; // merge conflict
                    comparisonResult.OldChanges = parentNewChanges;
                    comparisonResult.NewChanges = currentNewChanges;

                    return comparisonResult; //signifies merge conflict
                }

                // If the current branch's changes are equal to the parent branch's latest changes, there is nothing merge.
                if (string.Equals(currentBranchLatestCommitContent, parentBranchLatestCommitContent))
                {
                    return comparisonResult; //signifies "NO COMPARISON"
                }

                // else we simply find the diff and return it
                var diffResult = GenerateDiff(parentBranchLatestCommitContent, currentBranchLatestCommitContent);

                comparisonResult.IsMergeable = true;
                comparisonResult.OldChanges = diffResult.OldText.Lines;
                comparisonResult.NewChanges = diffResult.NewText.Lines;

                return comparisonResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetSideBySideComparisonForCommit)}\' " + ex.Message);
            }

            Console.WriteLine("Reached the end of the comparison method. Could possibly be an issue.");

            return null; // an exceptional case
        }

        private static SideBySideDiffModel GenerateDiff(string? oldString, string? newString)
        {
            var diffBuilder = new SideBySideDiffBuilder(new Differ());
            return diffBuilder.BuildDiffModel(oldString, newString);
        }
        private static string[] SplitAddress(string address)
        {
            return address.Split(Constants.Constants.ItemAddressDelimiter);
        }
        /*
         * Test this as well, to determine how fast the parallel ops were
          var comparisonResult = new DiffMergeEntity();
        var parentBranchLatestCommitContent = parentBranchLatestCommit?.Content?.CleanData();
        var currentBranchFirstCommitContent = currentBranchFirstCommit?.Content.CleanData();
        var currentBranchLatestCommitContent = currentBranchLatestCommit?.Content.CleanData();
         */
    }
}
