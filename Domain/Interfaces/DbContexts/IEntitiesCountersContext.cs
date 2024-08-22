using Domain.Entities.EntitiesCounters;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.DbContexts
{
    public interface IEntitiesCountersContext : IDbContext
    {
        public DbSet<OrderNumber> OrderNumbers { get; set; }
    }
}
