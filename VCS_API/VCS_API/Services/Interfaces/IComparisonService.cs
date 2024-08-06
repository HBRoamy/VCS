using DiffPlex.DiffBuilder.Model;
using VCS_API.Models;

namespace VCS_API.Services.Interfaces
{
    public interface IComparisonService
    {
        public Task<List<(DiffPiece, DiffPiece)>> GetSideBySideComparisonForCommit(BranchEntity branchEntity);
    }
}