using Domain.BackendResponses;
using Domain.Constants;
using Domain.CoreEnums;
using Domain.Entities.Items.ItemTrading;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services.ItemService.ItemTradingService;
using Domain.Interfaces.Services.ModerationService;
using Domain.Interfaces.Services.ModerationService.DTO;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ItemTradingService
{
    public class ItemTradingModerationService<TEntity> : IModerationService<TEntity> where TEntity : ItemTrading
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly IItemTradingService<TEntity> _itemTradingService;
        public ItemTradingModerationService(ICatalogDbContext catalogDbContext, IItemTradingService<TEntity> itemTradingService)
        {
            _catalogDbContext = catalogDbContext;
            _itemTradingService = itemTradingService;
        }

        public async Task<MethodResult<ItemTradingModerationResponse>> GetNextItemInfo()
        {
            var res = await _itemTradingService.GetItemForModerationAsync();

            if (res.Result == Enums.MethodResults.Conflict)
            {
                return new MethodResult<ItemTradingModerationResponse>(null, res.Messages, res.Result);
            }

            //check by rules. Send messages any way
            //var autoModerationRes = await CheckPreliminarilyAsync()

            return new MethodResult<ItemTradingModerationResponse>(new(res.ResultEntity!, []), [], res.Result);
        }

        /// <summary>
        /// Approve item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<MethodResult> ApproveAsync(Guid itemId, Guid moderatorId)
        {
            var item = await _catalogDbContext.ItemsTrading
                .Include(x => x.ItemTradingStatusHistories)
                .Where(x => x.Id == itemId)
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return new MethodResult(
                    [ResponsesTextConstants.GET_ITEM_BY_ID_ERROR],
                    Enums.MethodResults.Conflict);
            }

            item.ChangeItemStatus(Enums.ItemTradingStatus.Published);

            if (item.SellingType == Enums.SellingTypes.Auction)
            {
                var slots = await _catalogDbContext.ItemTradingAuctionSlots
                    .Include(x => x.StatusHistories)
                    .Where(x => x.ItemId == item.Id)
                    .ToListAsync();

                foreach ( var slot in slots)
                {
                    slot.ChangeStatus(Enums.AuctionSlotStatus.Started);
                }
            }

            await _catalogDbContext.SaveChangesAsync();

            return new MethodResult(
                    [],
                    Enums.MethodResults.Ok);
        }

        /// <summary>
        /// Disapprove item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<MethodResult> DisapproveAsync(Guid itemId, string reason, Guid moderatorId)
        {
            var item = await _catalogDbContext.ItemsTrading
                .Include(x => x.ItemTradingStatusHistories)
                .Where(x => x.Id == itemId)
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return new MethodResult(
                    [ResponsesTextConstants.GET_ITEM_BY_ID_ERROR],
                    Enums.MethodResults.Conflict);
            }

            item.ChangeItemStatus(Enums.ItemTradingStatus.DisapprovedByModerator);

            _catalogDbContext.ItemTradingModerationDisappReasons.Add(new(item, reason, moderatorId));

            await _catalogDbContext.SaveChangesAsync();

            return new MethodResult(
                    [],
                    Enums.MethodResults.Ok);
        }

        /// <summary>
        /// Preliminarily check item for moderation rules
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<MethodResult<List<string>>> CheckPreliminarilyAsync(TEntity item)
        {
            //check title, description by some list of prohibited word/phrases, links, phone numbers etc
            return new MethodResult<List<string>>([], [], Enums.MethodResults.Ok);
        }

        public async Task<int> GetModerationItemsCountAsync()
        {
            return await _catalogDbContext.ItemsTrading.CountAsync(item => item.Status == Enums.ItemTradingStatus.Moderation);
        }

    }
}
