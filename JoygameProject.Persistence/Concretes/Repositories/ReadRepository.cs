using JoygameProject.Application.Abstractions.Repositories;
using JoygameProject.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JoygameProject.Persistence.Concretes.Repositories
{
    public class ReadRepository<T>(DbContext context) : IReadRepository<T> where T : BaseEntity
    {
        public DbSet<T> Table => context.Set<T>();

        public bool Any(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, bool isIgnoreQueryFilter = false)
        {
            IQueryable<T> table;
            if (isIgnoreQueryFilter)
            {
                table = Table.IgnoreQueryFilters();
            }
            else
            {
                table = Table.AsQueryable();
            }
            if (!tracking)
            {
                table = table.AsNoTracking();
            }
            if (include != null)
            {
                table = include(table);
            }

            return table.Any(filter);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, bool isIgnoreQueryFilter = false)
        {
            var table = Table.AsQueryable();

            if (!tracking)
            {
                table = table.AsNoTracking();
            }
            if (include != null)
            {
                table = include(table);
            }
            return await table.AnyAsync(filter);
        }

        public T GetById(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false)
        {
            var table = Table.AsQueryable();
            if (!tracking)
            {
                table = table.AsNoTracking();
            }
            if (include != null)
            {
                table = include(table);
            }
            return table.FirstOrDefault(x => x.Id == id);
        }

        public async Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false)
        {
            var table = Table.AsQueryable();
            if (!tracking)
            {
                table = table.AsNoTracking();
            }
            if (include != null)
            {
                table = include(table);
            }
            return await table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public T GetFirst(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, bool isIgnoreQueryFilter = false)
        {
            IQueryable<T> table;
            if (isIgnoreQueryFilter)
            {
                table = Table.IgnoreQueryFilters();
            }
            else
            {
                table = Table.AsQueryable();
            }
            if (!tracking)
            {
                table.AsNoTracking();
            }
            if (include != null)
            {
                table = include(table);
            }
            return table.FirstOrDefault(filter);
        }

        public async Task<T> GetFirstAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool tracking = false,
            bool isIgnoreQueryFilter = false)
        {
            IQueryable<T> table = isIgnoreQueryFilter
                ? Table.IgnoreQueryFilters()
                : Table.AsQueryable();

            if (!tracking)
                table = table.AsNoTracking(); // ✅ ŞİMDİ EF “Peki kralım” diyecek

            if (include != null)
                table = include(table);

            return await table.FirstOrDefaultAsync(filter);
        }


        public IQueryable<T> GetList(Expression<Func<T, bool>>? filter = null, int? skip = null, int? take = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Expression<Func<T, object>>[] orderByList = null, bool orderByDESC = true, bool tracking = true)
        {
            var table = Table.AsQueryable();
            if (!tracking)
            {
                table = table.AsNoTracking();
            }
            if (filter != null) table = Table.Where(filter);
            if (skip.HasValue) table = table.Skip(skip.GetValueOrDefault());
            if (take.HasValue) table = table.Take(take.GetValueOrDefault());
            if (orderByList != null)
            {
                table = ApplyOrderByProperties(table, orderByList, orderByDESC);
            }
            if (include != null)
            {
                table = include(table);
            }
            return table;
        }
        private static IQueryable<T> ApplyOrderByProperties(IQueryable<T> query, Expression<Func<T, object>>[] orderByList, bool orderByDESC)
        {
            if (orderByList != null)
            {
                query = orderByList.Aggregate(query, (current, orderBy) => orderByDESC ? current.OrderByDescending(orderBy) : current.OrderBy(orderBy));
            }
            return query;
        }
    }
}
