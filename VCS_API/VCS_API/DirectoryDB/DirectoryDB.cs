using VCS_API.DirectoryDB.Helpers;
using VCS_API.Extensions;
using VCS_API.Helpers;

namespace VCS_API.DirectoryDB
{
    public static class DirectoryDB //Inherits IDirectoryValidator and IRepo
    {
        public static async Task WriteToFileAsync(string? filePath, string? content, bool append = true, bool canCreateDirectory = false)// enter false for overwriting the file
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if(!File.Exists(filePath) && !canCreateDirectory)
            {
                throw new FileNotFoundException(filePath);
            }

            if (content != null)
            {
                content = content.CleanData();
                using var sw = new StreamWriter(filePath, append: append);
                await sw.WriteLineAsync(content);
            }
        }

        public static void WriteToFile(string? filePath, string? content, bool append = true)// enter false for overwriting the file
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            if (content != null)
            {
                content = content.CleanData();
                using var sw = new StreamWriter(filePath, append: append);
                sw.WriteLine(content);
            }
        }

        public static async Task<string?> ReadAllTextAsync(string filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);
            if (!File.Exists(filePath)) return null;
            return (await File.ReadAllTextAsync(filePath)).CleanData();
        }

        public static async Task<string[]?> GetAllRowsAsync(string filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);
            if (!File.Exists(filePath)) return null;
            return await File.ReadAllLinesAsync(filePath);
        }

        public static async Task<bool> AnyRowExistsAsync(string? filePath, Func<string, bool> predicate)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);

            using var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (predicate(line))
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<List<string>> FilterRowsAsync(string filePath, Func<string, bool> predicate)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);
            using var reader = new StreamReader(filePath);
            var resultRows = new List<string>();
            
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (predicate(line))
                {
                    resultRows.Add(line);
                }
            }

            return resultRows;
        }

        public static async Task<string?> FirstOrDefaultRowAsync(string filePath, Func<string, bool> predicate)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);
            using var reader = new StreamReader(filePath);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (predicate(line))
                {
                    return line.CleanData();
                }
            }

            return null;
        }

        public static async Task<string?> DeleteRowAsync(string filePath, Func<string, bool> predicate)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if(File.Exists(filePath))
            {
                var rows = await GetAllRowsAsync(filePath);
                var previousRowCount = rows?.Length;

                rows = rows?.Where(row => !predicate(row)).ToArray();
                var newRowCount = rows?.Length;
                if(rows == null || rows.Length==0)
                {
                    throw new KeyNotFoundException("No item matches the search criterion.");
                }

                if(previousRowCount - newRowCount > 1)
                {
                    throw new InvalidOperationException("More than 1 rows match the search criterion.");
                }
                
                if (previousRowCount - newRowCount == 0)
                {
                    throw new KeyNotFoundException("No item matches the search criterion.");
                }

                using var writer = new StreamWriter(filePath);
                await writer.WriteLineAsync(rows[0]);
                return rows[0].CleanData();
            }
            else throw new FileNotFoundException(filePath);
        }

        public static async Task RemoveAll(string filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if(File.Exists(filePath))
            {
                await File.WriteAllTextAsync(filePath, string.Empty);
            }
            else throw new FileNotFoundException(filePath);
        }
    }
}
