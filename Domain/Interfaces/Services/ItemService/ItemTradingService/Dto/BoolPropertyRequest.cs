using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class BoolPropertyRequest : TextItemPropertyRequest
    {
        public BoolPropertyRequest(string systemValue, string propNameSystemName) : base(systemValue, propNameSystemName) { }
    }
}
