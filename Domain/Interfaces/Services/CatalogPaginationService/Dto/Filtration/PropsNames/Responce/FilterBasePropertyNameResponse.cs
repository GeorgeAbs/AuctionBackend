namespace Domain.Interfaces.Services.CatalogPaginationService.Dto.Filtration.PropsNames.Responce
{
    public class FilterBasePropertyNameResponse
    {
        public string NameSystemName { get; }

        public string Name { get; }

        public FilterBasePropertyNameResponse(string nameSystemName, string name)
        {
            NameSystemName = nameSystemName;
            Name = name;
        }
    }
}
