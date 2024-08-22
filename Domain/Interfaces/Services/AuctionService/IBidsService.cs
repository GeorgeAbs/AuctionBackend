using Domain.BackendResponses;
using Domain.Entities.AuctionSlots;
using Domain.Interfaces.Services.AuctionService.Dto;

namespace Domain.Interfaces.Services.AuctionService
{
    public interface IBidsService
    {
        public Task<ItemTradingAuctionSlot?> GetSlotForBiddingAsync(Guid itemId);

        public Task<BidHubSlotRequest?> GetSlotForBidHubInitializingAsync(Guid slotId);

        public Task<MethodResult> ProceedBidAsync(Guid userId, int bidAmount, ItemTradingAuctionSlot slot);
    }
}
