using Domain.BackendResponses;
using Domain.Interfaces.Services.ModerationService.DTO;

namespace Domain.Interfaces.Services.ModerationService
{
    public interface IModerationService<TItem> where TItem : class
    {
        public Task<MethodResult<ItemTradingModerationResponse>> GetNextItemInfo();
        /// <summary>
        /// Approve item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Task<MethodResult> ApproveAsync(Guid itemId, Guid moderatorId);

        /// <summary>
        /// Disapprove item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Task<MethodResult> DisapproveAsync(Guid itemId, string reason, Guid moderatorId);

        /// <summary>
        /// Preliminarily check item for moderation rules
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<MethodResult<List<string>>> CheckPreliminarilyAsync(TItem item);

        /// <summary>
        /// Gets moderation items count
        /// </summary>
        /// <returns></returns>
        public Task<int> GetModerationItemsCountAsync();
    }
}
