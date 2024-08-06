using System.Linq.Expressions;

namespace VCS_API.Helpers
{
    public class FileHelper : IFileHelper
    {
        private List<string> rows = [];
        private readonly string filePath;

        public FileHelper(string filePath)
        {
            this.filePath = filePath;
            LoadFile();
        }

        private void LoadFile()
        {
            using var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;                    
                }

                rows.Add(line);
            }
        }

        public void AddRow(string newRow)
        {
            rows.Add(newRow);
            WriteCSV();
        }

        public IEnumerable<string> ReadRows(Expression<Func<string, bool>> predicate)
        {
            return rows.AsQueryable().Where(predicate);
        }

        public int DeleteRows(Func<string, bool> predicate)
        {
            int previousCount = rows.Count;
            rows = rows.Where(row => !predicate(row)).ToList();
            WriteCSV();
            return previousCount - rows.Count;
        }

        private void WriteCSV()
        {
            using var writer = new StreamWriter(filePath);
            foreach (var row in rows)
            {
                writer.WriteLine(string.Join(Constants.Constants.StandardColumnDelimiter, row));
            }
        }
    }
}
