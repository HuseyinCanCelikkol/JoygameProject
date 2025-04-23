using JoygameProject.Application.Abstractions.Repositories;
using JoygameProject.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JoygameProject.Persistence.Concretes.Repositories
{
    public class WriteRepository<T>(DbContext context) : IWriteRepository<T> where T : BaseEntity
    {
        public DbSet<T> Table => context.Set<T>();

        public T Add(T entity)
        {
            return Table.Add(entity).Entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            Table.AddRange(entities);
            return entities;
        }

        public bool Remove(T entity)
        {
            entity.IsActive = false;
            var model = Table.Update(entity);
            return model.State == EntityState.Modified;
        }

        public bool RemoveById(int id)
        {
            var entity = Table.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }
            entity.IsActive = false;
            var model = Table.Update(entity);
            return model.State == EntityState.Modified;
        }

        public bool RemoveRange(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            return Table.Update(entity).Entity;
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            Table.UpdateRange(entities);
            return entities;
        }
    }
}

