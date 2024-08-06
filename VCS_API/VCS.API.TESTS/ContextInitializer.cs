using Newtonsoft.Json;
using System.Text;
using System;
using VCS_API.Models.RequestModels;
using System.Net;

namespace VCS.API.TESTS
{
    public class ContextInitializer
    {
        private readonly HttpClient _httpClient;
        public ContextInitializer()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7034") };
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "C# program");
        }

        public async Task AutomationTest()
        {
            var repoRequest = new RepositoryRequest
            {
                Name = "VCS.Repo1",
                Description = "My unique repo description " + Guid.NewGuid(),
                IsPrivate = new Random().Next(2)==0,
            };

            var json = JsonConvert.SerializeObject(repoRequest);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoints.RepositoryEndpoint, data);

            var repoResult = string.Empty;

            response.EnsureSuccessStatusCode();
            
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                var branchRequest = new BranchRequest
                {
                    Name = "X_Branch_1",
                };

                repoResult = await response.Content.ReadAsStringAsync();
                var repoName = repoResult.Split(Constants.Constants.ItemAddressDelimiter)[1];

                json = JsonConvert.SerializeObject(branchRequest);
                data = new StringContent(json, Encoding.UTF8, "application/json");
                var branchResponse = await _httpClient.PostAsync(string.Format(Endpoints.RepositoryAndBranchEndpoint, repoName, Constants.Constants.MasterBranchName), data);
            
                if(branchResponse.StatusCode.Equals(HttpStatusCode.OK))
                {
                    //Undo changes
                    var deleteResponse = await _httpClient.DeleteAsync(Endpoints.RepositoryEndpoint + $"/{repoName}");

                    if (deleteResponse.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        Assert.Equal(2, int.Parse(await deleteResponse.Content.ReadAsStringAsync()));
                    }
                }
            }

            //GetByRepoName /Repositories/repoName
            //getallrepo /Repositories
            //create repo POST /repositores
            //CreateBranchForRepository post /repositories/reponame/base/branchname
        }
    }

    internal static class Endpoints
    {
        public const string RepositoryEndpoint = "/api/v1/Repositories";
        public const string RepositoryAndBranchEndpoint = "/api/v1/Repositories/{0}/base/{1}/";
    }

}