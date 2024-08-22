using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class StringPropertyRequest : TextItemPropertyRequest
    {
        public StringPropertyRequest(string systemValue, string propNameSystemName) : base(systemValue, propNameSystemName) { }
    }
}
