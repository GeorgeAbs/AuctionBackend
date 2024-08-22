using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class IntPropertyRequest : NumericItemPropertyRequest<int>
    {
        public IntPropertyRequest(int value, string propNameSystemName) : base(value, propNameSystemName) { }
    }
}
