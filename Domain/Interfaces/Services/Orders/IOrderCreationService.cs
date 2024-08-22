using Domain.BackendResponses;
using Domain.Entities.AuctionSlots;
using Domain.Entities.Basket;

namespace Domain.Interfaces.Services.Orders
{
    public interface IOrderCreationService
    {
        public Task<MethodResult> CreateOrderFromItemTradingAuctionSlotAsync(Guid slotId);

        public Task<MethodResult> CreateOrderFromItemTradingBasketItemAsync(Guid basketItemId);
    }
}
