using JoygameProject.Domain;
using JoygameProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameProject.Persistence.Contexts
{
    public class JoygameProjectDbContext(DbContextOptions<JoygameProjectDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleDetail> RoleDetails { get; set; }
        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void OnBeforeSave()
        {
            var entites = ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Select(x => x.Entity.GetType() == typeof(BaseEntity) ? (BaseEntity)x.Entity : x.Entity);
            if (entites.Any())
            {
                PrepareAddedEntities(entites.OfType<BaseEntity>());
            }
            else
            {
                var updatedEntites = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Select(x => (BaseEntity)x.Entity);
                PrepareUpdatedEntities(updatedEntites);
            }
        }

        private static void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsActive = true;
            }
        }

        private static void PrepareUpdatedEntities(IEnumerable<BaseEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (!entity.IsActive)
                {
                    continue;
                }
  
                entity.IsActive = true;
            }
        }
    }
}
