using Domain.Entities.EntitiesCounters;
using Domain.Interfaces.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntitiesCountersDbContext
{
    public class EntitiesCountersDbContext : DbContext, IEntitiesCountersContext
    {
        public EntitiesCountersDbContext(DbContextOptions<EntitiesCountersDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
        }

        public DbContext Instance => this;

        public DbSet<OrderNumber> OrderNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("entities_counters");
        }
    }
}
