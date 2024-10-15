using System.Text;
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

            if(!File.Exists(filePath) && canCreateDirectory)
            {
                _ = Directory.CreateDirectory(GetDirectoryFromFilePath(filePath!));
                using (File.Create(filePath!)) ; //the using statement will close the file after creation.
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            content = content?.CleanData();
            using var sw = new StreamWriter(filePath, append: append);
            await sw.WriteLineAsync(content);
        }

        private static string GetDirectoryFromFilePath(string filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            return filePath!.Replace(filePath[filePath.LastIndexOf("\\")..], string.Empty);
        }

        public static void WriteToFile(string? filePath, string? content, bool append = true, bool canCreateDirectory = false)// enter false for overwriting the file
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (!File.Exists(filePath) && canCreateDirectory)
            {
                _ = Directory.CreateDirectory(GetDirectoryFromFilePath(filePath!));
                using (File.Create(filePath!)) ; //the using statement will close the file after creation.
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
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (!File.Exists(filePath))
            {
                return false;
            }

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

            if (!File.Exists(filePath))
            {
                return [];
            }

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

            return [.. resultRows];
        }

        public static async Task<string?> FirstOrDefaultRowAsync(string filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (!File.Exists(filePath))
            {
                return null;
            }

            using var reader = new StreamReader(filePath);
            var line = await reader.ReadLineAsync();
            return line?.CleanData();
        }

        public static async Task<string?> FirstOrDefaultRowAsync(string filePath, Func<string, bool> predicate)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if(File.Exists(filePath))
            {
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

        /// <summary>
        /// Searches from last to first. Memory-wise its bad, since we are loading all the data in the memory. 
        /// But it won't be a frequent operation. THIS HAS TO BE OPTIMIZED IN THE FUTURE when we move to a real DB.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<string?> LastOrDefaultRowAsync(string? filePath, Func<string, bool> predicate)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (!File.Exists(filePath))
            {
                return null;
            }

            var allLines = await File.ReadAllLinesAsync(filePath);

            // Process lines in reverse order
            for (int i = allLines.Length - 1; i >= 0; i--)
            {
                var line = allLines[i];

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (predicate(line))
                {
                    return line.CleanData();
                }
            }

            return null;
        }

        /// <summary>
        /// ChatGPT implementation which doesn't load all the data in the memory at once. Untested.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<string?> LastOrDefaultRowAsync2(string filePath, Func<string, bool> predicate)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if (!File.Exists(filePath))
                return null;

            const int bufferSize = 1024; // Buffer size to read chunks from the file
            char[] buffer = new char[bufferSize];
            StringBuilder sb = new StringBuilder();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                long filePosition = fs.Length;
                using (StreamReader reader = new StreamReader(fs))
                {
                    while (filePosition > 0)
                    {
                        // Move the file pointer backwards by bufferSize (or remaining file length)
                        filePosition = Math.Max(filePosition - bufferSize, 0);
                        fs.Seek(filePosition, SeekOrigin.Begin);

                        // Read a chunk of the file
                        int charsRead = await reader.ReadAsync(buffer, 0, bufferSize);
                        for (int i = charsRead - 1; i >= 0; i--)
                        {
                            char currentChar = buffer[i];

                            // Handle line breaks (Unix LF or Windows CRLF)
                            if (currentChar == '\n' || currentChar == '\r')
                            {
                                if (sb.Length > 0)
                                {
                                    var line = sb.ToString();
                                    sb.Clear();

                                    // Process the line if it's not empty and matches the predicate
                                    if (!string.IsNullOrWhiteSpace(line) && predicate(line))
                                    {
                                        return line.CleanData();
                                    }
                                }
                            }
                            else
                            {
                                // Add character to the StringBuilder (reversed direction)
                                sb.Insert(0, currentChar);
                            }
                        }
                    }

                    // Handle any leftover content if the last line doesn't end with a newline
                    if (sb.Length > 0)
                    {
                        var line = sb.ToString();
                        if (!string.IsNullOrWhiteSpace(line) && predicate(line))
                        {
                            return line.CleanData();
                        }
                    }
                }
            }

            return null;
        }

        public static async Task<string?> LastOrDefaultRowAsync(string? filePath)
        {
            Validations.ThrowIfNullOrWhiteSpace(filePath);

            if(!File.Exists(filePath))
            {
                return null;
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
