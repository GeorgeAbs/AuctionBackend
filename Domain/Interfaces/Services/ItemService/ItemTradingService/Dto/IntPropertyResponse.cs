using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class IntPropertyResponse : NumericItemPropertyResponse<int>
    {
        public IntPropertyResponse(int value, string propertyNameSystemName)
            : base(value, propertyNameSystemName) { }
    }
}
