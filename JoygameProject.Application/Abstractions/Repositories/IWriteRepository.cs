using JoygameProject.Domain;
using System.Linq.Expressions;

namespace JoygameProject.Application.Abstractions.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        T Add(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        bool Remove(T entity);
        bool RemoveRange(Expression<Func<T, bool>> predicate);
        bool RemoveById(int id);
        T Update(T entity);
        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
    }
}
