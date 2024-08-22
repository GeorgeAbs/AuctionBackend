using Domain.BackendResponses;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services.ItemService;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ItemTradingService
{
    public class ItemTradingStatusesService : IItemTradingStatusesService
    {
        private readonly ICatalogDbContext _catalogContext;

        public ItemTradingStatusesService(ICatalogDbContext catalogContext) 
        {
            _catalogContext = catalogContext;
        }

        public async Task<MethodResult> FromTemplateToModerationAsync(Guid userId, Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Template);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Moderation);

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromPublishedToTemplateAsync(Guid userId, Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Published);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Template);

            if (item.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
            {
                var slots = await _catalogContext.ItemTradingAuctionSlots.Where(x => x.ItemId == itemId).ToListAsync();

                foreach (var slot in slots)
                {
                    slot.ChangeStatus(Domain.CoreEnums.Enums.AuctionSlotStatus.Created);
                }
            }

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromModerationToTemplateAsync(Guid userId, Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Moderation);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Template);

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromModerationToPublishedAsync(Guid userId, Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
               .FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Moderation);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            if (item.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
            {
                var slots = await _catalogContext.ItemTradingAuctionSlots.Where(x => x.ItemId == itemId).ToListAsync();

                foreach (var slot in slots)
                {
                    slot.ChangeStatus(Domain.CoreEnums.Enums.AuctionSlotStatus.Started);
                }
            }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Published);

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromDeclinedByModeratorToTemplateAsync(Guid userId, Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.DisapprovedByModerator);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Template);

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromPublishedToLockedByAuctionAsync(Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Published);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.LockedByAuction);

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromPublishedToAuctionIsEndedAsync(Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.Published);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            var slots = await _catalogContext.ItemTradingAuctionSlots.Where(x => x.ItemId == itemId).ToListAsync();

            foreach (var slot in slots)
            {
                slot.ChangeStatus(Domain.CoreEnums.Enums.AuctionSlotStatus.Started);
            }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.AuctionIsEnded);

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromLockedByAuctionToAuctionIsEndedAsync(Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.LockedByAuction);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.AuctionIsEnded);

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> FromAuctionIsEndedToTemplateAsync(Guid userId, Guid itemId)
        {
            var item = await _catalogContext.ItemsTrading
                .FirstOrDefaultAsync(x => x.Id == itemId && x.UserId == userId && x.Status == Domain.CoreEnums.Enums.ItemTradingStatus.AuctionIsEnded);

            if (item == null) { return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Conflict); }

            item.ChangeItemStatus(Domain.CoreEnums.Enums.ItemTradingStatus.Template);

            if (item.SellingType == Domain.CoreEnums.Enums.SellingTypes.Auction)
            {
                var slots = await _catalogContext.ItemTradingAuctionSlots.Where(x => x.ItemId == itemId).ToListAsync();

                foreach (var slot in slots)
                {
                    slot.ChangeStatus(Domain.CoreEnums.Enums.AuctionSlotStatus.Created);
                }
            }

            await _catalogContext.SaveChangesAsync();

            return new MethodResult([], Domain.CoreEnums.Enums.MethodResults.Ok);
        }
    }
}
