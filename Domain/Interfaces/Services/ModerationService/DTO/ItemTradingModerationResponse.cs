using Domain.Interfaces.Services.ItemService.ItemTradingService.Dto;

namespace Domain.Interfaces.Services.ModerationService.DTO
{
    public class ItemTradingModerationResponse : ModerationResponseViewModel<ItemTradingGetItemForModerationResponse>
    {
        public ItemTradingModerationResponse(ItemTradingGetItemForModerationResponse itemInfo, List<string> autoModerationMessages) : base(itemInfo, autoModerationMessages)
        {
        }
    }
}
