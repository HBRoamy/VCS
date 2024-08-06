using System.Linq.Expressions;
using VCS_API.Extensions;
using VCS_API.Helpers;
using VCS_API.Repositories.Interfaces;

namespace VCS_API.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private static readonly string expectedDirectory = $@"DataWarehouse\{typeof(TEntity).Name.Replace("Entity", "")}";
        private readonly string storageFilePath = $@"DataWarehouse\{typeof(TEntity).Name.Replace("Entity", "")}" + @$"\{typeof(TEntity).Name.Replace("Entity", "Store")}.txt";

        public Repository()
        {
            if (!Directory.Exists(expectedDirectory))
            {
                Directory.CreateDirectory(expectedDirectory);
                Console.WriteLine($"Directory created: {expectedDirectory}");
            }
        }

        public int Count
        {
            get { return 0; }
        }

        public async Task<string[]?> AllRows()
        {
            if (!File.Exists(storageFilePath)) return null;
            return await ReadAllLinesAsync(storageFilePath);
        }

        public TEntity GetById(object id)
        {
            return null;

        }
        public async Task<TEntity> GetByIdAsync(object id)
        {
            return null;

        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = null;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return orderBy != null ? orderBy(query).AsQueryable() : query.AsQueryable();
        }

        public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return null;
        }


        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return true;
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return null;
        }

        public async Task<string?> FindAsync(Expression<Func<string, bool>> predicate)
        {
            if(File.Exists(storageFilePath))
            {
                var repoRows = await ReadAllLinesAsync(storageFilePath);

                var item = repoRows.AsQueryable().SingleOrDefault(predicate);

                if (!string.IsNullOrWhiteSpace(item))
                {
                    return item;
                }
            }
            
            return null;
        }

        public async Task<List<string>?> FindModifiedAsync(string target)
        {
            if (File.Exists(storageFilePath))
            {
                var repoRows = await ReadAllLinesAsync(storageFilePath);

                var item = repoRows.AsQueryable().Where(x => x.Contains(target)).ToList();

                if (!item.IsNullOrEmpty())
                {
                    return item;
                }
            }

            return null;
        }

        public async Task<string?> Create(string itemDetailsWithCommaDelimiter)
        {
            try
            {
                //the following contains logic should got to the find() method
                var itemName = itemDetailsWithCommaDelimiter.Split(Constants.Constants.StandardColumnDelimiter)[0];
                await WriteToFileAsync(storageFilePath, itemDetailsWithCommaDelimiter);
                Console.WriteLine("Item Added to: " + storageFilePath);
                return $"{storageFilePath}{Constants.Constants.ItemAddressDelimiter}{itemName}";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            return null;
        }

        public void Delete(object id)
        {
            var entityToDelete = GetById(id);
            Delete(entityToDelete);
        }
        public async void DeleteAsync(object id)
        {
            var entityToDelete = await GetByIdAsync(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
        }

        public int Delete(Func<string, bool> predicate)
        {
            return new FileHelper(storageFilePath).DeleteRows(predicate);
        }

        public async Task<bool> ContainsAsync(string itemName)
        {
            if (File.Exists(storageFilePath))
            {
                var fileContent = await ReadFileAsync(storageFilePath);
                return fileContent.Contains(itemName, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        private static async Task<string> ReadFileAsync(string filePath)
        {
            using var sr = new StreamReader(filePath);
            return await sr.ReadToEndAsync();
        }

        private static async Task<string[]> ReadAllLinesAsync(string filePath)
        {
            return await File.ReadAllLinesAsync(filePath);
        }

        private static async Task WriteToFileAsync(string filePath, string content, bool append = true)// enter false for overwriting the file
        {
            using var sw = new StreamWriter(filePath, append: append);
            await sw.WriteLineAsync(content);
        }
    }
}
