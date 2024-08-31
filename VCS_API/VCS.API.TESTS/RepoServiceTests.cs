using System.Reflection;
using VCS_API.DirectoryDB;

namespace VCS.API.TESTS
{
    public class RepoServiceTests
    {
        public RepoServiceTests()
        {
        }

        [Fact]
        public async Task Test_GetAllRepos_ReturnsAllReps()
        {
            
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
            
        }

        [Fact]
        public async Task Test_GetRepoByNameAsync_IsNotFound()
        {
            
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
