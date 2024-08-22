using Domain.BackendResponses;
using Domain.Entities.Orders;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Orders;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class OrderCreationService : IOrderCreationService
    {
        private readonly IIdentifiersService _identifiersService;

        private readonly ICatalogDbContext _catalogDbContext;

        public OrderCreationService(IIdentifiersService identifiersService, ICatalogDbContext catalogDbContext)
        {
            _identifiersService = identifiersService;
            _catalogDbContext = catalogDbContext;
        }

        public async Task<MethodResult> CreateOrderFromItemTradingAuctionSlotAsync(Guid slotId)
        {

            //var slot = await _catalogDbContext.ItemTradingAuctionSlots
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.Id == slotId);

            //if (slot is null) return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict);

            //var newIdentifier = await _identifiersService.GetOrderNewIdentifierAsync();

            //var newOrder = new ItemTradingOrder(orderIdentifier: newIdentifier,
            //   orderTitle: )

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> CreateOrderFromItemTradingBasketItemAsync(Guid basketItemId)
        {
            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }
    }
}
