using DiffPlex.DiffBuilder;
using DiffPlex;
using DiffPlex.DiffBuilder.Model;
using VCS_API.DirectoryDB.Helpers;
using VCS_API.Helpers;
using VCS_API.ServicesV2.Interfaces;

namespace VCS_API.ServicesV2
{
    public class PullServiceV2(ICommitServiceV2 commitServiceV2) : IPullServiceV2
    {
        public async Task<List<(DiffPiece, DiffPiece)>?> GetSideBySideComparisonForCommit(string? repoName, string? branchName, string? parentBranchName)
        {
            // get parent's latest commit
            // get self's first (oldest) commit and check if the base commit matches with the parent's latest commit, if yes then proceed, else its a merge conflict.
            // get self's latest commit
            // compare the two, if equal return null else return diff

            try
            {
                Validations.ThrowIfNullOrWhiteSpace(repoName, parentBranchName, branchName);

                //Prevent comparison of master
                if (string.Equals(parentBranchName, Constants.Constants.NullPlaceholder))
                {
                    return null;
                }

                var parentBranchLatestCommit = await commitServiceV2.GetLatestCommitAsync(repoName, parentBranchName);
                Validations.ThrowIfNull(parentBranchLatestCommit);
                var currentBranchFirstCommit = await commitServiceV2.GetOldestCommitAsync(repoName, branchName);
                Validations.ThrowIfNull(currentBranchFirstCommit);

                // Merge conflict if parent branch is ahead of the current branch
                if (!string.Equals(parentBranchLatestCommit?.Hash, currentBranchFirstCommit?.Hash, StringComparison.OrdinalIgnoreCase))
                {
                    return []; //signifies merge conflict
                }

                var currentBranchLatestCommit = await commitServiceV2.GetLatestCommitAsync(repoName, branchName);
                Validations.ThrowIfNull(currentBranchFirstCommit);

                var oldData = parentBranchLatestCommit?.Content?.CleanData();
                var newData = currentBranchLatestCommit?.Content?.CleanData();

                if (string.Equals(newData, oldData))
                {
                    return null; //signifies "NO COMPARISON"
                }

                var diffBuilder = new SideBySideDiffBuilder(new Differ());
                var diffResult = diffBuilder.BuildDiffModel(oldData, newData);

                return diffResult.OldText.Lines.Zip(diffResult.NewText.Lines, (oldLine, newLine) => (oldLine, newLine)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured in the method \'{nameof(GetSideBySideComparisonForCommit)}\' " + ex.Message);
            }

            Console.WriteLine("Reached the end of the comparison method. Could possibly be an issue.");

            return null;
        }
    }
}
