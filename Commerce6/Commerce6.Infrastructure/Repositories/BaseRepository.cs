using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Commerce6.Infrastructure.Repositories
{
    public class BaseRepository<T> where T: class
    {
        protected readonly Context Context;
        protected readonly DbSet<T> DbSet;

        protected BaseRepository(Context context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }



        protected IEnumerable<T> FromSqlRaw(string query) => DbSet.FromSqlRaw(query).ToList();
        //=> _dbSet.FromSqlInterpolated(query).ToList();

        protected bool Any(Expression<Func<T, bool>> predicate) => DbSet.AsNoTracking().Any(predicate);



        public T? Find(params object[] keys) => DbSet.Find(keys);

        public void Insert(T entity) => DbSet.Add(entity);

        public void Delete(T entity) => DbSet.Remove(entity);
    }
}
