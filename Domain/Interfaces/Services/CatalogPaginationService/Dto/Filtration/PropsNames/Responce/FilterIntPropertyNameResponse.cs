using System.Text.Json.Serialization;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Responce
{
    public class FilterIntPropertyNameResponse : FilterBasePropertyNameResponse
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        [JsonConstructor]
        public FilterIntPropertyNameResponse(string nameSystemName, string name, int minValue, int maxValue) : base(nameSystemName, name)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
