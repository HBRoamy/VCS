using VCS_API.DirectoryDB.Helpers;
using VCS_API.Extensions;
using VCS_API.Helpers;

namespace VCS_API.DirectoryDB
{
    public static class DirectoryDB //Inherits IDirectoryValidator and IRepo
    {
        public static async Task WriteToFileAsync(string? filePath, string? content, bool append = true, bool canCreateDirectory = false)// enter append:false for overwriting the file
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if(canCreateDirectory)
            {
                File.Create(filePath!);
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            content = content?.CleanData();
            using var sw = new StreamWriter(filePath, append: append);
            await sw.WriteLineAsync(content);
        }

        public static void WriteToFile(string? filePath, string? content, bool append = true, bool canCreateDirectory = false)// enter false for overwriting the file
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (canCreateDirectory)
            {
                File.Create(filePath!);
            }

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

        public static async Task<string?> ReadAllTextAsync(string? filePath)
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

        public static async Task<string[]> FilterRowsAsync(string filePath, Func<string, bool> predicate)
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

            return resultRows.ToArray();
        }

        public static async Task<string?> FirstOrDefaultRowAsync(string filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);
            using var reader = new StreamReader(filePath);
            var line = await reader.ReadLineAsync();
            return line?.CleanData();
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

            if (File.Exists(filePath))
            {
                var rows = await GetAllRowsAsync(filePath);

                if (rows == null || rows.Length == 0)
                {
                    throw new KeyNotFoundException("No items found.");
                }

                List<string> newRows = [];
                var deletedRow = default(string);

                foreach (var row in rows)
                {
                    if(predicate(row))
                    {
                        if (!string.IsNullOrWhiteSpace(deletedRow))
                        {
                           throw new InvalidOperationException("More than 1 rows match the search criterion.");
                        }

                        deletedRow = row;
                    }
                    else
                    {
                        newRows.Add(row);
                    }
                }

                if (string.IsNullOrWhiteSpace(deletedRow))
                {
                    throw new KeyNotFoundException("No item matches the search criterion.");
                }

                await WriteToFileAsync(filePath, "", append: false);//emptyig the file first

                foreach (var row in newRows)
                {
                    await WriteToFileAsync(filePath, row);
                }

                return deletedRow.CleanData();
            }
            else throw new FileNotFoundException(filePath);
        }

        public static async Task RemoveAll(string filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (File.Exists(filePath))
            {
                await File.WriteAllTextAsync(filePath, string.Empty);
            }
            else throw new FileNotFoundException(filePath);
        }

        public static async Task<string?> LastOrDefaultRowAsync(string? filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            string? lastLine = null;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                long position = fs.Length - 1;
                int byteValue;

                while (position >= 0)
                {
                    fs.Seek(position, SeekOrigin.Begin);
                    byteValue = fs.ReadByte();

                    if (byteValue == '\n' || byteValue == '\r') // Handling both '\n' and '\r'
                    {
                        if (position < fs.Length - 1) // Check if there is content after the newline
                        {
                            using var sr = new StreamReader(fs, leaveOpen: true);
                            lastLine = await sr.ReadLineAsync();
                            if (!string.IsNullOrWhiteSpace(lastLine))
                            {
                                // Reset the pointer to the start of the file at the end
                                fs.Seek(0, SeekOrigin.Begin);
                                return lastLine;
                            }
                        }
                    }
                    position--;
                }

                // Handle the case where there is only one line without any newline at the end
                if (position < 0)
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    using var sr = new StreamReader(fs, leaveOpen: true);
                    lastLine = await sr.ReadLineAsync();
                }

                // Reset the pointer to the start of the file at the end
                fs.Seek(0, SeekOrigin.Begin);
            }

            return lastLine;
        }
    }
}
