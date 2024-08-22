using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class StringPropertyResponse : TextItemPropertyResponse
    {
        public StringPropertyResponse(string systemValue, string value, string propertyNameSystemName)
            : base(systemValue, value, propertyNameSystemName) { }
    }
}
