namespace VCS_API.DirectoryDB.Helpers
{
    public static class DBHelper
    {
        public static string AppendDelimited(params string?[] strings) => string.Join(Constants.Constants.StandardColumnDelimiter, strings);
    }
}
