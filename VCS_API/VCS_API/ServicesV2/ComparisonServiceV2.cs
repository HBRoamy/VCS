using DiffPlex.DiffBuilder.Model;
using VCS_API.Models;
using VCS_API.Services.Interfaces;

namespace VCS_API.ServicesV2
{
    public class ComparisonServiceV2
    {
        public Task<List<(DiffPiece, DiffPiece)>> GetSideBySideComparisonForCommit(BranchEntity branchEntity)
        {
            throw new NotImplementedException();
        }
    }
}
