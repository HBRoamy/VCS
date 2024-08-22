namespace VCS_API.DirectoryDB.Helpers
{
    public static class DBHelper
    {
        public static string AppendDelimited(params string?[] strings)
        {
            // Join the array of strings with a comma separator
            return string.Join(Constants.Constants.StandardColumnDelimiter, strings);
        }
    }
}
