using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class BoolPropertyResponse : TextItemPropertyResponse
    {
        public BoolPropertyResponse(string systemValue, string value, string propertyNameSystemName)
            : base(systemValue, value, propertyNameSystemName) { }
    }
}
