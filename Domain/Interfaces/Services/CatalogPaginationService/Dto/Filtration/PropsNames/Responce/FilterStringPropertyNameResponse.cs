using Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.Props.Responce;
using System.Text.Json.Serialization;

namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Responce
{
    public class FilterStringPropertyNameResponse : FilterBasePropertyNameResponse
    {
        public IEnumerable<FilterStringProperty> Properties { get; set; }

        [JsonConstructor]
        public FilterStringPropertyNameResponse(string nameSystemName, string name, IEnumerable<FilterStringProperty> properties) : base(nameSystemName, name)
        {
            Properties = properties;
        }
    }
}
