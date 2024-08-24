using Moq;
using System.Linq.Expressions;
using System.Reflection;
using VCS_API.DirectoryDB;
using VCS_API.Models;
using VCS_API.Repositories.Interfaces;
using VCS_API.Services;

namespace VCS.API.TESTS
{
    public class RepoServiceTests
    {
        private readonly Mock<IRepository<RepositoryEntity>> _mockRepo;

        public RepoServiceTests()
        {
            _mockRepo = new Mock<IRepository<RepositoryEntity>>();
        }

        [Fact]
        public async Task Test_GetAllRepos_ReturnsAllReps()
        {
            var rowResponse = new string[]
            {
                "VCS.Repo1, 02-07-2024 14:20:46, My unique repo description 38ffc99cedad, False",
                "VCS.Repo2, 03-08-2024 10:33:46, My unique repo description 21b36bba, True",
                "Repo_3, 04-08-2024 12:45:46, My unique repo description 21b36bba-b3fd-49fd-955e-38ffc99cedad, False"
            };

            var expectedResult = GetEntityListFromRows(rowResponse);

            _mockRepo.Setup(x => x.AllRows()).ReturnsAsync(rowResponse);

            var sut = await new RepoService(_mockRepo.Object).GetAllRepos();

            Assert.True(CompareLists(expectedResult, sut));
        }

        [Fact]
        public async Task TestReader()
        {
            var result1 = await DirectoryDB.LastOrDefaultRowAsync(@"C:\Work\Personal\PersonalGitRepos\VCS\VCS_API\VCS_API\DataWarehouse\Branch\BranchStore.txt");
            var result2 = await DirectoryDB.LastOrDefaultRowAsync(@"C:\Work\Personal\PersonalGitRepos\VCS\VCS_API\VCS_API\DataWarehouse\Branch\BranchStore.txt");
            var result3 = await DirectoryDB.FirstOrDefaultRowAsync(@"C:\Work\Personal\PersonalGitRepos\VCS\VCS_API\VCS_API\DataWarehouse\Branch\BranchStore.txt");
            var result4 = await DirectoryDB.LastOrDefaultRowAsync(@"C:\Work\Personal\PersonalGitRepos\VCS\VCS_API\VCS_API\DataWarehouse\Branch\BranchStore.txt");
        }

        [Fact]
        public async Task Test_GetRepoByNameAsync_IsFound()
        {
            var expectedResult = new RepositoryEntity
            {
                Name = "VCS.Repo2",
                Description = "My unique repo description 21b36bba",
                CreationTime = "03-08-2024 10:33:46",
                IsPrivate = true,
            };

            _mockRepo.Setup(x => x.FindAsync(It.IsAny<Expression<Func<string, bool>>>())).ReturnsAsync("VCS.Repo2, 03-08-2024 10:33:46, My unique repo description 21b36bba, True");

            var sut = await new RepoService(_mockRepo.Object).GetRepoByNameAsync("VCS.Repo2");

            Assert.True(AreObjectsEqual(expectedResult,sut));
        }

        [Fact]
        public async Task Test_GetRepoByNameAsync_IsNotFound()
        {
            _mockRepo.Setup(x => x.FindAsync(It.IsAny<Expression<Func<string, bool>>>())).ReturnsAsync(default(string?));

            var sut = await new RepoService(_mockRepo.Object).GetRepoByNameAsync("ABCD");

            Assert.Null(sut);
        }

        private static List<RepositoryEntity> GetEntityListFromRows(string[] rows)
        {
            var result = new List<RepositoryEntity>();

            foreach (var row in rows)
            {
                var columns = row.Split(Constants.Constants.StandardColumnDelimiter);

                result.Add(new RepositoryEntity
                {
                    Name = columns[0],
                    CreationTime = columns[1],
                    Description = columns[2],
                    IsPrivate = bool.Parse(columns[3])
                });
            }

            return result;
        }

        public static bool CompareLists<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null && list2 == null) return true;
            if (list1 == null || list2 == null) return false;

            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!AreObjectsEqual(list1[i], list2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AreObjectsEqual<T>(T obj1, T obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null || obj2 == null) return false;

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value1 = property.GetValue(obj1);
                var value2 = property.GetValue(obj2);

                if (!Equals(value1, value2))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
