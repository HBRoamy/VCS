using VCS_API.Models;
using VCS_API.Models.RequestModels;

namespace VCS_API.Services.Interfaces
{
    public interface ICommitService
    {
        public Task<string> CommitChanges(CommitEntity commitEntity);
        public Task<string?> GetLatestCommitHashFromBranch(string branchName, string repoName);
        public Task<string?> FetchHead(string repoName, string branchName);
        public Task<string?> GetCommittedContentThroughContentPath(string commitContentPath);
        public Task<CommitEntity?> GetContentfulCommitAsync(CodeRequest request);
    }
}