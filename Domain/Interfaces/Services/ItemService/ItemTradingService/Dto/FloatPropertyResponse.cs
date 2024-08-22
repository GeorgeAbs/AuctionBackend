using Domain.Common.Dto;

namespace Domain.Interfaces.Services.ItemService.ItemTradingService.Dto
{
    public class FloatPropertyResponse : NumericItemPropertyResponse<float>
    {
        public FloatPropertyResponse(float value, string propertyNameSystemName)
            : base(value, propertyNameSystemName) { }
    }
}
