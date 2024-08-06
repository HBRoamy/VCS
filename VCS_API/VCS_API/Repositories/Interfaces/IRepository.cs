using System.Linq.Expressions;

namespace VCS_API.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<string[]?> AllRows();
        public TEntity GetById(object id);
        public Task<TEntity> GetByIdAsync(object id);
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        public bool Contains(Expression<Func<TEntity, bool>> predicate);
        public Task<bool> ContainsAsync(string itemName);
        public TEntity Find(Expression<Func<TEntity, bool>> predicate);
        public Task<string?> FindAsync(Expression<Func<string, bool>> predicate);
        public Task<List<string>?> FindModifiedAsync(string target);
        public Task<string?> Create(string itemDetailsWithCommaDelimiter);
        public Task<TEntity> CreateAsync(TEntity entity);
        public void Delete(object id);
        public void DeleteAsync(object id);
        public void Delete(TEntity entity);
        public int Delete(Func<string, bool> predicate);

    }
}
