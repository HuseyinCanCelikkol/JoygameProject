using JoygameProject.Domain;
using Microsoft.EntityFrameworkCore;

namespace JoygameProject.Application.Abstractions.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
