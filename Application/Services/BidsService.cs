using Domain.BackendResponses;
using Domain.Entities.AuctionSlots;
using Domain.Entities.Bid;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.AuctionService;
using Domain.Interfaces.Services.AuctionService.Dto;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BidsService : IBidsService
    {
        private readonly ICatalogDbContext _catalogContext;
        private readonly IUserNotifier _userNotifier;

        public BidsService(ICatalogDbContext catalogDbContext, IUserNotifier userNotifier) 
        {
            _catalogContext = catalogDbContext;
            _userNotifier = userNotifier;
        }

        public async Task<BidHubSlotRequest?> GetSlotForBidHubInitializingAsync(Guid slotId)
        {
            var slot = await _catalogContext.ItemTradingAuctionSlots
                .Where(i => i.AuctionEndingTime > DateTime.UtcNow && i.Id == slotId)
                .Select(x => new BidHubSlotRequest(
                    x.Id,
                    x.AuctionEndingTime,
                    x.Status))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return slot;
        }

        public async Task<ItemTradingAuctionSlot?> GetSlotForBiddingAsync(Guid slotId)
        {
            var slot = await _catalogContext.ItemTradingAuctionSlots
                .Include(x => x.Bids)
                .FirstOrDefaultAsync(i => i.AuctionEndingTime > DateTime.UtcNow && i.Id == slotId);

            return slot;
        }

        public async Task<MethodResult> ProceedBidAsync(Guid userId, int bidAmount, ItemTradingAuctionSlot slot)
        {
            slot.SetCustomerId(userId);
            //if bids > 0, then notify last bid user of being bit
            if (slot.Bids.Count() > 0)
            {
                var currentBidUserId = slot.Bids.OrderBy(x => x.BidAmount).Last().BidOwnerId;

                await _userNotifier.NotifyBiggestBidOwnerChangedForBidderAsync(currentBidUserId, slot);
            }

            //just add new bid, user is notified (notified if bids count was > 0. if bids count was == 0, no notification is required)
            if (bidAmount < slot.BlitzPrice)
            {
                slot.SetPrice(bidAmount);
                slot.AddBid(new ItemTradingBid(userId, bidAmount, slot));
            }

            //bid is bigger or equal to blitz price, end auction with notifications to winner, item owner and other parts.
            else
            {
                slot.SetPrice(slot.BlitzPrice);
                slot.AddBid(new ItemTradingBid(userId, slot.BlitzPrice, slot));
                slot.SetPendingOrderFormingStartTime(DateTime.UtcNow);
                await FinalizeSlotWithWinnerAsync(userId, slot);
                slot.ChangeStatus(Domain.CoreEnums.Enums.AuctionSlotStatus.EndedWithBids);
            }

            var item = await _catalogContext.ItemsTrading.FirstAsync(x => x.Id == slot.ItemId);

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.LockedByAuction);

            //find min and max slots (only slots which are still in auction) prices and put it into item
            var slots = await _catalogContext.ItemTradingAuctionSlots
                .Where(x => x.ItemId == item.Id && x.Status == Domain.CoreEnums.Enums.AuctionSlotStatus.Started)
                .Select(x => new { x.ItemId, x.Status, x.Price })
                .ToListAsync();

            if (slots.Count > 0)
            {
                var minPrice = slots.MinBy(x => x.Price)!.Price;

                var maxPrice = slots.MaxBy(x => x.Price)!.Price;

                item.MinPrice = minPrice;
                item.MaxPrice = maxPrice;
            }

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        private async Task FinalizeSlotWithWinnerAsync(Guid winnerId, ItemTradingAuctionSlot slot)
        {
            await _userNotifier.NotifyAuctionIsEndedWithWinnerForOwnerAsync(slot.UserId, slot);

            await _userNotifier.NotifyAuctionIsEndedWithVictoryForBidderAsync(winnerId, slot);

            //notify all parts except winner
            foreach (var bid in slot.Bids.DistinctBy(b => b.BidOwnerId).Where(b => b.BidOwnerId != slot.CustomerId))
            {
                await _userNotifier.NotifyAuctionIsEndedWithoutVictoryForBidderAsync(bid.BidOwnerId, slot);
            }
        }
    }
}
