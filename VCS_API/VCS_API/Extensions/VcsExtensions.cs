using VCS_API.Models;

namespace VCS_API.Extensions;

public static class VcsExtensions 
{
    public static bool IsNullOrEmpty<TEntity>(this IEnumerable<TEntity>? list)
    {
        return list == null || !list.Any();
    }

    public static string[] GetColumns(this string row, string customDelimiter = Constants.Constants.StandardColumnDelimiter) => row?.Split(customDelimiter) ?? [];
    public static bool IsRootBranch(this string branch) => string.Equals(branch, Constants.Constants.MasterBranchName, StringComparison.OrdinalIgnoreCase);
}
