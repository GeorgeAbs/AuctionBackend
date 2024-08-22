using Domain.BackendResponses;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto;
using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto.AuctionItem;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService
{
    public interface IItemTradingService<T> where T : class
    {
        /// <summary>
        /// Returns item info for get request
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Task<MethodResult<ItemTradingGetResponse>> GetAsync(Guid itemId);

        public Task<MethodResult<ItemTradingGetItemForModerationResponse>> GetItemForModerationAsync();
        /// <summary>
        /// Returns full template info
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Task<MethodResult<ItemTradingTemplateResponse>> GetItemTemplateAsync(Guid userId, Guid itemId);

        /// <summary>
        /// Creates new item template. If itemId is not provided, creates new item(template)
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Task<MethodResult> CreateUpdateItemAsync(Guid userId, ItemTradingInfoRequest itemInfo);

        public Task<MethodResult> CreateUpdateAuctionItemAsync(Guid userId, ItemTradingAuctionInfoRequest itemInfo);

        public Task<MethodResult> DeleteAsync(Guid userId, Guid itemId);
    }
}
