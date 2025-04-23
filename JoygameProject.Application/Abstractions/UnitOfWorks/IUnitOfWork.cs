using JoygameProject.Application.Abstractions.Repositories;
using JoygameProject.Domain;
using System.Data.Common;

namespace JoygameProject.Application.Abstractions.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IReadRepository<T> Read<T>() where T : BaseEntity;
        IWriteRepository<T> Write<T>() where T : BaseEntity;
        Task SaveAsync();
        Task<List<T>> ExecuteSqlAsync<T>(string storedProcedure, Func<DbDataReader, T> mapper);
    }
}
