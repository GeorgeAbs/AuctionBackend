using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.DbContexts
{
    public interface IDbContext : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbContext Instance { get; }
    }
}
