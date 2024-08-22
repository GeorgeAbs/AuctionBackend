using System.Text.Json.Serialization;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Responce
{
    public class FilterFloatPropertyNameResponse : FilterBasePropertyNameResponse
    {
        public float MinValue { get; set; }

        public float MaxValue { get; set; }

        [JsonConstructor]
        public FilterFloatPropertyNameResponse(string nameSystemName, string name, float minValue, float maxValue) : base(nameSystemName, name)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
