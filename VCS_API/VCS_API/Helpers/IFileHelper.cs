using System.Linq.Expressions;

namespace VCS_API.Helpers
{
    public interface IFileHelper
    {
        public void AddRow(string newRow);
        public IEnumerable<string> ReadRows(Expression<Func<string, bool>> predicate);
        public int DeleteRows(Func<string, bool> predicate);
    }
}
