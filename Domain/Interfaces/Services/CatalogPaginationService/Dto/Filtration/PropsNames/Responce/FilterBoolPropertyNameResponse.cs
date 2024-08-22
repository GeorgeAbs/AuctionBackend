using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.Props.Responce;
using System.Text.Json.Serialization;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Responce
{
    public class FilterBoolPropertyNameResponse : FilterBasePropertyNameResponse
    {
        public IEnumerable<FilterBoolProperty> Properties { get; set; }

        [JsonConstructor]
        public FilterBoolPropertyNameResponse(string nameSystemName, string name, IEnumerable<FilterBoolProperty> properties) : base(nameSystemName, name)
        {
            Properties = properties;
        }
    }
}
