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
                // Find the latest commit in the current branch's commit history with a merge commit from the parent branch or the first commit (which was from the parent branch only)
                var latestMergeCommitTask = commitServiceV2.GetLatestMergeCommitOrDefaultAsync(repoName, branchName, parentBranchName);
                //Uncomment it if the new merge implmentation fails.
                ///var currentBranchFirstCommitTask = commitServiceV2.GetOldestCommitAsync(repoName, branchName);

                var currentBranchLatestCommitTask = commitServiceV2.GetLatestCommitAsync(repoName, branchName);

                await Task.WhenAll(parentBranchLatestCommitTask, latestMergeCommitTask, currentBranchLatestCommitTask);

                var parentBranchLatestCommit = await parentBranchLatestCommitTask;
                var currentBranchLatestMergeCommit = await latestMergeCommitTask;
                var currentBranchLatestCommit = await currentBranchLatestCommitTask;

                Validations.ThrowIfNull(parentBranchLatestCommit, currentBranchLatestMergeCommit, currentBranchLatestCommit);


                var comparisonResult = new DiffMergeEntity
                {
                    IsMergeable = true,//default
                    OldChanges = [],
                    NewChanges = [],
                    RepoName = repoName,
                    BranchName = branchName,
                    BaseBranchName = parentBranchName,
                    BranchCommitHash = currentBranchLatestCommit?.Hash,
                    BaseBranchCommitHash = parentBranchLatestCommit?.Hash
                };

                var parentBranchLatestCommitContent = parentBranchLatestCommit?.Content?.CleanData();
                var currentBranchLatestMergeCommitContent = currentBranchLatestMergeCommit?.Content.CleanData(); // find the latest common commit of both branches instead
                var currentBranchLatestCommitContent = currentBranchLatestCommit?.Content.CleanData();
                
                // find the latest common commit


                var currentBranchLatestMergeCommitHash = currentBranchLatestMergeCommit!.Hash!;
                // Potential merge conflict if parent branch is ahead of the current branch
                if (!string.Equals(parentBranchLatestCommit?.Hash, currentBranchLatestMergeCommitHash))
                {
                    //check how much the parent branch has changed since we created a branch from it. The current branch's first commit can be used as a base.
                    var parentNewChanges = GenerateDiff(currentBranchLatestMergeCommitContent, parentBranchLatestCommitContent).NewText.Lines;

                    //check how much the current branch has changed since we created it.
                    var currentNewChanges = GenerateDiff(currentBranchLatestMergeCommitContent, currentBranchLatestCommitContent).NewText.Lines;
                    int j = 0;
                    for (int i = 0; i < parentNewChanges.Count; i++)
                    {
                        if (i < currentNewChanges.Count)
                        {
                            var leftLine = parentNewChanges[i];
                            var rightLine = currentNewChanges[i];
                            int maxLength = Math.Max(parentNewChanges.Count, currentNewChanges.Count);

                            if 
                            (
                                ( leftLine.Type.Equals(ChangeType.Inserted) && rightLine.Type.Equals(ChangeType.Inserted) && leftLine.Text != rightLine.Text) ||
                                ( leftLine.Type.Equals(ChangeType.Modified) && rightLine.Type.Equals(ChangeType.Modified) && leftLine.Text != rightLine.Text) ||
                                ( leftLine.Type.Equals(ChangeType.Modified) && !rightLine.Type.Equals(ChangeType.Modified) )||
                                ( leftLine.Type.Equals(ChangeType.Deleted) && !rightLine.Type.Equals(ChangeType.Deleted) ) ||
                                ( leftLine.Type.Equals(ChangeType.Imaginary) && !rightLine.Type.Equals(ChangeType.Imaginary) )
                            )
                            {
                                // Add a new bool property to mark the line as a conflict
                                comparisonResult.IsMergeable = false;
                            }
                            //else if(leftLine.Type.Equals(rightLine.Type) && leftLine.Text != rightLine.Text)
                            //{
                            //    leftLine.Type = rightLine.Type = ChangeType.Unchanged;
                            //    foreach (var item in leftLine.SubPieces)
                            //    {
                            //        item.Type = ChangeType.Unchanged;
                            //    }
                            //    foreach (var item in rightLine.SubPieces)
                            //    {
                            //        item.Type = ChangeType.Unchanged;
                            //    }
                            //}
                        }
                        else
                        {
                            comparisonResult.IsMergeable = false;
                        }
                    }

                    if(!(comparisonResult.IsMergeable ?? false))
                    {
                        comparisonResult.OldChanges = parentNewChanges;
                        comparisonResult.NewChanges = currentNewChanges;

                        return comparisonResult;
                    }
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

        private void HandleMissingLines(DiffPiece parentLine, DiffPiece currentLine, DiffMergeEntity comparisonResult)
        {
            if (parentLine == null && currentLine != null)
            {
                // Line added in the current branch but not in the parent branch
                comparisonResult.OldChanges?.Add(new DiffPiece { Text = "", Type = ChangeType.Unchanged });
                comparisonResult.NewChanges?.Add(currentLine);
                comparisonResult.IsMergeable = false;
            }
            else if (currentLine == null && parentLine != null)
            {
                // Line removed in the current branch but exists in the parent branch
                comparisonResult.OldChanges?.Add(parentLine);
                comparisonResult.NewChanges?.Add(new DiffPiece { Text = "", Type = ChangeType.Unchanged });
                comparisonResult.IsMergeable = false;
            }
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
