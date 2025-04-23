using JoygameProject.Domain;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JoygameProject.Application.Abstractions.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetList(Expression<Func<T, bool>>? filter = null, int? skip = null, int? take = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Expression<Func<T, object>>[] orderByList = null, bool orderByDESC = true, bool tracking = false);
        bool Any(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, bool isIgnoreQueryFilter = false);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, bool isIgnoreQueryFilter = false);
        T GetFirst(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, bool isIgnoreQueryFilter = false);
        T GetById(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool tracking = false);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, bool isIgnoreQueryFilter = false);
        Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false);
    }
}
