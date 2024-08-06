using VCS_API.Models;

namespace VCS_API.Extensions;

public static class VcsExtensions 
{
    public static bool IsNullOrEmpty<TEntity>(this IEnumerable<TEntity> list)
    {
        return list == null || !list.Any();
    }

    public static bool IsValidName(this string? name, IEnumerable<char> prohibitedCharacters, int maxLength, int minLength = 1 )
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > maxLength || name.Length < minLength) return false;

        foreach (char item in name)
        {
            if (prohibitedCharacters.Contains(item)) return false;
        }

        return true;
    }

    public static string[] GetColumns(this string row, string customDelimiter = Constants.Constants.StandardColumnDelimiter) => row?.Split(customDelimiter) ?? [];
    public static bool IsRootBranch(this BranchEntity branch) => string.Equals(branch.Name, Constants.Constants.MasterBranchName, StringComparison.OrdinalIgnoreCase);
}
