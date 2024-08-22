using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class FloatPropertyRequest : NumericItemPropertyRequest<float>
    {
        public FloatPropertyRequest(float value, string propNameSystemName) : base(value, propNameSystemName) { }
    }
}
