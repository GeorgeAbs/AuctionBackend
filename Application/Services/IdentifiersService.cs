using Domain.Entities.EntitiesCounters;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;

namespace Application.Services
{
    public class IdentifiersService : IIdentifiersService
    {
        private readonly IEntitiesCountersContext _context;

        public IdentifiersService(IEntitiesCountersContext context)
        {
            _context = context;
        }

        public async Task<string> GetOrderNewIdentifierAsync()
        {
            var newNumber = new OrderNumber();

            _context.OrderNumbers.Add(newNumber);

            await _context.SaveChangesAsync();

            var monthPart = DateTime.UtcNow.Month < 10 ? $"0{DateTime.UtcNow.Month}" : DateTime.UtcNow.Month.ToString();

            var newIdentifier = $"{DateTime.UtcNow.Year - 2000}{monthPart}/{newNumber.Id}";

            return newIdentifier ;
        }
    }
}
