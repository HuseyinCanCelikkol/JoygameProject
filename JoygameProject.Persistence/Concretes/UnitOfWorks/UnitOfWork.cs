using JoygameProject.Application.Abstractions.Repositories;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Domain;
using JoygameProject.Persistence.Concretes.Repositories;
using JoygameProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace JoygameProject.Persistence.Concretes.UnitOfWorks
{
    public class UnitOfWork(JoygameProjectDbContext context) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _readRepos = [];
        private readonly Dictionary<Type, object> _writeRepos = [];

        public async Task<List<T>> ExecuteSqlAsync<T>(string storedProcedure, Func<DbDataReader, T> mapper)
        {
            // Use ExecuteSqlRawAsync instead of the non-existent ExecuteRawSqlAsync
            var connection = context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = CommandType.StoredProcedure;

            var result = new List<T>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(mapper(reader));
            }

            return result;
        }

        public IReadRepository<T> Read<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!_readRepos.ContainsKey(type))
                _readRepos[type] = new ReadRepository<T>(context);
            return (IReadRepository<T>)_readRepos[type];
        }

        public async Task SaveAsync()
        {
            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync();

                if (context.ChangeTracker.HasChanges())
                {
                    var x = await context.SaveChangesAsync();
                    Console.WriteLine(x + " data changed");
                }

                await transaction.CommitAsync();
            });
        }

        public IWriteRepository<T> Write<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!_writeRepos.ContainsKey(type))
                _writeRepos[type] = new WriteRepository<T>(context);
            return (IWriteRepository<T>)_writeRepos[type];
        }
    }


}
