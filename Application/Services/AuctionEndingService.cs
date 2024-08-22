using Domain.BackendResponses;
using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Orders;
using Microsoft.EntityFrameworkCore;
using static Domain.CoreEnums.Enums;

namespace Application.Services
{
    public class AuctionEndingService : IAuctionEndingService
    {
        private readonly ICatalogDbContext  _catalogContext;
        private readonly IUserNotifier _userNotifier;
        private readonly IOrderCreationService _orderCreationService;
        public AuctionEndingService(ICatalogDbContext catalogContext, IUserNotifier userNotifier, IOrderCreationService orderCreationService)
        {
            _catalogContext = catalogContext;
            _userNotifier = userNotifier;
            _orderCreationService = orderCreationService;
        }

        public async Task ProceedEndedAuctionAsync()
        {
            int secondsToDelay = 10;

            //find auction items that are out of time
            var s1 = ItemTradingStatus.Published;
            var s2 = ItemTradingStatus.LockedByAuction;

            var items = await _catalogContext.ItemsTrading
                .Where(i => i.SellingType == SellingTypes.Auction &&
                    i.AuctionEndingTime.AddSeconds(secondsToDelay) < DateTime.UtcNow &&
                    (i.Status == s1 || i.Status == s2))
                .ToListAsync();

            foreach (var item in items)
            {
                var res = await ProceedItemAsync(item);
                if (res.Result == MethodResults.Conflict) return;//need log
            }

            await _catalogContext.SaveChangesAsync();
        }

        private async Task<MethodResult> ProceedItemAsync(ItemTrading item)
        {
            //item and slots ending time are equal, so no need to check it again
            var slots = await _catalogContext.ItemTradingAuctionSlots
                .Where(x => x.ItemId == item.Id)
                .ToListAsync();

            //create order where status is EndedWithBids

            switch (item.Status)
            {
                //if item status is Published, it means that no slots have bids by this time
                case ItemTradingStatus.Published:
                    item.ChangeItemStatus(ItemTradingStatus.AuctionIsEnded);

                    await _userNotifier.NotifyAuctionIsEndedForOwnerAsync(item.UserId, item);

                    //all slots are without bids
                    slots.ForEach(s => s.ChangeStatus(AuctionSlotStatus.EndedWithoutBids));
                    break;

                //if item status is LockedByAuction, it means that at least one slot has bids
                case ItemTradingStatus.LockedByAuction:

                    //all slots are out of auction, end auction
                    if (!slots.Any(s => s.Status == AuctionSlotStatus.Started))
                    {
                        item.ChangeItemStatus(ItemTradingStatus.AuctionIsEnded);
                        await _userNotifier.NotifyAuctionIsEndedForOwnerAsync(item.UserId, item); break;
                    }

                    foreach (var slot in slots)
                    {
                        switch (slot.Status)
                        {
                            //nobody won slot
                            case AuctionSlotStatus.Started:
                                if (!slot.Bids.Any())
                                {
                                    slot.ChangeStatus(AuctionSlotStatus.EndedWithoutBids);
                                }

                                //someone won slot
                                else
                                {
                                    slot.ChangeStatus(AuctionSlotStatus.EndedWithBids);

                                    var res = await _orderCreationService.CreateOrderFromItemTradingAuctionSlotAsync(slot.Id);
                                    if (res.Result == MethodResults.Conflict) return res;

                                    await _userNotifier.NotifyAuctionIsEndedWithVictoryForBidderAsync(slot.CustomerId, slot);
                                }
                                break;
                        }
                    }
                    break;
            }

            return new MethodResult([], MethodResults.Ok);
        }
    }
}
