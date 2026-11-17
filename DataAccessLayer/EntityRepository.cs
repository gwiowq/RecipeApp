using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Репозиторий через Entity Framework.
    /// </summary>
    public class EntityRepository<T> : IRepository<T>, IReadRepository<T>, IWriteRepository<T> where T : class, IDomainObject
    {
        /// <inheritdoc/>
        public void Add(T entity)
        {
            using var context = new AppDbContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            using var context = new AppDbContext();
            var entity = context.Set<T>().Find(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        /// <inheritdoc/>
        public List<T> ReadAll()
        {
            using var context = new AppDbContext();
            return context.Set<T>().ToList();
        }

        /// <inheritdoc/>
        public T? ReadById(int id)
        {
            using var context = new AppDbContext();
            return context.Set<T>().Find(id);
        }

        /// <inheritdoc/>
        public void Update(T entity)
        {
            using var context = new AppDbContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }
    }
}