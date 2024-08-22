namespace VCS_API.DirectoryDB.Helpers
{
    public static class FileIOExtensions
    {
        private static readonly Dictionary<FileType, string> fileExtensionMap
        = new()
        {
            { FileType.CSharp, ".cs"},
            { FileType.CSS, ".css"},
            { FileType.HTML, ".html"},
            { FileType.Java, ".java"},
            { FileType.Javascript, ".js"},
            { FileType.JSON, ".json"},
            { FileType.Markdown, ".md"},
            { FileType.Python, ".py"},
            { FileType.RazorPage, ".cshtml"},
            { FileType.Text, ".txt"},
            { FileType.Typescript, ".tx"},
            { FileType.XML, ".xml"},
            { FileType.YAML, ".yaml"}
        };

        internal static string AppendExtension(this string name, FileType filetype = FileType.Text) => $"{name}{fileExtensionMap[filetype]}";

        internal static string? CleanData(this string? data)
        {
            if (data == null) return null;
            return data.Replace("\r\n", "\n").Trim();
        }
    }
}
